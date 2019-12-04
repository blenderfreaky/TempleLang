namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class PostfixExpression : Expression
    {
        public Expression Value { get; }

        public Token Operator { get; }

        public PostfixExpression(Expression value, Token @operator)
        {
            Value = value;
            Operator = @operator;
        }

        public static Parser<PostfixExpression, Token> CreateParser(Parser<Expression, Token> parser, Token @operator) =>
            from val in parser
            from op in Parse.Token(@operator)
            select new PostfixExpression(val, op.Token);
    }
}
