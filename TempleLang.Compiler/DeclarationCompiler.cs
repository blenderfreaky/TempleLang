namespace TempleLang.Compiler
{
    using Diagnostic;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Binder;
    using TempleLang.Bound.Declarations;
    using TempleLang.CodeGenerator.NASM;
    using TempleLang.Intermediate;
    using S = Parser;

    public class DeclarationCompiler
    {
        private Transformer Transformer { get; }
        public Dictionary<Constant, DataLocation> ConstantTable { get; }
        public List<string> Externs { get; }

        public DeclarationCompiler()
        {
            Transformer = new Transformer();
            ConstantTable = new Dictionary<Constant, DataLocation>();
            Externs = new List<string>();
        }

        public List<ProcedureCompilation> Compile(S.NamespaceDeclaration declaration, out IEnumerable<DiagnosticInfo> diagnostics)
        {
            var binder = new NamespaceBinder(declaration);
            binder.Explore();
            var bound = binder.Bind();
            binder.CollectDiagnostics();
            diagnostics = binder.Diagnostics;

            var procedures = CompileDeclaration(bound).ToList();

            foreach (var constant in Transformer.ConstantTable)
            {
                ConstantTable[constant] = new DataLocation(constant.DebugName.Replace(' ', '_'), constant.Type.Size);
            }

            return procedures.ToList();
        }

        private IEnumerable<ProcedureCompilation> CompileDeclaration(IDeclaration declaration)
        {
            switch (declaration)
            {
                case NamespaceDeclaration decl:
                    foreach (var compilation in decl.Declarations.SelectMany(CompileDeclaration)) yield return compilation;
                    break;
                case Procedure procedure:
                    var transformed = Transformer.TransformStatement(procedure.EntryPoint).ToList();

                    var allocation = RegisterAllocation.Generate(transformed);

                    yield return new ProcedureCompilation(procedure, transformed, ConstantTable, allocation);
                    break;
                case ProcedureImport import:
                    Externs.Add(import.ImportedName);
                    break;
            }
        }
    }
}
