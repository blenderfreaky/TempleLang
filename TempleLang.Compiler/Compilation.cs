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
            yield return NasmInstruction.Call("global", new LiteralParameter("_start"));
            foreach (var externName in Externs) yield return NasmInstruction.Call("extern", new LiteralParameter(externName));
        }

        public IEnumerable<NasmRegion> WriteProcedures()
        {
            return ProcedureCompilations.Select(x => new NasmRegion(x.Procedure.Signature, x.Procedure.Name, x.CompileInstructions()));
        }

        public IEnumerable<NasmInstruction> WriteConstantTable()
        {
            foreach (var constant in ConstantTable)
            {
                var isString = constant.Key.Type == PrimitiveType.StringPointer;

                yield return NasmInstruction.LabeledCall(
                    label: constant.Value.LabelName,
                    name: isString ? "db" : "equ",
                    new LiteralParameter(isString ? $"__utf16__(`{constant.Key.ValueText}`)" : constant.Key.ValueText));
            }
        }
    }
}