using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TempleLang.CodeGenerator.NASM
{
    public static class BasicOptimizer
    {
        public static IEnumerable<NasmInstruction> BasicOptimization(this IEnumerable<NasmInstruction> compiledInstructions)
        {
            var instructions = compiledInstructions.ToList();

            for (int i = 0; i < instructions.Count; i++)
            {
                var inst = instructions[i];

                if (inst.Name == "mov")
                {
                    var fSource = inst.Parameters![1];
                    var fTarget = inst.Parameters[0];

                    if (EqualityComparer<IParameter>.Default.Equals(fSource, fTarget))
                    {
                        instructions.RemoveAt(i);
                        i = 0;
                        continue;
                    }

                    if (i < instructions.Count - 1)
                    {
                        var sInst = instructions[i + 1];

                        if (sInst.Name == "mov")
                        {
                            var sSource = sInst.Parameters![1];
                            var sTarget = sInst.Parameters[0];

                            if (EqualityComparer<IParameter>.Default.Equals(fTarget, sSource))
                            {
                                instructions.RemoveAt(i + 1);
                                instructions[i] = new NasmInstruction("mov", sTarget, fSource);
                                i = 0;
                                continue;
                            }

                            if (EqualityComparer<IParameter>.Default.Equals(sTarget, fTarget))
                            {
                                instructions.RemoveAt(i);
                                i = 0;
                                continue;
                            }
                        }
                    }
                }

                yield return inst;
            }
        }
    }
}
