namespace TempleLang.Intermediate.Expressions
{
    using TempleLang.Intermediate;
    using TempleLang.Diagnostic;

    public struct TernaryExpression : IExpression
    {
        public IExpression Condition { get; }
        public IExpression TrueValue { get; }
        public IExpression FalseValue { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public TernaryExpression(IExpression condition, IExpression trueValue, IExpression falseValue, ITypeInfo returnType, FileLocation location)
        {
            Condition = condition;
            TrueValue = trueValue;
            FalseValue = falseValue;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"({Condition} ? {TrueValue} : {FalseValue}) : {ReturnType}";
    }
}
