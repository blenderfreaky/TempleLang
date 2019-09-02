namespace TempleLang.Parser.Abstractions
{
    using TempleLang.Lexer.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IAstParser<out TToken, out TAstNode, out TTerminalAstNode, in TTokenType, in TAstNodeType>
        where TToken : IToken<TTokenType>
        where TAstNode : class, IAstNode<TToken, TAstNode, TTokenType, TAstNodeType>
        where TTerminalAstNode : TAstNode
    {
        TAstNode Parse(TAstNodeType nodeType);

        TTerminalAstNode ParseTerminal(TTokenType tokenType);
    }
}
