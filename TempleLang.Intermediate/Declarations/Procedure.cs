namespace TempleLang.Intermediate.Declarations
{
    using Intermediate.Statements;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;

    public sealed class Procedure : IDeclaration, ICallable
    {
        public Positioned<string> Name { get; }

        public ITypeInfo ReturnType { get; }

        public IReadOnlyList<Local> Parameters { get; }

        public IStatement EntryPoint { get; }

        public FileLocation Location { get; }

        string ITypeInfo.Name => Name.Value;

        //TODO
        string ITypeInfo.FullyQualifiedName => Name.Value;

        public int StackSize => Parameters.Sum(x => x.ReturnType.Size) + (EntryPoint is BlockStatement block ? block.StackSize : 0);

        public int Size => 8;

        public Procedure(Positioned<string> name, ITypeInfo returnType, IReadOnlyList<Local> parameters, IStatement entryPoint, FileLocation location)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            EntryPoint = entryPoint;
            Location = location;
        }

        public override string ToString() => $"let {Name.Value}({string.Join(", ", Parameters)}) : {ReturnType?.ToString() ?? "void"} {EntryPoint}";

        public bool TryGetMember(string name, out IMemberInfo? member)
        {
            member = null;
            return false;
        }
    }
}
