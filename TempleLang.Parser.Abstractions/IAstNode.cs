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
        where TAstNode : IAstNode<TAstNode, TAstNodeType>
    {
        /// <summary>
        /// Gets the type of this specific node
        /// </summary>
        TAstNodeType NodeType { get; }

        /// <summary>
        /// Gets a bool specifying whether the node has a parent node
        /// </summary>
        bool HasParent { get; }

        /// <summary>
        /// Gets the AST Node this is a child to
        /// </summary>
        /// <remarks>May throw of HasParent is false</remarks>
        TAstNode Parent { get; }

        /// <summary>
        /// Gets all AST Nodes that have this node as their Parent
        /// </summary>
        IEnumerable<TAstNode> Children { get; }
    }
}