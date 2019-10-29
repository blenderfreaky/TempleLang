﻿namespace TempleLang.Lexer.Abstractions
{
    using System.IO;

    /// <summary>
    /// Represents a lexer, able to lex the input from a <see cref="TextReader"/>
    /// </summary>
    /// <typeparam name="TLexeme">The Type of the lexeme the lexer returns. Must implement <see cref="ILexeme{TToken, TSourceFile}"/></typeparam>
    /// <typeparam name="TToken">The Token Type used by the Lexeme</typeparam>
    public interface ILexer<out TLexeme, out TToken, in TSourceFile>
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile
    {
        /// <summary>
        /// Lexes the text far enough to generate one lexeme
        /// </summary>
        /// <returns>The topmost token from the text</returns>
        TLexeme LexOne();
    }
}
