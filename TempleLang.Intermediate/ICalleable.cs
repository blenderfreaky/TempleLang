using System.Collections.Generic;
using TempleLang.Intermediate.Expressions;

namespace TempleLang.Intermediate
{
    public interface ICallable : ITypeInfo
    {
        ITypeInfo ReturnType { get; }

        IReadOnlyList<Local> Parameters { get; }
    }
}
