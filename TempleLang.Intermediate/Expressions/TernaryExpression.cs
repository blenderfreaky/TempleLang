using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Compiler;
using TempleLang.Diagnostic;

namespace TempleLang.Intermediate.Expressions
{
    public struct TernaryExpression : IExpression
    {
        public IExpression Condition { get; }
        public IExpression TrueValue { get; }
        public IExpression FalseValue { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public TernaryExpression(IExpression condition, IExpression trueValue, IExpression falseValue, ITypeInfo returnType, FileLocation location)
        {
            Condition = condition;
            TrueValue = trueValue;
            FalseValue = falseValue;
            ReturnType = returnType;
            Location = location;
        }
    }
}
