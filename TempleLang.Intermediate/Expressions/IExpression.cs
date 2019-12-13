namespace TempleLang.Intermediate.Expressions
{
    using TempleLang.Intermediate;
    using TempleLang.Diagnostic;

    public interface IExpression : IPositioned
    {
        ITypeInfo ReturnType { get; }
    }
}
