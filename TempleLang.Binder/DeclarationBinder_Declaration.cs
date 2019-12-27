﻿namespace TempleLang.Binder
{
    using System;
    using System.Linq;
    using TempleLang.Bound;
    using TempleLang.Bound.Declarations;
    using TempleLang.Bound.Expressions;
    using TempleLang.Bound.Primitives;
    using TempleLang.Diagnostic;
    using S = TempleLang.Parser;

    public partial class DeclarationBinder
    {
        public IDeclaration BindDeclaration(S.Declaration syntaxDeclaration) => syntaxDeclaration switch
        {
            S.ProcedureDeclaration decl => BindDeclaration(decl),
            _ => throw new ArgumentException(nameof(syntaxDeclaration)),
        };

        public IDeclaration BindDeclaration(S.ProcedureDeclaration decl)
        {
            var returnTypeAnnotation = decl.ReturnTypeAnnotation == null ? null : FindType(decl.ReturnTypeAnnotation);

            ITypeInfo returnType = returnTypeAnnotation!;

            var parameters = decl.Parameters.Select(BindParameter).ToDictionary(x => x.Name, x => x);

            using CodeBinder binder = new CodeBinder(parameters, this);

            var entry = binder.BindStatement(decl.EntryPoint);

            var boundParams = decl.Parameters.Select(x => binder.Locals[x.Name.Name]).ToList();

            if (entry == null)
            {
                Error(DiagnosticCode.EmptyProcedure, decl.Location);
            }

            var procedure = new Procedure(decl.Name.PositionedText, returnType, boundParams, entry!, decl.Location);

            Procedures[procedure.Name] = procedure;

            return procedure;
        }

        public Local BindParameter(S.TypeAnnotatedName parameter)
        {
            return new Local(
                parameter.Name.Name,
                ValueFlags.Readable | ValueFlags.Assignable,
                parameter.TypeAnnotation == null ? PrimitiveType.Unknown : FindType(parameter.TypeAnnotation),
                parameter.Location);
        }
    }
}