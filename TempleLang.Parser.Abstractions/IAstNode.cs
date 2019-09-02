namespace TempleLang.Parser.Abstractions
{
    using TempleLang.Lexer.Abstractions;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a node of the Abstract Syntax tree
    /// </summary>
    /// <typeparam name="TTokenType">The IDogeTokenType implementation to use for the Tokens</typeparam>
    public interface IAstNode<out TToken, out TAstNode, out TTokenType, out TAstNodeType>
        where TToken : IToken<TTokenType>
        where TAstNode : class, IAstNode<TToken, TAstNode, TTokenType, TAstNodeType>
    {
        /// <summary>
        /// The type of this specific node
        /// </summary>
        TAstNodeType NodeType { get; }

        /// <summary>
        /// The Tokens the AST Node contains
        /// </summary>
        IEnumerable<TToken> Tokens { get; }

        /// <summary>
        /// The AST Node this is a child to. Null if it is a root node
        /// </summary>
        TAstNode? Parent { get; }

        /// <summary>
        /// All AST Nodes that have this node as their Parent
        /// </summary>
        IEnumerable<TAstNode> Children { get; }
    }
}