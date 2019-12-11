using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Diagnostic;
using TempleLang.Intermediate.Expressions;

namespace TempleLang.Intermediate.Statements
{
    public struct ExpressionStatement : IStatement
    {
        public IExpression Expression { get; }

        public FileLocation Location { get; }

        public ExpressionStatement(IExpression expression, FileLocation location)
        {
            Expression = expression;
            Location = location;
        }
    }
}
