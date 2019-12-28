using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempleLang.Binder;
using TempleLang.Bound;
using TempleLang.Bound.Declarations;
using TempleLang.Bound.Primitives;
using TempleLang.CodeGenerator.NASM;
using TempleLang.Intermediate;
using TempleLang.Parser;

namespace TempleLang.Compiler
{
    public class DeclarationCompiler
    {
        public DeclarationBinder DeclarationBinder { get; }
        public Transformer Transformer { get; }

        public DeclarationCompiler()
        {
            DeclarationBinder = new DeclarationBinder(PrimitiveType.Types);
            Transformer = new Transformer();
        }

        public List<ProcedureCompilation> Compile(List<Declaration> declarations)
        {
            _ = declarations.Select(DeclarationBinder.BindDeclarationHead).ToList();
            var boundDeclarations = declarations.Select(DeclarationBinder.BindDeclarationBody).ToList();
            var transformed = boundDeclarations.Select(x => x as Procedure).Where(x => x != null)
                .ToDictionary(x => x!, x => Transformer.TransformStatement(x!.EntryPoint).ToList());

            var constantTable = Transformer.ConstantTable.ToDictionary(x => x, x => new DataLocation(x.DebugName.Replace(" ", "_"), x.Type.Size));

            var procedures = transformed.Select(procedure =>
            {
                var allocation = RegisterAllocation.Generate(procedure.Value);

                Console.WriteLine(new string('-', 50));
                Console.WriteLine(string.Join("\n", allocation.AssignedLocations.Select(x => x.Key + " -> " + x.Value)));
                Console.WriteLine();
                Console.WriteLine(string.Join("\n", procedure.Value));
                Console.WriteLine();

                return new ProcedureCompilation(procedure.Key, procedure.Value, constantTable, allocation.AssignedLocations);
            }).ToList();

            return procedures;
        }
    }
}
