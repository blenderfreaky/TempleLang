using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Intermediate.Expressions;

namespace TempleLang.Intermediate.Statements
{
    public struct ReturnStatement
    {
        public IExpression Expression { get; }
    }
}
