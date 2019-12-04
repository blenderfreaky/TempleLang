using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempleLang.Lexer;
using TempleLang.Parser.Abstractions;

namespace TempleLang.Parser
{

    public class BinaryExpression : Expression
    {
        public Expression Lhs { get; }
        public Expression Rhs { get; }

        public Token Operator { get; }

        public BinaryExpression(Expression lhs, Expression rhs, Token @operator)
        {
            Lhs = lhs;
            Rhs = rhs;
            Operator = @operator;
        }

        public static Parser<BinaryExpression, Token> CreateParser(Parser<Expression, Token> parser, Token @operator) =>
            from lhs in parser
            from op in Parse.Token(@operator)
            from rhs in parser
            select new BinaryExpression(lhs, rhs, op.Token);
    }
}
