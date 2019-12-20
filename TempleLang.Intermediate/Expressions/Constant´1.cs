namespace TempleLang.Intermediate.Expressions
{
    using TempleLang.Intermediate;
    using TempleLang.Diagnostic;

    public struct Constant<T> : IValue
    {
        public T Value { get; }

        public ValueFlags Flags => ValueFlags.Readable | ValueFlags.Constant;

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public Constant(T value, ITypeInfo returnType, FileLocation location)
        {
            Value = value;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"Const {Value} : {ReturnType}";

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(Constant<T> left, Constant<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Constant<T> left, Constant<T> right)
        {
            return !(left == right);
        }
    }
}
