namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public abstract class Expression
    {
        public static readonly Parser<Expression, Token> Atomic =
            Literal.Parser
            .Or<Expression, Token>(Identifier.Parser)
            .Or(from l in Parse.Token(Token.LeftExpressionDelimiter)
                from expr in Parse.Ref(() => Parser)
                from r in Parse.Token(Token.RightExpressionDelimiter)
                select expr);

        public static readonly Parser<Expression, Token> Parser = BinaryExpression.Assignment;
    }
}
