namespace TempleLang.Binder
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
        public IDeclaration BindDeclarationHead(S.Declaration syntaxDeclaration) => syntaxDeclaration switch
        {
            S.ProcedureDeclaration decl => BindDeclarationHead(decl),
            _ => throw new ArgumentException(nameof(syntaxDeclaration)),
        };
        public IDeclaration BindDeclarationBody(S.Declaration syntaxDeclaration) => syntaxDeclaration switch
        {
            S.ProcedureDeclaration decl => BindDeclarationBody(decl),
            _ => throw new ArgumentException(nameof(syntaxDeclaration)),
        };

        public IDeclaration BindDeclarationHead(S.ProcedureDeclaration decl)
        {
            var returnTypeAnnotation = decl.ReturnTypeAnnotation == null ? null : FindType(decl.ReturnTypeAnnotation);

            ITypeInfo returnType = returnTypeAnnotation!;

            var parameters = decl.Parameters.Select(BindParameter).ToDictionary(x => x.Name, x => x);

            if (decl.EntryPoint == null)
            {
                var procedure = new ProcedureImport(decl.Name.PositionedText, returnType, parameters.Select(x => x.Value).ToList(), decl.ImportedName!.Value, decl.Location);

                Procedures[procedure.Name] = procedure;

                return procedure;
            }
            else
            {
                var procedure = new Procedure(decl.Name.PositionedText, returnType, parameters.Select(x => x.Value).ToList(), null!, decl.Location);

                Procedures[procedure.Name] = procedure;

                return procedure;
            }
        }

        public IDeclaration BindDeclarationBody(S.ProcedureDeclaration decl)
        {
            var proc = Procedures[decl.Name.Name];

            if (decl.EntryPoint != null)
            {
                var boundParameters = decl.Parameters.Select(BindParameter).ToDictionary(x => x.Name, x => x);
                var binder = new CodeBinder(boundParameters, this);

                var entry = binder.BindStatement(decl.EntryPoint);

                if (entry == null)
                {
                    Error(DiagnosticCode.EmptyProcedure, decl.Location);
                }

                var boundParams = decl.Parameters.Select(x => binder.Locals[x.Name.Name]).ToList();

                var procedure = new Procedure(decl.Name.PositionedText, proc.ReturnType, boundParams, entry!, decl.Location);

                Procedures[procedure.Name] = procedure;

                return procedure;
            }

            return (Procedure)proc;
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