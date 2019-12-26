namespace TempleLang.Bound.Expressions
{
    using TempleLang.Diagnostic;

    public struct AccessExpression : IValue
    {
        public IExpression Accessee { get; }

        public AccessOperationType AccessOperator { get; }

        public Positioned<string> Accessor { get; }

        public ValueFlags Flags { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public AccessExpression(IExpression accessee, AccessOperationType accessOperator, Positioned<string> accessor, ValueFlags flags, ITypeInfo returnType, FileLocation location)
        {
            Accessee = accessee;
            AccessOperator = accessOperator;
            Accessor = accessor;
            Flags = flags;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"({Accessee} {AccessOperator} {Accessor})";

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(AccessExpression left, AccessExpression right) => left.Equals(right);

        public static bool operator !=(AccessExpression left, AccessExpression right) => !(left == right);
    }

    public enum AccessOperationType
    {
        Regular,

        ERROR,
    }
}
