namespace TempleLang.Intermediate.Expressions
{
    using TempleLang.Compiler;
    using TempleLang.Diagnostic;

    public interface IExpression : IPositioned
    {
        ITypeInfo ReturnType { get; }
    }
}
