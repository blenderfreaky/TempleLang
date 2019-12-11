namespace TempleLang.Compiler.Procedures
{
    using Intermediate.Statements;
    using System.Collections.Generic;

    public sealed class Procedure
    {
        public ITypeInfo ReturnType { get; }

        public IReadOnlyList<ParameterInfo> Parameters { get; }

        public IStatement EntryPoint { get; }

        public Procedure(ITypeInfo returnType, IReadOnlyList<ParameterInfo> parameters, IStatement entryPoint)
        {
            ReturnType = returnType;
            Parameters = parameters;
            EntryPoint = entryPoint;
        }
    }

}
