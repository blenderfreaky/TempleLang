namespace TempleLang.Parser.Abstractions
{
    using TempleLang.Lexer.Abstractions;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a node of the Abstract Syntax tree
    /// </summary>
    /// <typeparam name="TAstNode">The type of the parent and child AST Nodes, should generally match the type itself</typeparam>
    /// <typeparam name="TAstNodeType">The AstNodeType enum to classify the AST Node type with</typeparam>
    public interface IAstNode<out TAstNode, out TAstNodeType>
        where TAstNode : class, IAstNode<TAstNode, TAstNodeType>
    {
        /// <summary>
        /// Returns the type of this specific node
        /// </summary>
        TAstNodeType NodeType { get; }

        /// <summary>
        /// Returns the AST Node this is a child to. Null if it is a root node
        /// </summary>
        TAstNode? Parent { get; }

        /// <summary>
        /// Returns all AST Nodes that have this node as their Parent
        /// </summary>
        IEnumerable<TAstNode> Children { get; }
    }
}