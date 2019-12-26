namespace TempleLang.Intermediate
{
    using System.Collections.Generic;
    using TempleLang.Intermediate.Expressions;

    public interface ICallable : ITypeInfo
    {
        ITypeInfo ReturnType { get; }

        IReadOnlyList<Local> Parameters { get; }
    }
}
