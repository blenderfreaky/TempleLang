namespace TempleLang.Lexer
{
    using Abstractions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Lexer
    {
        private static readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
        {
            ["if"] = TokenType.If,
            ["if"] = TokenType.Else,
            ["for"] = TokenType.For,
            ["While"] = TokenType.While,
        };

        public static IEnumerable<Token<TokenType, SourceFile>> Tokenize(
            TextReader textReader, SourceFile sourceFile)
        {
            int tokenIndex = 0, tokenCharStartIndex = 0, currentCharIndex = 0;

            StringBuilder buffer = new StringBuilder();

            while (true)
            {
                int characterInt = readChar();

                if (characterInt == -1)
                {
                    yield break;
                }

                char character = (char)characterInt;
                buffer.Append(character);

                switch (character)
                {
                    case '+':
                        yield return makeToken(switchOnNextCharacter(
                            TokenType.Add,
                            ('=', TokenType.AddCompoundAssign)));
                        continue;
                    case '-':
                        yield return makeToken(switchOnNextCharacter(
                            TokenType.Subtract,
                            ('=', TokenType.SubtractCompoundAssign)));
                        continue;
                    case '*':
                        yield return makeToken(switchOnNextCharacter(
                            TokenType.Multiply,
                            ('=', TokenType.MultiplyCompoundAssign)));
                        continue;
                    case '/':
                        yield return makeToken(switchOnNextCharacter(
                            TokenType.Divide,
                            ('=', TokenType.DivideCompoundAssign)));
                        continue;
                    case '%':
                        yield return makeToken(switchOnNextCharacter(
                            TokenType.Remainder,
                            ('=', TokenType.RemainderCompoundAssign)));
                        continue;
                    case '!':
                        yield return makeToken(
                            switchOnNextCharacter(TokenType.Not,
                            ('=', TokenType.ComparisonNotEqual)));
                        continue;
                    case '~':
                        yield return makeToken(
                            TokenType.BitwiseNot);
                        continue;
                    case '^':
                        yield return makeToken(switchOnNextCharacter(
                            TokenType.BitwiseXor,
                            ('=', TokenType.BitwiseXorCompoundAssign)));
                        continue;
                    case '=':
                        yield return makeToken(switchOnNextCharacter(
                            TokenType.Assign,
                            ('=', TokenType.ComparisonEqual)));
                        continue;
                    case '&':
                        var andType = switchOnNextCharacter(
                            TokenType.BitwiseAnd,
                            ('=', TokenType.BitwiseAndCompoundAssign),
                            ('&', TokenType.And));
                        if (andType == TokenType.And)
                        {
                            andType = switchOnNextCharacter(
                                TokenType.And,
                                ('=', TokenType.AndCompoundAssign));
                        }
                        yield return makeToken(andType);
                        continue;
                    case '|':
                        var orType = switchOnNextCharacter(
                            TokenType.BitwiseOr,
                            ('=', TokenType.BitwiseOrCompoundAssign),
                            ('|', TokenType.Or));
                        if (orType == TokenType.Or)
                        {
                            orType = switchOnNextCharacter(
                                TokenType.Or,
                                ('=', TokenType.OrCompoundAssign));
                        }
                        yield return makeToken(orType);
                        continue;
                    case '<':
                        var lessType = switchOnNextCharacter(
                            TokenType.ComparisonLessThan,
                            ('=', TokenType.ComparisonLessOrEqualThan),
                            ('<', TokenType.BitshiftLeft));
                        if (lessType == TokenType.BitshiftLeft)
                        {
                            lessType = switchOnNextCharacter(
                                TokenType.BitshiftLeft,
                                ('=', TokenType.BitshiftLeftCompoundAssign));
                        }
                        yield return makeToken(lessType);
                        continue;
                    case '>':
                        var greaterType = switchOnNextCharacter(
                            TokenType.ComparisonGreaterThan,
                            ('=', TokenType.ComparisonGreaterOrEqualThan),
                            ('>', TokenType.BitshiftRight));
                        if (greaterType == TokenType.BitshiftRight)
                        {
                            greaterType = switchOnNextCharacter(
                                TokenType.BitshiftRight,
                                ('=', TokenType.BitshiftRightCompoundAssign));
                        }
                        yield return makeToken(greaterType);
                        continue;
                    case '\'':
                        lexEscapableChar();
                        lexChar('\'', "lexing Char literal");
                        yield return makeToken(TokenType.CharacterLiteral);
                        continue;
                    case '\"':
                        while (true)
                        {
                            int nextCharacterInt = peekChar();
                            
                            if (nextCharacterInt == -1)
                            {
                                throw new UnexpectedCharException(null, "a char or a string delimiter", "lexing String literal");
                            }
                            
                            char nextCharacter = (char)nextCharacterInt;

                            if (nextCharacter == '\"')
                            {
                                buffer.Append(nextCharacter);
                                break;
                            }

                            lexEscapableChar();
                        }
                        yield return makeToken(TokenType.StringLiteral);
                        continue;
                    case '{':
                        yield return makeToken(TokenType.LeftCodeDelimiter);
                        continue;
                    case '}':
                        yield return makeToken(TokenType.RightCodeDelimiter);
                        continue;
                    case '[':
                        yield return makeToken(TokenType.LeftEnumerationDelimiter);
                        continue;
                    case ']':
                        yield return makeToken(TokenType.RightEnumerationDelimiter);
                        continue;
                    case '(':
                        yield return makeToken(TokenType.LeftExpressionDelimiter);
                        continue;
                    case ')':
                        yield return makeToken(TokenType.RightExpressionDelimiter);
                        continue;
                    case ';':
                        yield return makeToken(TokenType.StatementDelimiter);
                        continue;
                }

                if (char.IsLetter(character) || character == '_')
                {
                    while (true)
                    {
                        int nextCharacterInt = peekChar();

                        if (nextCharacterInt == -1) break;

                        char nextCharacter = (char)nextCharacterInt;

                        if (!char.IsLetterOrDigit(nextCharacter) && nextCharacter != '_') break;

                        readChar();

                        buffer.Append((char)nextCharacterInt);
                    }

                    yield return _keywords.TryGetValue(buffer.ToString(), out TokenType keyword) 
                        ? makeToken(keyword)
                        : makeToken(TokenType.Identifier);
                    continue;
                }

                bool dot = false;

                if (character == '.')
                {
                    buffer.Append(character);

                    dot = true;

                    int nextCharacterInt = peekChar();

                    if (nextCharacterInt == -1)
                    {
                        throw new UnexpectedCharException(null, "Digit", "lexing " + nameof(TokenType.RealLiteral));
                    }

                    character = (char)nextCharacterInt;

                    if (!char.IsDigit(character))
                    {
                        throw new UnexpectedCharException(character, "Digit", "lexing " + nameof(TokenType.RealLiteral));
                    }
                }

                if (char.IsDigit(character))
                {
                    while (true)
                    {
                        int nextCharacterInt = peekChar();

                        if (nextCharacterInt == -1) break;

                        char nextCharacter = (char)nextCharacterInt;

                        if (!char.IsDigit(nextCharacter))
                        {
                            if (!dot && nextCharacter == '.')
                            {
                                dot = true;
                            }
                            else
                            {
                                break;
                            }
                        }

                        readChar();

                        buffer.Append(nextCharacter);
                    }

                    yield return dot
                        ? makeToken(TokenType.RealLiteral)
                        : makeToken(TokenType.IntegerLiteral);
                    continue;
                }
            }

            int peekChar()
            {
                return textReader.Peek();
            }

            int readChar()
            {
                currentCharIndex++;
                return textReader.Read();
            }

            Token<TokenType, SourceFile> makeToken(TokenType tokenType)
            {
                var token = new Token<TokenType, SourceFile>(buffer.ToString(), tokenType, sourceFile, tokenIndex, tokenCharStartIndex, currentCharIndex);

                tokenCharStartIndex = ++tokenIndex;
                buffer.Clear();

                return token;
            }

            /*TokenType switchOnNextCharacter(TokenType fallback, Dictionary<char, TokenType> options)
            {
                int characterInt = peekChar();
                if (characterInt == -1) return fallback;
                if (options.TryGetValue((char)characterInt, out TokenType next))
                {
                    buffer.Append((char)characterInt);
                    readChar();
                    return next;
                }
                return fallback;
            }*/

            TokenType switchOnNextCharacter(TokenType fallback, params (char character, TokenType tokenType)[] options)
            {
                int characterInt = peekChar();
                if (characterInt == -1) return fallback;

                var match = Array.Find(options, x => x.character == (char)characterInt);

                if (match != default)
                {
                    buffer.Append((char)characterInt);
                    readChar();
                    return match.tokenType;
                }

                return fallback;
            }

            void lexEscapableChar()
            {
                int characterInt = peekChar();
                
                if (characterInt == -1) return;

                char character = (char)characterInt;

                if (character == '\\')
                {
                    buffer.Append(character);
                    readChar();

                    int nextCharacterInt = peekChar();

                    if (nextCharacterInt == -1) throw new UnexpectedCharException(null, "escapable char", "lexing escaped character");

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
                            readChar();
                            break;
                        case 'u':
                            readChar();
                            lexHexDigit();
                            lexHexDigit();
                            lexHexDigit();
                            lexHexDigit();
                            break;
                    }

                    buffer.Append(nextCharacter);
                }
            }

            void lexHexDigit()
            {
                int characterInt = peekChar();

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
                    throw new UnexpectedCharException(character, "Hex digit", "lexing Hex digit");
                }

                readChar();
            }

            void lexChar(char characterToMatch, string context)
            {
                int characterInt = peekChar();

                if (characterInt == -1) return;

                char character = (char)characterInt;

                if (character != characterToMatch)
                {
                    throw new UnexpectedCharException(character, context, characterToMatch);
                }

                readChar();
            }
        }
    }
}