namespace TempleLang.Intermediate.Expressions
{
    using TempleLang.Intermediate;
    using TempleLang.Diagnostic;

    public struct BinaryExpression : IExpression
    {
        public IExpression Lhs { get; }
        public IExpression Rhs { get; }

        public BinaryOperatorType Operator { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public BinaryExpression(IExpression lhs, IExpression rhs, BinaryOperatorType @operator, ITypeInfo returnType, FileLocation location)
        {
            Lhs = lhs;
            Rhs = rhs;
            Operator = @operator;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"({Lhs} {Operator} {Rhs}) : {ReturnType}";

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(BinaryExpression left, BinaryExpression right) => left.Equals(right);

        public static bool operator !=(BinaryExpression left, BinaryExpression right) => !(left == right);
    }

    public enum BinaryOperatorType
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Remainder,
        LogicalOr,
        BitwiseOr,
        LogicalAnd,
        BitwiseAnd,
        BitwiseXor,
        BitshiftLeft,
        BitshiftRight,

        Assign,

        ComparisonGreaterThan,
        ComparisonGreaterThanOrEqual,
        ComparisonLessThan,
        ComparisonLessThanOrEqual,
        ComparisonEqual,
        ComparisonNotEqual,
        ERROR,
    }
}
