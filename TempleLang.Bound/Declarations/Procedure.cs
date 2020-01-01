namespace TempleLang.Bound.Declarations
{
    using Bound.Statements;
    using System.Collections.Generic;
    using TempleLang.Bound.Expressions;
    using TempleLang.Bound.Primitives;
    using TempleLang.Diagnostic;

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

        int ITypeInfo.Size => 8;

        public Procedure(Positioned<string> name, ITypeInfo returnType, IReadOnlyList<Local> parameters, IStatement entryPoint, FileLocation location)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            EntryPoint = entryPoint;
            Location = location;
        }

        public override string ToString() => $"func {Signature} {EntryPoint}";
        public string Signature => $"{Name.Value}({string.Join(", ", Parameters)}) : {ReturnType?.ToString() ?? "void"}";

        public bool TryGetMember(string name, out IMemberInfo? member)
        {
            member = null;
            return false;
        }

        public CallExpression BindOverload(IExpression callee, IReadOnlyList<IExpression> parameters, FileLocation location, IDiagnosticReceiver diagnosticReceiver)
        {
            if (Parameters.Count != parameters.Count)
            {
                diagnosticReceiver.ReceiveDiagnostic(DiagnosticCode.InvalidParamCount, location, true);
                return new CallExpression(null!, null!, PrimitiveType.Unknown, location);
            }

            for (int i = 0; i < parameters.Count; i++)
            {
                if (Parameters[i].ReturnType == parameters[i].ReturnType) continue;

                diagnosticReceiver.ReceiveDiagnostic(DiagnosticCode.InvalidParamType, location, true);
            }

            return new CallExpression(callee, parameters, ReturnType, location);
        }
    }
}