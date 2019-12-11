namespace TempleLang.Parser
{
    using TempleLang.Diagnostic;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public abstract class Statement : Syntax
    {
        protected Statement(IPositioned location) : base(location) { }

        protected Statement(IPositioned first, IPositioned second) : base(first, second) { }

        protected Statement(params IPositioned[] locations) : base(locations) { }

        public static readonly Parser<Statement, Token> Parser =
            LocalDeclarationStatement.Parser.OfType<Statement, Token>()
            .Or(ExpressionStatement.Parser)
            .Or(BlockStatement.Parser);
    }
}
