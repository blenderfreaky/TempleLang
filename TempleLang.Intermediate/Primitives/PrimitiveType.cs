using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Compiler;

namespace TempleLang.Intermediate.Primitives
{
    public sealed class PrimitiveType : ITypeInfo
    {
        public static PrimitiveType Double { get; } = new PrimitiveType("double", "double");
        public static PrimitiveType Long { get; } = new PrimitiveType("long", "long");
        public static PrimitiveType Bool { get; } = new PrimitiveType("bool", "bool");

        public string Name { get; }

        public string FullyQualifiedName { get; }

        public PrimitiveType(string name, string fullyQualifiedName)
        {
            Name = name;
            FullyQualifiedName = fullyQualifiedName;
        }

        public override string ToString() => $"{FullyQualifiedName}";
    }
}
