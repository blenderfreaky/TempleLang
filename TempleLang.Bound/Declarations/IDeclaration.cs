namespace TempleLang.Bound.Declarations
{
    using TempleLang.Diagnostic;

    public interface IDeclaration : IPositioned
    {
        Positioned<string> Name { get; }
    }
}