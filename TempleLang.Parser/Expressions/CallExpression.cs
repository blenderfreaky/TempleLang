namespace TempleLang.Parser
{
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class CallExpression : Expression
    {
        public Expression Callee { get; }

        public List<Expression> Parameters { get; }

        public CallExpression(Expression callee, List<Expression> parameters) : base(callee, parameters.Last())
        {
            Callee = callee;
            Parameters = parameters;
        }

        public override string ToString() => $"({Callee}({string.Join(", ", Parameters)}))";

        public static readonly Parser<List<Expression>, Token> ParameterListParser =
            from l in Parse.Token(Token.LeftExpressionDelimiter)
            from parameters in Parse.Ref(() => Expression.Parser).SeparatedBy(Parse.Token(Token.Comma))
            from r in Parse.Token(Token.RightExpressionDelimiter)
            select parameters;

        public static new readonly Parser<Expression, Token> Parser =
            from callee in BinaryExpression.Assignment
            from parameters in ParameterListParser
            select new CallExpression(callee, parameters);
    }
}
