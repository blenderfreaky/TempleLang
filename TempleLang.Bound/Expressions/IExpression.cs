namespace TempleLang.Bound.Expressions
{
    using TempleLang.Bound;
    using TempleLang.Diagnostic;

    public interface IExpression : IPositioned
    {
        ITypeInfo ReturnType { get; }
    }
}