﻿namespace TempleLang.Lexer.Abstractions
{
    /// <summary>
    /// Represents a stream of tokens generated by lexing a file, but providing only read-only actions
    /// </summary>
    /// <typeparam name="TTokenType">The TokenType implementation to use for the tokens</typeparam>
    public interface IReadonlyTokenStream<out TTokenType>
    {
        /// <summary>
        /// Gets the next token from the stream, without altering the streams state
        /// </summary>
        /// <returns>The next token from the stream</returns>
        IToken<TTokenType> Peek();

        /// <summary>
        /// Represents whether the token stream has finished. If true getting more tokens will throw an exception
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Represents the amount of tokens that have been read so far
        /// </summary>
        int ReadTokenCount { get; }
    }
}