namespace TempleLang.Binder
{
    using System;
    using System.Linq;
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;
    using TempleLang.Intermediate.Primitives;
    using TempleLang.Intermediate.Statements;
    using TempleLang.Parser;
    using IS = TempleLang.Intermediate.Statements;
    using S = TempleLang.Parser;

    public partial class Binder
    {
        public IStatement BindStatement(Statement syntaxStatement) => syntaxStatement switch
        {
            S.ExpressionStatement stmt => BindStatement(stmt),
            S.LocalDeclarationStatement stmt => BindStatement(stmt),
            S.BlockStatement stmt => BindStatement(stmt),
            _ => throw new ArgumentException(nameof(syntaxStatement)),
        };

        public IS.ExpressionStatement BindStatement(S.ExpressionStatement stmt) => new IS.ExpressionStatement(BindExpression(stmt.Expression), stmt.Location);

        public IS.LocalDeclarationStatement BindStatement(S.LocalDeclarationStatement stmt)
        {
            var assignedValue = stmt.Assignment == null ? null : BindExpression(stmt.Assignment);

            var returnType = assignedValue?.ReturnType;

            if (returnType == null)
            {
                Error(DiagnosticCode.MissingType, stmt.Location);
                returnType = PrimitiveType.Long;
            }

            var local = new Local(stmt.Name, ValueFlags.Assignable | ValueFlags.Readable, returnType, stmt.Location);

            Symbols[local.Name] = local;

            return new IS.LocalDeclarationStatement(local, assignedValue, stmt.Location);
        }

        public IS.BlockStatement BindStatement(S.BlockStatement stmt)
        {
            Binder binder = new Binder(this);
            var result = new IS.BlockStatement(stmt.Statements.Select(BindStatement).ToList(), stmt.Location);
            foreach (var diagnostic in binder.Diagnostics) Diagnostics.Add(diagnostic);
            return result;
        }
    }
}
