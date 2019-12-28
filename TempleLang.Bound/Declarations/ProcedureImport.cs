namespace TempleLang.Bound.Declarations
{
    using Bound.Statements;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Bound.Expressions;
    using TempleLang.Bound.Primitives;
    using TempleLang.Diagnostic;
    using IE = TempleLang.Bound.Expressions;

    public sealed class ProcedureImport : IDeclaration, ICallable
    {
        public Positioned<string> Name { get; }

        public ITypeInfo ReturnType { get; }

        public IReadOnlyList<Local> Parameters { get; }

        public Positioned<string> ImportedName;

        public FileLocation Location { get; }

        string ITypeInfo.Name => Name.Value;

        //TODO
        string ITypeInfo.FullyQualifiedName => Name.Value;

        int ITypeInfo.Size => 8;

        public ProcedureImport(Positioned<string> name, ITypeInfo returnType, IReadOnlyList<Local> parameters, Positioned<string> importedName, FileLocation location)
        {
            Name = name;
            ReturnType = returnType;
            Parameters = parameters;
            ImportedName = importedName;
            Location = location;
        }

        public override string ToString() =>
            $"let {Name.Value}({string.Join(", ", Parameters)}) : {ReturnType?.ToString() ?? "void"} {"using \"" + ImportedName.Value + "\""}";

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
                return new IE.CallExpression(null!, null!, PrimitiveType.Unknown, location);
            }

            for (int i = 0; i < parameters.Count; i++)
            {
                if (Parameters[i].ReturnType == parameters[i].ReturnType) continue;

                diagnosticReceiver.ReceiveDiagnostic(DiagnosticCode.InvalidParamType, location, true);
            }

            return new IE.CallExpression(callee, parameters, ReturnType, location);
        }
    }
}