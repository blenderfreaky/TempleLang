namespace TempleLang.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public abstract class Statement
    {
        public static readonly Parser<Statement, Token> Parser =
            ExpressionStatement.Parser.OfType<Statement, Token>()
            .Or(LocalDeclarationStatement.Parser);
    }
}
