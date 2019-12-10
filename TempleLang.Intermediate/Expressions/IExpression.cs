using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Compiler;
using TempleLang.Diagnostic;

namespace TempleLang.Intermediate.Expressions
{
    public interface IExpression : IPositioned
    {
        ITypeInfo ReturnType { get; }
    }
}
