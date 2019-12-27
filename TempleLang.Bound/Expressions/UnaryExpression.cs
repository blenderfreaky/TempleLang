namespace TempleLang.Bound.Expressions
{
    using System.Collections.Generic;
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

        public override bool Equals(object? obj) => obj is UnaryExpression expression && EqualityComparer<IExpression>.Default.Equals(Operand, expression.Operand) && Operator == expression.Operator && EqualityComparer<ITypeInfo>.Default.Equals(ReturnType, expression.ReturnType) && EqualityComparer<FileLocation>.Default.Equals(Location, expression.Location);

        public override int GetHashCode()
        {
            var hashCode = -795401056;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IExpression>.Default.GetHashCode(Operand);
            hashCode = (hashCode * -1521134295) + Operator.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<ITypeInfo>.Default.GetHashCode(ReturnType);
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
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
