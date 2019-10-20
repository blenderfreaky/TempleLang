namespace TempleLang.Parser.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TempleLang.Lexer.Abstractions;

    /// <summary>
    /// Represents a terminal node of the Abstract Syntax tree
    /// </summary>
    /// <typeparam name="TParseTreeNodeType">The type of the parent (and child) Parse Tree Nodes, should generally match the type itself</typeparam>
    /// <typeparam name="TParseTreeNodeType">The ParseTreeNodeType enum to classify the Parse Tree Node type with</typeparam>
    /// <typeparam name="TToken">The <see cref="IToken{TTokenType}"/> implementation to use for tokens</typeparam>
    /// <typeparam name="TTokenType">The <see cref="TTokenType"/> implementation to use for the Tokens</typeparam>
    public interface IParseTreeTerminalNode<out TParseTreeNodeType, out TToken, out TTokenType, out TSourceFile> : IParseTreeNode<TParseTreeNodeType>
        where TToken : IToken<TTokenType, TSourceFile>
        where TSourceFile : ISourceFile
    {
        /// <summary>
        /// Returns the Token the terminal represents
        /// </summary>
        TToken Token { get; }
    }
}
