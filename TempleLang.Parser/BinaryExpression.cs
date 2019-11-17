using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TempleLang.Lexer;

namespace TempleLang.Parser.Abstractions
{
    public class BinaryExpression : Expression
    {
        public readonly IReadOnlyList<Expression> ChainedExpressions;

        public readonly Token Operator;

        public BinaryExpression(IReadOnlyList<Expression> chainedExpressions, Token @operator)
        {
            ChainedExpressions = chainedExpressions;
            Operator = @operator;
        }
    }
}
