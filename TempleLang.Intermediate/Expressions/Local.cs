namespace TempleLang.Intermediate.Expressions
{
    using TempleLang.Intermediate;
    using TempleLang.Diagnostic;
    using Primitives;

    public struct Local : IValue
    {
        public string Name { get; }
        public ValueFlags Flags { get; }

        public ITypeInfo ReturnType { get; }
        public FileLocation Location { get; }

        public Local(string name, ValueFlags flags, ITypeInfo returnType, FileLocation location)
        {
            Name = name;
            Flags = flags;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"{Name} : {ReturnType}";

        public static readonly Local Unknown = new Local("?", ValueFlags.None, PrimitiveType.Unknown, FileLocation.Null);

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(Local left, Local right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Local left, Local right)
        {
            return !(left == right);
        }
    }
}