namespace TempleLang.Parser.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer.Abstractions;

    public struct ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> :
        IParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile>,
        IEquatable<ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile>>,
        IEquatable<IParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile>>
        where TToken : IToken<TTokenType, TSourceFile>        where TSourceFile : ISourceFile
    {
        public TToken Token { get; }

        public TParseTreeNodeType NodeType => default!;
        public bool IsTerminal => true;
        public IEnumerable<IParseTreeNode<TParseTreeNodeType>> Children => Enumerable.Empty<IParseTreeNode<TParseTreeNodeType>>();

        public ParseTreeTerminalNode(TToken token) => Token = token;

        public override string ToString() => $"Terminal: [{Token}]";

        public bool Equals(ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> other) =>
            Token.Equals(other.Token);

        public bool Equals(IParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> other) =>
            Token.Equals(other.Token);

        public override bool Equals(object obj) =>
            obj is IParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> other
            && Equals(other);

        public override int GetHashCode() => Token.GetHashCode();

        public static bool operator ==(ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> left, ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> right) =>
            left.Equals(right);

        public static bool operator !=(ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> left, ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile> right) =>
            !(left == right);
    }
}
