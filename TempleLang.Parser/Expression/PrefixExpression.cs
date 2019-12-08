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

        public override string ToString() => $"{Operator} ({Value})";

        public static new readonly Parser<Expression, Token> Parser =
            CreateParser(PostfixExpression.Parser, Parse.Token(
                Token.Increment, Token.Decrement,
                Token.Not, Token.BitwiseNot,
                Token.Add, Token.Subtract));

        public static Parser<Expression, Token> CreateParser(Parser<Expression, Token> parser, Parser<Lexeme<Token>, Token> @operator) =>
            (from op in @operator
            from val in parser
            select new PrefixExpression(val, op.Token))
            .Or(parser);
    }
}
