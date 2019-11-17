namespace TempleLang.Lexer
{
    using TempleLang.Lexer.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using TempleLang.Lexer.Abstractions.Exceptions;

    /// <summary>
    /// Represents a lexer, able to lex the input from a <see cref="TextReader"/>
    /// </summary>
    /// <typeparam name="TLexeme">The Type of the lexeme the lexer returns. Must implement <see cref="ILexeme{TToken, TSourceFile}"/></typeparam>
    /// <typeparam name="TToken">The Token Type used by the Lexeme</typeparam>
    public class Lexer : ILexer<Lexeme<Token, SourceFile>, Token, SourceFile>
    {
        private static readonly Dictionary<string, Token> _keywords = new Dictionary<string, Token>
        {
            ["if"] = Token.If,
            ["else"] = Token.Else,
            ["for"] = Token.For,
            ["while"] = Token.While,
        };

        public Lexer(TextReader textReader, SourceFile sourceFile)
        {
            TextReader = textReader;
            SourceFile = sourceFile;

            TokenBuffer = new StringBuilder();
        }

        private int TokenIndex, TokenCharStartIndex, CurrentCharIndex;
        private readonly StringBuilder TokenBuffer;
        private readonly TextReader TextReader;
        private readonly SourceFile SourceFile;

        /// <summary>
        /// Lexes the text far enough to generate one lexeme
        /// </summary>
        /// <returns>The topmost token from the text</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Lexeme<Token, SourceFile> LexOne()
        {
            int characterInt;
            char character;

            do
            {
                characterInt = AdvanceChar();

                if (characterInt == -1)
                {
                    return MakeLexeme(Token.EoF);
                }

                character = (char)characterInt;
            }
            while (char.IsWhiteSpace(character));

            TokenBuffer.Append(character);

            switch (character)
            {
                case '+':
                    return MakeLexeme(SwitchOnNextCharacter(
                       Token.Add,
                       ('=', Token.AddCompoundAssign)));
                case '-':
                    return MakeLexeme(SwitchOnNextCharacter(
                       Token.Subtract,
                       ('=', Token.SubtractCompoundAssign)));
                case '*':
                    return MakeLexeme(SwitchOnNextCharacter(
                       Token.Multiply,
                       ('=', Token.MultiplyCompoundAssign)));
                case '/':
                    return MakeLexeme(SwitchOnNextCharacter(
                       Token.Divide,
                       ('=', Token.DivideCompoundAssign)));
                case '%':
                    return MakeLexeme(SwitchOnNextCharacter(
                       Token.Remainder,
                       ('=', Token.RemainderCompoundAssign)));
                case '!':
                    return MakeLexeme(
                       SwitchOnNextCharacter(Token.Not,
                       ('=', Token.ComparisonNotEqual)));
                case '~':
                    return MakeLexeme(
                       Token.BitwiseNot);
                case '^':
                    return MakeLexeme(SwitchOnNextCharacter(
                       Token.BitwiseXor,
                       ('=', Token.BitwiseXorCompoundAssign)));
                case '=':
                    return MakeLexeme(SwitchOnNextCharacter(
                       Token.Assign,
                       ('=', Token.ComparisonEqual)));
                case '&':
                    var andType = SwitchOnNextCharacter(
                        Token.BitwiseAnd,
                        ('=', Token.BitwiseAndCompoundAssign),
                        ('&', Token.And));
                    if (andType == Token.And)
                    {
                        andType = SwitchOnNextCharacter(
                            Token.And,
                            ('=', Token.AndCompoundAssign));
                    }
                    return MakeLexeme(andType);
                case '|':
                    var orType = SwitchOnNextCharacter(
                        Token.BitwiseOr,
                        ('=', Token.BitwiseOrCompoundAssign),
                        ('|', Token.Or));
                    if (orType == Token.Or)
                    {
                        orType = SwitchOnNextCharacter(
                            Token.Or,
                            ('=', Token.OrCompoundAssign));
                    }
                    return MakeLexeme(orType);
                case '<':
                    var lessType = SwitchOnNextCharacter(
                        Token.ComparisonLessThan,
                        ('=', Token.ComparisonLessOrEqualThan),
                        ('<', Token.BitshiftLeft));
                    if (lessType == Token.BitshiftLeft)
                    {
                        lessType = SwitchOnNextCharacter(
                            Token.BitshiftLeft,
                            ('=', Token.BitshiftLeftCompoundAssign));
                    }
                    return MakeLexeme(lessType);
                case '>':
                    var greaterType = SwitchOnNextCharacter(
                        Token.ComparisonGreaterThan,
                        ('=', Token.ComparisonGreaterOrEqualThan),
                        ('>', Token.BitshiftRight));
                    if (greaterType == Token.BitshiftRight)
                    {
                        greaterType = SwitchOnNextCharacter(
                            Token.BitshiftRight,
                            ('=', Token.BitshiftRightCompoundAssign));
                    }
                    return MakeLexeme(greaterType);
                case '\'':
                    LexEscapableChar(Token.CharacterLiteral);
                    LexChar('\'', Token.CharacterLiteral);
                    return MakeLexeme(Token.CharacterLiteral);
                case '\"':
                    while (true)
                    {
                        int nextCharacterInt = PeekChar();

                        if (nextCharacterInt == -1)
                        {
                            throw UnexpectedCharException.Create(null, Token.StringLiteral, "a char or a string delimiter");
                        }

                        char nextCharacter = (char)nextCharacterInt;

                        if (nextCharacter == '\"')
                        {
                            TokenBuffer.Append(nextCharacter);
                            break;
                        }

                        LexEscapableChar(Token.StringLiteral);
                    }
                    return MakeLexeme(Token.StringLiteral);
                case '{':
                    return MakeLexeme(Token.LeftCodeDelimiter);
                case '}':
                    return MakeLexeme(Token.RightCodeDelimiter);
                case '[':
                    return MakeLexeme(Token.LeftEnumerationDelimiter);
                case ']':
                    return MakeLexeme(Token.RightEnumerationDelimiter);
                case '(':
                    return MakeLexeme(Token.LeftExpressionDelimiter);
                case ')':
                    return MakeLexeme(Token.RightExpressionDelimiter);
                case ';':
                    return MakeLexeme(Token.StatementDelimiter);
            }

            // Identifier or keyword
            if (char.IsLetter(character) || character == '_')
            {
                while (true)
                {
                    int nextCharacterInt = PeekChar();

                    if (nextCharacterInt == -1) break;

                    char nextCharacter = (char)nextCharacterInt;

                    if (!char.IsLetterOrDigit(nextCharacter) && character != '_') break;

                    AdvanceChar();

                    TokenBuffer.Append(nextCharacter);
                }

                return MakeLexeme(_keywords.TryGetValue(TokenBuffer.ToString(), out Token keyword) ? keyword : Token.Identifier);
            }

            // Numerical literal
            {
                bool reachedDot = false;

                if (character == '.')
                {
                    reachedDot = true;

                    int nextCharacterInt = PeekChar();

                    if (nextCharacterInt == -1)
                    {
                        throw UnexpectedCharException.Create(null, Token.FloatLiteral, "Digit");
                    }

                    character = (char)nextCharacterInt;

                    if (!char.IsDigit(character))
                    {
                        throw UnexpectedCharException.Create(character, Token.FloatLiteral, "Digit");
                    }
                }

                if (char.IsDigit(character))
                {
                    LexNumber(ref reachedDot);

                    return reachedDot
                       ? MakeLexeme(Token.FloatLiteral)
                       : MakeLexeme(Token.IntegerLiteral);
                }
            }

            throw UnexpectedCharException.Create(character, Token.EoF, "EoF");
        }

        private void LexNumber(ref bool reachedDot)
        {
            while (true)
            {
                int nextCharacterInt = PeekChar();

                if (nextCharacterInt == -1) break;

                char nextCharacter = (char)nextCharacterInt;

                if (!char.IsDigit(nextCharacter))
                {
                    if (!reachedDot && nextCharacter == '.')
                    {
                        reachedDot = true;
                    }
                    else
                    {
                        break;
                    }
                }

                AdvanceChar();

                TokenBuffer.Append(nextCharacter);
            }
        }

        private int PeekChar()
        {
            return TextReader.Peek();
        }

        private int AdvanceChar()
        {
            CurrentCharIndex++;
            return TextReader.Read();
        }

        private void LexChar(char characterToMatch, Token context)
        {
            int characterInt = PeekChar();

            if (characterInt == -1) return;

            char character = (char)characterInt;

            if (character != characterToMatch)
            {
                throw UnexpectedCharException.Create(character, context, characterToMatch);
            }

            AdvanceChar();
        }

        private Lexeme<Token, SourceFile> MakeLexeme(Token tokenType)
        {
            var token = new Lexeme<Token, SourceFile>(TokenBuffer.ToString(), tokenType, SourceFile, TokenIndex, TokenCharStartIndex, CurrentCharIndex);

            TokenCharStartIndex = CurrentCharIndex + 1;
            TokenIndex++;
            TokenBuffer.Clear();

            return token;
        }

        private Token SwitchOnNextCharacter(Token fallback, params (char character, Token tokenType)[] options)
        {
            int characterInt = PeekChar();
            if (characterInt == -1) return fallback;

            var match = Array.Find(options, x => x.character == (char)characterInt);

            if (match != default)
            {
                TokenBuffer.Append((char)characterInt);
                AdvanceChar();
                return match.tokenType;
            }

            return fallback;
        }

        private void LexEscapableChar(Token context)
        {
            int characterInt = PeekChar();

            if (characterInt == -1) return;

            char character = (char)characterInt;

            if (character == '\\')
            {
                TokenBuffer.Append(character);
                AdvanceChar();

                int nextCharacterInt = PeekChar();

                if (nextCharacterInt == -1) throw UnexpectedCharException.Create(null, context, "escapable char");

                char nextCharacter = (char)nextCharacterInt;

                switch (nextCharacter)
                {
                    case '\'':
                    case '"':
                    case '\\':
                    case 'n':
                    case 'r':
                    case 't':
                    case '0':
                        AdvanceChar();
                        break;
                    case 'u':
                        AdvanceChar();
                        LexHexDigit(context);
                        LexHexDigit(context);
                        LexHexDigit(context);
                        LexHexDigit(context);
                        break;
                }

                TokenBuffer.Append(nextCharacter);
            }
            else
            {
                TokenBuffer.Append(character);
            }
        }

        private void LexHexDigit(Token context)
        {
            int characterInt = PeekChar();

            if (characterInt == -1) return;

            char character = (char)characterInt;

            if (!char.IsDigit(character)
                && character != 'a' && character != 'A'
                && character != 'b' && character != 'B'
                && character != 'c' && character != 'C'
                && character != 'd' && character != 'D'
                && character != 'e' && character != 'E'
                && character != 'f' && character != 'F')
            {
                throw UnexpectedCharException.Create(character, context, "Hex digit");
            }

            AdvanceChar();
        }
    }
}