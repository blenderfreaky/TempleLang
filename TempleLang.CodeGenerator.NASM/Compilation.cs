namespace TempleLang.CodeGenerator.NASM
{
    using Bound.Expressions;
    using Bound.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Intermediate;

    public class Compilation
    {
        public Dictionary<Constant, DataLocation> ConstantTable { get; }

        public Compilation(List<Constant> constantTable)
        {
            ConstantTable = constantTable.Distinct().ToDictionary(x => x, x => new DataLocation(x.DebugName.Replace(' ', '_'), x.Type.Size));
        }

        public IEnumerable<NasmInstruction> CompileConstantTable()
        {
            foreach (var constant in ConstantTable)
            {
                var isString = constant.Key.Type == PrimitiveType.String;

                yield return new NasmInstruction(
                    label: constant.Value.LabelName,
                    name: isString ? "db" : "equ",
                    new LiteralParameter(isString ? $"__utf16__({constant.Key.ValueText})" : constant.Key.ValueString));
            }
        }
    }
}