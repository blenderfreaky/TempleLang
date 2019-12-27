namespace TempleLang.Bound.Expressions
{
    using TempleLang.Bound;
    using TempleLang.Diagnostic;

    public struct UnaryExpression : IExpression
    {
        public IExpression Operand { get; }

        public UnaryOperatorType Operator { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public UnaryExpression(IExpression value, UnaryOperatorType @operator, ITypeInfo returnType, FileLocation location)
        {
            Operand = value;
            Operator = @operator;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"{Operator}({Operand}) : {ReturnType}";

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(UnaryExpression left, UnaryExpression right) => left.Equals(right);

        public static bool operator !=(UnaryExpression left, UnaryExpression right) => !(left == right);
    }

    public enum UnaryOperatorType
    {
        PreIncrement, PostIncrement,

        PreDecrement, PostDecrement,

        LogicalNot, BitwiseNot,

        ERROR,
        Negation,
        Dereference,
        Reference,
    }
}
