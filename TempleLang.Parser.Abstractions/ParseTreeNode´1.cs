namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer.Abstractions;

    public class ParseTreeNode<TParseTreeNodeType> : IParseTreeNode<TParseTreeNodeType>
    {
        public TParseTreeNodeType NodeType { get; }

        public bool IsTerminal => false;

        public IEnumerable<IParseTreeNode<TParseTreeNodeType>> Children { get; }

        public ParseTreeNode(TParseTreeNodeType nodeType, IEnumerable<ParseTreeNode<TParseTreeNodeType>> children)
        {
            NodeType = nodeType;
            Children = children;
        }

        public ParseTreeNode(TParseTreeNodeType nodeType, params ParseTreeNode<TParseTreeNodeType>[] children)
        {
            NodeType = nodeType;
            Children = children;
        }

        public override string ToString() => $"{NodeType}: ({string.Join(", ", Children)})";
    }
}
