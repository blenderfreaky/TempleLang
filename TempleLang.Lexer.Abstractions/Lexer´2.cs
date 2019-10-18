using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TempleLang.Lexer.Abstractions
{
    public struct Lexer<TTokenType, TSourceFile> : ILexer<Token<TTokenType, TSourceFile>, TTokenType, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public LexerState<TTokenType> StartingState { get; }

        public Lexer(LexerState<TTokenType> startingState) => StartingState = startingState;

        public IEnumerable<Token<TTokenType, TSourceFile>> Tokenize(TextReader textReader, TSourceFile sourceFile)
        {
            LexerState<TTokenType> currentState = StartingState;
            StringBuilder buffer = new StringBuilder();
            int tokenIndex = 0, tokenStartCharIndex = 0, currentCharIndex = 0;

            while (true)
            {
                int character = textReader.Read();

                LexerStep<TTokenType> step = currentState.Step(character);

                if (character == -1)
                {
                    if (!step.ShouldPushToken) throw new InvalidOperationException("Unexpected EoF while lexing " + currentState.Name);

                    yield return new Token<TTokenType, TSourceFile>(
                        buffer.ToString(),
                        step.TokenType,
                        sourceFile,
                        tokenIndex,
                        tokenStartCharIndex,
                        currentCharIndex);

                    yield break;
                }

                buffer.Append((char)character);

                if (step.ShouldPushToken)
                {
                    yield return new Token<TTokenType, TSourceFile>(
                        buffer.ToString(),
                        step.TokenType,
                        sourceFile,
                        tokenIndex,
                        tokenStartCharIndex,
                        currentCharIndex);

                    tokenIndex++;
                    tokenStartCharIndex = currentCharIndex + 1;
                }

                currentState = step.NextState;

                currentCharIndex++;
            }
        }
    }
}
