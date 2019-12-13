
using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Diagnostic;

namespace TempleLang.Intermediate.Expressions
{
    public struct CallExpression : IExpression
    {
        public IExpression Callee { get; }

        public IReadOnlyList<IExpression> Parameters { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public CallExpression(IExpression callee, IReadOnlyList<IExpression> parameters, ITypeInfo returnType, FileLocation location)
        {
            Callee = callee;
            Parameters = parameters;
            ReturnType = returnType;
            Location = location;
        }
    }
}
