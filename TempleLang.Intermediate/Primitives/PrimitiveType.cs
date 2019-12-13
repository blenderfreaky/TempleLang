namespace TempleLang.Intermediate.Primitives
{
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Intermediate;

    public sealed class PrimitiveType : ITypeInfo
    {
        public static PrimitiveType Double { get; } = new PrimitiveType("double", "double");
        public static PrimitiveType Long { get; } = new PrimitiveType("long", "long");
        public static PrimitiveType Bool { get; } = new PrimitiveType("bool", "bool");
        public static PrimitiveType String { get; } = new PrimitiveType("string", "string");
        public static PrimitiveType Unknown { get; } = new PrimitiveType("?", "?");

        public string Name { get; }

        public string FullyQualifiedName { get; }

        public int Size => 8;

        private PrimitiveType(string name, string fullyQualifiedName)
        {
            Name = name;
            FullyQualifiedName = fullyQualifiedName;
        }

        public override string ToString() => $"{FullyQualifiedName}";
        public bool TryGetMember(string name, out IMemberInfo member) => throw new System.NotImplementedException();

        public static readonly Dictionary<string, ITypeInfo> Types = new[]
        {
            Double,
            Long,
            Bool,
            String,
        }.ToDictionary(x => x.Name, x => (ITypeInfo)x);
    }
}
