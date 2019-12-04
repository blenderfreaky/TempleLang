namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class TernaryExpression : Expression
    {
        public Expression Condition { get; }

        public Expression TrueValue { get; }
        public Expression FalseValue { get; }

        public TernaryExpression(Expression condition, Expression trueValue, Expression falseValue)
        {
            Condition = condition;
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public static Parser<TernaryExpression, Token> CreateParser(Parser<Expression, Token> conditionParser, Parser<Expression, Token> valueParser) =>
            from condition in conditionParser
            from _ in Parse.Token(Token.TernaryTruePrefix)
            from trueValue in valueParser
            from __ in Parse.Token(Token.TernaryFalsePrefix)
            from falseValue in valueParser
            select new TernaryExpression(condition, trueValue, falseValue);
    }
}
