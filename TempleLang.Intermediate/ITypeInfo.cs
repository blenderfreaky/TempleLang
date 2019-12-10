namespace TempleLang.Compiler
{
    using System;
    using System.Collections;

    public interface ITypeInfo : ISymbolContainer
    {
        string Name { get; }
        string FullyQualifiedName { get; }
    }
}
