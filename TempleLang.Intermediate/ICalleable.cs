using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Intermediate.Expressions;

namespace TempleLang.Intermediate
{
    public interface ICallable : ITypeInfo
    {
        ITypeInfo ReturnType { get; }

        IReadOnlyList<Local> Parameters { get; }
    }
}
