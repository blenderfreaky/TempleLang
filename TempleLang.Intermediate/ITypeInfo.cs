namespace TempleLang.Compiler
{
    public interface ITypeInfo : ISymbolContainer
    {
        string Name { get; }
        string FullyQualifiedName { get; }
    }
}
