﻿namespace TempleLang.Binder
{
    using System;
    using System.Linq;
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;
    using TempleLang.Intermediate.Primitives;
    using TempleLang.Intermediate.Statements;
    using TempleLang.Parser;
    using IE = TempleLang.Intermediate.Expressions;
    using IS = TempleLang.Intermediate.Statements;
    using S = TempleLang.Parser;

    public partial class CodeBinder : Binder
    {
        public IStatement? BindStatement(Statement syntaxStatement) => syntaxStatement switch
        {
            S.ExpressionStatement stmt => BindStatement(stmt),
            S.LocalDeclarationStatement stmt => BindStatement(stmt),
            S.BlockStatement stmt => BindStatement(stmt),
            _ => throw new ArgumentException(nameof(syntaxStatement)),
        };

        public IS.ExpressionStatement? BindStatement(S.ExpressionStatement stmt) => new IS.ExpressionStatement(BindExpression(stmt.Expression), stmt.Location);

        public IS.ExpressionStatement? BindStatement(S.LocalDeclarationStatement stmt)
        {
            var assignedValue = stmt.Assignment == null ? null : BindExpression(stmt.Assignment);

            var assignedType = assignedValue?.ReturnType;
            var annotatedType = stmt.Name.TypeAnnotation == null ? null : FindType(stmt.Name.TypeAnnotation);
            var returnType = assignedType ?? annotatedType;

            if (assignedType == null && annotatedType == null)
            {
                Error(DiagnosticCode.TypeInferenceFailed, stmt.Location);
            }

            if (assignedType != null && annotatedType != null && assignedType != annotatedType)
            {
                Error(DiagnosticCode.InvalidType, stmt.Location);
            }

            if (returnType == null)
            {
                Error(DiagnosticCode.MissingType, stmt.Location);
                returnType = PrimitiveType.Long;
            }

            var local = new Local(stmt.Name.Name.Name, ValueFlags.Assignable | ValueFlags.Readable, returnType, stmt.Location);

            Locals[local.Name] = local;

            if (assignedValue == null) return null;

            return new IS.ExpressionStatement(new IE.BinaryExpression(local, assignedValue, BinaryOperatorType.Assign, returnType, stmt.Location), stmt.Location);
        }

        public IS.BlockStatement? BindStatement(S.BlockStatement stmt)
        {
            using CodeBinder binder = new CodeBinder(this);

            var statements = stmt.Statements.Select(binder.BindStatement).Where(x => x != null).Select(x => x!).ToList();
            var result = new IS.BlockStatement(binder.Locals.Values, statements, stmt.Location);

            return result;
        }
    }
}