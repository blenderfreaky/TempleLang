using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempleLang.Lexer.Abstractions;

namespace TempleLang.Parser.Abstractions
{
    /// <summary>
    /// Represents a terminal node of the Abstract Syntax tree
    /// </summary>
    /// <typeparam name="TAstNode">The type of the parent (and child) AST Nodes, should generally match the type itself</typeparam>
    /// <typeparam name="TAstNodeType">The AstNodeType enum to classify the AST Node type with</typeparam>
    /// <typeparam name="TToken">The <see cref="IToken{TTokenType}"/> implementation to use for tokens</typeparam>
    /// <typeparam name="TTokenType">The <see cref="TTokenType"/> implementation to use for the Tokens</typeparam>
    public interface IAstNodeTerminal<out TAstNode, out TAstNodeType, out TToken, out TTokenType> : IAstNode<TAstNode, TAstNodeType>
        where TAstNode : class, IAstNode<TAstNode, TAstNodeType>
        where TToken : IToken<TTokenType>
    {
        /// <summary>
        /// Returns the Token the terminal represents
        /// </summary>
        TToken Token { get; }
    }
}
