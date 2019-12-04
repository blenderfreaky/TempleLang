namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class PrefixExpression : Expression
    {
        public Expression Value { get; }

        public Token Operator { get; }

        public PrefixExpression(Expression value, Token @operator)
        {
            Value = value;
            Operator = @operator;
        }

        public static Parser<PrefixExpression, Token> CreateParser(Parser<Expression, Token> parser, Token @operator) =>
            from op in Parse.Token(@operator)
            from val in parser
            select new PrefixExpression(val, op.Token);
    }
}
