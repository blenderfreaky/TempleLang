namespace TempleLang.Intermediate.Primitives
{
    using TempleLang.Compiler;

    public sealed class PrimitiveType : ITypeInfo
    {
        public static PrimitiveType Double { get; } = new PrimitiveType("double", "double");
        public static PrimitiveType Long { get; } = new PrimitiveType("long", "long");
        public static PrimitiveType Bool { get; } = new PrimitiveType("bool", "bool");
        public static PrimitiveType String { get; } = new PrimitiveType("string", "string");

        public string Name { get; }

        public string FullyQualifiedName { get; }

        private PrimitiveType(string name, string fullyQualifiedName)
        {
            Name = name;
            FullyQualifiedName = fullyQualifiedName;
        }

        public override string ToString() => $"{FullyQualifiedName}";
    }
}
