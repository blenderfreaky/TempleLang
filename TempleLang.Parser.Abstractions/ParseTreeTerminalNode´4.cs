namespace TempleLang.Parser.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer.Abstractions;

    public struct ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> : IParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile>
        where TToken : IToken<TTokenType, TSourceFile>        where TSourceFile : ISourceFile
    {
        public TToken Token { get; }

        public TParseTreeNodeType NodeType => default!;
        public bool IsTerminal => true;
        public IEnumerable<IParseTreeNode<TParseTreeNodeType>> Children => Enumerable.Empty<IParseTreeNode<TParseTreeNodeType>>();

        public ParseTreeTerminalNode(TToken token) => Token = token;

        public override string ToString() => $"Terminal: [{Token}]";
    }
}
