using System.Collections.Generic;
using System.Linq;
using TempleLang.Lexer;
using TempleLang.Parser.Abstractions;

namespace TempleLang.Parser
{
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

        public static new readonly Parser<CallExpression, Token> Parser =
            from callee in Parse.Ref(() => Expression.Parser)
            from l in Parse.Token(Token.LeftExpressionDelimiter)
            from parameters in Parse.Ref(() => Expression.Parser).SeparatedBy(Parse.Token(Token.Comma))
            from r in Parse.Token(Token.RightExpressionDelimiter)
            select new CallExpression(callee, parameters);
    }
}
