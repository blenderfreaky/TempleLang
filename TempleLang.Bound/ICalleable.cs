namespace TempleLang.Bound
{
    using System.Collections.Generic;
    using TempleLang.Bound.Expressions;

    public interface ICallable : ITypeInfo
    {
        ITypeInfo ReturnType { get; }

        IReadOnlyList<Local> Parameters { get; }
    }
}
