namespace TempleLang.Binder
{
    using TempleLang.Compiler;
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;

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
    }
}