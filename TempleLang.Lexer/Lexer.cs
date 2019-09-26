namespace TempleLang.Lexer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TempleLang.Lexer.Abstractions;

    public class Lexer : ILexer<Token, TokenType>
    {
        public Lexer()
        {
        }

        public static readonly IReadOnlyDictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>
        {
            ["then"] = TokenType.StatementDelimiter,
            ["we want a"] = TokenType.Declarator,
            ["give back"] = TokenType.KeywordReturn,
            ["is"] = TokenType.KeywordIf,
            ["if not"] = TokenType.KeywordElse,
            ["as long as"] = TokenType.KeywordWhile,
            ["loop"] = TokenType.KeywordFor,
            ["not"] = TokenType.Not,
        };

        public IEnumerable<Token> Step(TextReader textReader, IFile file)
        {
            int currentTokenIndex = -1;
            int currentCharIndex = -1;

            while (true)
            {
                var currentInt = readChar();

                if (currentInt == -1)
                {
                    yield break;
                }

                char currentChar = (char)currentInt;

                StringBuilder buffer = new StringBuilder();
                buffer.Append(currentChar);

                int startIndex = currentCharIndex;

                Token makeToken(TokenType tokenType, string? text = null) =>
                    new Token(
                        text ?? buffer.ToString(),
                        tokenType,
                        file,
                        currentTokenIndex,
                        startIndex,
                        currentCharIndex - 1);

                if (char.IsLetter(currentChar))
                {
                    goto KeywordOrIdentifier;
                }
                else if (char.IsDigit(currentChar))
                {
                    goto Integer;
                }
                else if (char.IsWhiteSpace(currentChar))
                {
                    continue;
                }
                else
                {
                    switch (currentChar)
                    {
                        case '(': yield return makeToken(TokenType.LeftExpressionDelimiter); continue;
                        case ')': yield return makeToken(TokenType.RightExpressionDelimiter); continue;
                        case '{': yield return makeToken(TokenType.LeftCodeDelimiter); continue;
                        case '}': yield return makeToken(TokenType.RightCodeDelimiter); continue;
                        case '[': yield return makeToken(TokenType.LeftEnumerationDelimiter); continue;
                        case ']': yield return makeToken(TokenType.RightEnumerationDelimiter); continue;
                        case '+': yield return makeToken(TokenType.Add); continue;
                        case '-': yield return makeToken(TokenType.Subtract); continue;
                        case '*': yield return makeToken(TokenType.Multiply); continue;
                        case '/': yield return makeToken(TokenType.Divide); continue;
                        case '^': yield return makeToken(TokenType.BitwiseXor); continue;
                        case '<': yield return makeToken(TokenType.ComparisonLessThan); continue;
                        case '>': yield return makeToken(TokenType.ComparisonGreaterThan); continue;
                        case '~': yield return makeToken(TokenType.BinaryNegate); continue;
                        case '\n': yield return makeToken(TokenType.StatementDelimiter); continue;
                        case '|':
                            if (stepIfNextCharIs('|')) yield return makeToken(TokenType.Or);
                            else yield return makeToken(TokenType.BitwiseOr);
                            continue;

                        case '&':
                            if (stepIfNextCharIs('&')) yield return makeToken(TokenType.And);
                            else yield return makeToken(TokenType.BitwiseAnd);
                            continue;

                        case '!':
                            if (stepIfNextCharIs('<')) yield return makeToken(TokenType.ComparisonGreaterOrEqualThan);
                            if (stepIfNextCharIs('>')) yield return makeToken(TokenType.ComparisonLessOrEqualThan);
                            else yield return makeToken(TokenType.ComparisonNotEquals);
                            continue;

                        case ':':
                            expectChar(':', nameof(TokenType.StaticAccessor));
                            yield return makeToken(TokenType.StaticAccessor);
                            continue;

                        case '.': goto Real;
                        case '_': goto KeywordOrIdentifier;
                        case '"': goto String;
                        case '\'': goto Char;
                    }
                }

                throw new FormatException($"{currentChar} is not a recognized start of a token");

                KeywordOrIdentifier:
                while (true)
                {
                    var nextInt = readChar();
                    if (nextInt == -1)
                    {
                        var text = buffer.ToString();
                        var keyword = Keywords.FirstOrDefault(x => string.Equals(x.Key, text, StringComparison.Ordinal));

                        if (keyword.Key == null)
                        {
                            yield return makeToken(TokenType.Identifier, text);
                        }
                        else
                        {
                            yield return makeToken(keyword.Value, text);
                        }
                    }

                    var nextChar = (char)nextInt;

                    if (char.IsLetterOrDigit(nextChar) || nextChar == '_')
                    {
                        buffer.Append(nextChar);
                    }
                    else
                    {
                        var text = buffer.ToString();

                        possibleKeywords = possibleKeywords.Where(x => x.Text.StartsWith(text)).ToList();

                        if (possibleKeywords.Count == 0)
                        {
                            returnType(TokenType.Identifier);
                            return;
                        }

                        if (string.Equals(possibleKeywords[0].Text, text, StringComparison.CurrentCulture))
                        {
                            returnType(possibleKeywords[0].Keyword, text);
                            return;
                        }

                        buffer.Append(nextChar);
                    }
                }

                Integer:
                while (true)
                {
                    var nextInt = readChar();
                    if (nextInt == -1)
                    {
                        IsFinished = true;
                        returnType(TokenType.Integer);
                        return;
                    }

                    var nextChar = (char)nextInt;

                    if (char.IsDigit(nextChar))
                    {
                        buffer.Append(nextChar);
                    }
                    else if (nextChar == '.')
                    {
                        buffer.Append('.');
                        goto Real;
                    }
                    else if (char.IsWhiteSpace(nextChar))
                    {
                        returnType(TokenType.Integer);
                        return;
                    }
                }

                Real:
                while (true)
                {
                    var nextInt = readChar();
                    if (nextInt == -1)
                    {
                        IsFinished = true;
                        returnType(TokenType.Real);
                        return;
                    }

                    var nextChar = (char)nextInt;

                    if (char.IsDigit(nextChar))
                    {
                        buffer.Append(nextChar);
                    }
                    else
                    {
                        string text = buffer.ToString();
                        if (text.EndsWith("."))
                        {
                            //TODO
                        }
                        returnType(TokenType.Real, text);
                        return;
                    }
                }

                String:
                bool delimitedString = false;
                while (true)
                {
                    var nextInt = readChar();

                    var nextChar = (char)nextInt;

                    if (!delimitedString)
                    {
                        if (nextChar == '"')
                        {
                            returnType(TokenType.String, buffer.ToString(1, buffer.Length - 2));
                            return;
                        }

                        if (nextChar == '\\') delimitedString = true;
                        else buffer.Append(nextChar);
                    }
                    else
                    {
                        delimitedString = false;
                        if (nextChar == '\\') buffer.Append('\\');
                        else if (nextChar == '"') buffer.Append('"');
                        else if (nextChar == 'n') buffer.Append('\n');
                        else if (nextChar == 't') buffer.Append('\t');
                    }
                }

                Char:
                {
                    bool delimitedChar = false;
                    var nextInt = readChar();

                    var nextChar = (char)nextInt;

                    if (!delimitedChar)
                    {
                        if (nextChar == '\\') delimitedChar = true;
                        else buffer.Append(nextChar);
                    }
                    else
                    {
                        delimitedChar = false;
                        if (nextChar == '\\') buffer.Append('\\');
                        else if (nextChar == '\'') buffer.Append('\'');
                        else if (nextChar == 'n') buffer.Append('\n');
                        else if (nextChar == 't') buffer.Append('\t');
                    }

                    expectChar('\'', nameof(TokenType.Char));
                    yield return makeToken(TokenType.Char, buffer.ToString(1, 1));
                    continue;
                }
            }

            #region Internal Helpers

            int peekChar() => textReader.Peek();

            int readChar()
            {
                currentCharIndex++;
                return textReader.Read();
            }

            void expectChar(char expected, string context)
            {
                char actualChar = (char)readChar();
                if (actualChar != expected) throwExpect(context, expected.ToString(), actualChar.ToString());
            }

            bool stepIfNextCharIs(char expected)
            {
                char actualChar = (char)peekChar();
                if (actualChar != expected) return false;
                readChar();
                return true;
            }

            void throwExpect(string context, string expected, string actual)
            {
                throw new FormatException($"Error while parsing {context}, expected {expected} but got {actual}");
            }

            #endregion Internal Helpers
        }
    }
}
