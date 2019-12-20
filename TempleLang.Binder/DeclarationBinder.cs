using System.Collections.Generic;
using TempleLang.Intermediate;
using TempleLang.Diagnostic;
using TempleLang.Parser;
using S = TempleLang.Parser;
using TempleLang.Intermediate.Declarations;
using TempleLang.Intermediate.Primitives;

namespace TempleLang.Binder
{
    public partial class DeclarationBinder : Binder
    {
        public Dictionary<string, ITypeInfo> Types { get; }
        public Dictionary<string, Procedure> Procedures { get; }

        public DeclarationBinder(Dictionary<string, ITypeInfo> types, Binder? parent  = null) : base(parent)
        {
            Types = types;
            Procedures = new Dictionary<string, Procedure>();
        }

        public DeclarationBinder(Binder? parent) : base(parent)
        {
            Types = new Dictionary<string, ITypeInfo>();
            Procedures = new Dictionary<string, Procedure>();
        }

        public override ITypeInfo FindType(Expression expression)
        {
            switch (expression)
            {
                case S.AccessExpression expr:
                    return FindType(expr);
                case S.Identifier expr:
                    return FindType(expr);
                default:
                    Error(DiagnosticCode.InvalidTypeSpecifier, expression.Location);
                    return PrimitiveType.Unknown;
            }
        }

        public ITypeInfo FindType(S.AccessExpression expr)
        {
            var lhs = FindType(expr.Accessee);

            //TODO
            return lhs;
        }

        public ITypeInfo FindType(S.Identifier expr)
        {
            if (Types.TryGetValue(expr.Name, out var value))
            {
                return value;
            }

            if (Parent != null)
            {
                return Parent.FindType(expr);
            }

            //TODO
            Error(DiagnosticCode.UnknownType, expr.Location);

            return PrimitiveType.Unknown;
        }
    }
}
