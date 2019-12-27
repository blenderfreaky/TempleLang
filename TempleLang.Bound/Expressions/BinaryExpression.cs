namespace TempleLang.Bound.Expressions
{
    using System.Collections.Generic;
    using TempleLang.Bound;
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

        public override bool Equals(object? obj) => obj is BinaryExpression expression && EqualityComparer<IExpression>.Default.Equals(Lhs, expression.Lhs) && EqualityComparer<IExpression>.Default.Equals(Rhs, expression.Rhs) && Operator == expression.Operator && EqualityComparer<ITypeInfo>.Default.Equals(ReturnType, expression.ReturnType) && EqualityComparer<FileLocation>.Default.Equals(Location, expression.Location);

        public override int GetHashCode()
        {
            var hashCode = -1311190100;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IExpression>.Default.GetHashCode(Lhs);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IExpression>.Default.GetHashCode(Rhs);
            hashCode = (hashCode * -1521134295) + Operator.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<ITypeInfo>.Default.GetHashCode(ReturnType);
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
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
