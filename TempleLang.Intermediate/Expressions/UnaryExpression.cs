namespace TempleLang.Intermediate.Expressions
{
    using TempleLang.Intermediate;
    using TempleLang.Diagnostic;

    public struct UnaryExpression : IExpression
    {
        public IExpression Value { get; }

        public UnaryOperatorType Operator { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public UnaryExpression(IExpression value, UnaryOperatorType @operator, ITypeInfo returnType, FileLocation location)
        {
            Value = value;
            Operator = @operator;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"{Operator}({Value}) : {ReturnType}";
    }

    public enum UnaryOperatorType
    {
        PreIncrement, PostIncrement,

        PreDecrement, PostDecrement,

        LogicalNot, BitwiseNot,

        ERROR,
    }
}
