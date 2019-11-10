﻿namespace TempleLang.Lexer.Abstractions
{
    /// <summary>
    /// Represents a string and its associated token type
    /// </summary>
    /// <typeparam name="TToken">The Token enum to classify the token type with</typeparam>
    /// <typeparam name="TSourceFile">The implementation of <see cref="ISourceFile"/> to use</typeparam>
    public interface ILexeme<out TToken, out TSourceFile>

        where TSourceFile : ISourceFile
    {
        /// <summary>
        /// The Text this Lexeme contains
        /// </summary>
        string Text { get; }

        /// <summary>
        /// This tokens type
        /// </summary>
        TToken Token { get; }

        /// <summary>
        /// The File the token was generated from
        /// </summary>
        TSourceFile SourceFile { get; }

        /// <summary>
        /// The index of the token in the token sequence of its file
        /// </summary>
        int LexemeIndex { get; }

        /// <summary>
        /// The tokens first characters index in the file
        /// </summary>
        int FirstCharIndex { get; }

        /// <summary>
        /// The tokens last characters index in the file
        /// </summary>
        int LastCharIndex { get; }
    }
}