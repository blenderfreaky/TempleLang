namespace TempleLang.Compiler
{
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Bound.Primitives;
    using TempleLang.CodeGenerator.NASM;
    using TempleLang.Intermediate;

    public class Compilation
    {
        public List<ProcedureCompilation> ProcedureCompilations { get; }
        public List<string> Externs { get; }
        public Dictionary<Constant, DataLocation> ConstantTable { get; }

        public Compilation(List<ProcedureCompilation> procedureCompilations, List<string> externs, Dictionary<Constant, DataLocation> constantTable)
        {
            ProcedureCompilations = procedureCompilations;
            Externs = externs;
            ConstantTable = constantTable;
        }

        public IEnumerable<NasmInstruction> WriteExterns()
        {
            return Externs.Select(x => new NasmInstruction("extern", new LiteralParameter(x)));
        }

        public IEnumerable<NasmRegion> WriteProcedures()
        {
            return ProcedureCompilations.Select(x => new NasmRegion(x.Procedure.Name, x.CompileInstructions()));
        }

        public IEnumerable<NasmInstruction> WriteConstantTable()
        {
            foreach (var constant in ConstantTable)
            {
                var isString = constant.Key.Type == PrimitiveType.String;

                yield return new NasmInstruction(
                    label: constant.Value.LabelName,
                    name: isString ? "db" : "equ",
                    new LiteralParameter(isString ? $"__utf16__(\"{constant.Key.ValueText}\")" : constant.Key.ValueString));
            }
        }
    }
}
