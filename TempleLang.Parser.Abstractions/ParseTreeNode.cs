namespace TempleLang.Parser.Abstractions
{
    using Exceptions;
    using Lexer.Abstractions.Exceptions;
    using System;
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    public static class ParseTreeNode
    {
        public static ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile>
            AsTerminal<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(this TToken token)
            where TToken : IToken<TTokenType, TSourceFile>
            where TSourceFile : ISourceFile
            => new ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(token);

        public static ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile>
            MatchTerminal<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(this IEnumerator<TToken> token, TTokenType tokenType, TParseTreeNodeType context)
            where TToken : IToken<TTokenType, TSourceFile>
            where TSourceFile : ISourceFile
        {
            if (EqualityComparer<TTokenType>.Default.Equals(token.Current.Type, tokenType))
            {
                return new ParseTreeTerminalNode<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(token.Current);
            }

            // TODO: Find a way to get type interference here
            throw UnexpectedTokenException.Create<TParseTreeNodeType, TToken, TTokenType, TSourceFile>(token.Current, context, tokenType);
        }
    }
}
