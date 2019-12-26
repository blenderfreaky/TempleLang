namespace TempleLang.Binder
{
    using System.Collections.Generic;
    using TempleLang.Bound;
    using TempleLang.Diagnostic;
    using TempleLang.Bound.Expressions;
    using IE = TempleLang.Bound.Expressions;
    using S = TempleLang.Parser;
    using TempleLang.Bound.Primitives;

    public partial class CodeBinder : Binder
    {
        public Dictionary<string, Local> Locals { get; }

        public CodeBinder(Binder? parent = null) : base(parent)
        {
            Locals = new Dictionary<string, Local>();
        }

        public CodeBinder(Dictionary<string, Local> symbols, Binder? parent = null) : base(parent)
        {
            Locals = symbols;
        }

        public override ITypeInfo FindType(S.Expression expr) => Parent?.FindType(expr) ?? PrimitiveType.Unknown;

        public IE.IValue FindValue(S.Identifier expr)
        {
            if (Locals.TryGetValue(expr.Name, out var value))
            {
                return value;
            }

            if (Parent is CodeBinder codeBinder)
            {
                return codeBinder.FindValue(expr);
            }

            //TODO
            Error(DiagnosticCode.UnknownValue, expr.Location);

            return Local.Unknown;
        }
    }
}
