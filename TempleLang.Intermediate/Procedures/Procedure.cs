namespace TempleLang.Compiler.Procedures
{
    using System.Collections.Generic;

    public sealed class Procedure
    {
        public ITypeInfo ReturnType { get; }

        public IReadOnlyList<ParameterInfo> Parameters { get; }
    }

}
