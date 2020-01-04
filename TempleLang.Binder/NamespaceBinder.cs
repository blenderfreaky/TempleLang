namespace TempleLang.Binder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Bound.Declarations;
    using TempleLang.Bound.Expressions;
    using TempleLang.Bound.Primitives;
    using TempleLang.Bound.Statements;
    using TempleLang.Diagnostic;
    using S = Parser;

    public sealed class NamespaceBinder : Binder
    {
        public S.NamespaceDeclaration Declaration { get; }
        public List<NamespaceBinder> Namespaces { get; }
        public Dictionary<S.Declaration, IDeclaration> Declarations { get; }

        public NamespaceBinder(S.NamespaceDeclaration declaration)
        {
            Declaration = declaration;
            Declarations = new Dictionary<S.Declaration, IDeclaration>();
            Namespaces = new List<NamespaceBinder>();
        }

        public void Explore()
        {
            foreach (var declaration in Declaration.Declarations)
            {
                switch (declaration)
                {
                    case S.NamespaceDeclaration decl:
                        var binder = new NamespaceBinder(decl);
                        binder.Explore();
                        Namespaces.Add(binder);
                        break;

                    case S.ProcedureDeclaration decl:
                        Declarations[decl] = ExploreDeclaration(decl);
                        break;

                    default:
                        throw new InvalidOperationException("Invalid declaration");
                }
            }
        }

        private IDeclaration ExploreDeclaration(S.ProcedureDeclaration decl)
        {
            var returnType = FindType(decl.ReturnTypeAnnotation);
            var parameters = decl.Parameters.Select(x => new Local(x.Name.Name, ValueFlags.Readable | ValueFlags.Assignable, FindType(x.TypeAnnotation), x.Location)).ToList();

            return decl.ImportedName == null
                ? (IDeclaration)new Procedure(decl.Name.PositionedText, returnType, parameters, null!, decl.Location)
                : new ProcedureImport(decl.Name.PositionedText, returnType, parameters, decl.ImportedName.Value, decl.Location);
        }

        public IDeclaration Bind()
        {
            var declarations = Declarations.Select(x =>
                {
                    switch (x.Value)
                    {
                        case Procedure proc:
                            var syntaxDecl = (S.ProcedureDeclaration)x.Key;

                            IStatement? boundStatement;
                            using (CodeBinder codeBinder = new CodeBinder(proc.Parameters.ToDictionary(x => x.Name, x => x), this))
                            {
                                boundStatement = codeBinder.BindStatement(syntaxDecl.EntryPoint!);
                            }

                            if (boundStatement == null)
                            {
                                //Error()
                            }

                            return new Procedure(proc.Name, proc.ReturnType, proc.Parameters, boundStatement!, proc.Location);

                        default:
                            return x.Value;
                    }
                })
                .Concat(Namespaces.Select(x => x.Bind()))
                .ToList();

            return new NamespaceDeclaration(Declaration.Name, declarations, Declaration.Location);
        }

        public override IDeclaration? FindDeclaration(S.Expression expression)
        {
            switch (expression)
            {
                case S.AccessExpression expr:
                    return FindDeclarationCore(expr);

                case S.Identifier expr:
                    return FindDeclarationCore(expr);

                default:
                    Error(DiagnosticCode.InvalidTypeSpecifier, expression.Location);
                    return null;
            }
        }

        public IDeclaration? FindDeclarationCore(S.AccessExpression expr)
        {
            var lhs = FindDeclaration(expr.Accessee);

            //TODO
            return lhs;
        }

        public IDeclaration? FindDeclarationCore(S.Identifier expr)
        {
            var result = Declarations.Values.FirstOrDefault(x => x.Name == expr.Name);

            if (result != null)
            {
                return result;
            }

            if (Parent != null)
            {
                return Parent.FindDeclaration(expr);
            }

            if (PrimitiveType.Types.TryGetValue(expr.Name, out var type)) return type;

            //TODO
            Error(DiagnosticCode.UnknownType, expr.Location);

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            CollectDiagnostics();
            base.Dispose(disposing);
        }

        public void CollectDiagnostics()
        {
            foreach (var @namespace in Namespaces) @namespace.Dispose();
        }
    }
}