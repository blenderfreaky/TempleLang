namespace TempleLang.Binder
{
    using System;
    using TempleLang.Diagnostic;
    using TempleLang.Bound.Expressions;
    using TempleLang.Bound.Primitives;
    using TempleLang.Parser;
    using IE = TempleLang.Bound.Expressions;
    using S = TempleLang.Parser;

    public partial class CodeBinder : Binder
    {
        public IE.IValue BindLiteral(S.Literal syntaxLiteral) => syntaxLiteral switch
        {
            S.BoolLiteral expr => BindLiteral(expr),
            S.NumberLiteral expr => BindLiteral(expr),
            S.StringLiteral expr => BindLiteral(expr),
            _ => throw new ArgumentException(nameof(syntaxLiteral)),
        };

        public IE.IValue BindLiteral(S.BoolLiteral expr) => new Constant<bool>(expr.Value, PrimitiveType.Bool, expr.Location);

        public IE.IValue BindLiteral(S.NumberLiteral expr)
        {
            if (expr.Value.Contains(".") || (expr.Flags & (NumberFlags.SuffixDouble)) != 0)
            {
                if (!double.TryParse(expr.Value, out var result))
                {
                    Error(DiagnosticCode.InvalidNumeric, expr.Location);
                }

                return new Constant<double>(result, PrimitiveType.Double, expr.Location);
            }
            else
            {
                if (!long.TryParse(expr.Value, out var result))
                {
                    Error(DiagnosticCode.InvalidNumeric, expr.Location);
                }

                return new Constant<long>(result, PrimitiveType.Long, expr.Location);
            }
        }

        public IE.IValue BindLiteral(S.StringLiteral expr) => new Constant<string>(expr.Value, PrimitiveType.String, expr.Location);
    }
}
