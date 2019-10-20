namespace TempleLang.Parser.Abstractions
{
    using TempleLang.Lexer.Abstractions;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a node of the Abstract Syntax tree
    /// </summary>
    /// <typeparam name="TParseTreeNodeType">The ParseTreeNodeType enum to classify the Parse Tree Node type with</typeparam>
    public interface IParseTreeNode<out TParseTreeNodeType>
    {
        /// <summary>
        /// Gets the type of this specific node
        /// </summary>
        TParseTreeNodeType NodeType { get; }

        /// <summary>
        /// Gets a bool specifying whether the node is terminal
        /// </summary>
        /// <remarks>If true, enumeration of Children should yield no results, and NodeType may have invalid values</remarks>
        bool IsTerminal { get; }

        /// <summary>
        /// Gets all Parse Tree Nodes that have this node as their Parent
        /// </summary>
        IEnumerable<IParseTreeNode<TParseTreeNodeType>> Children { get; }
    }
}