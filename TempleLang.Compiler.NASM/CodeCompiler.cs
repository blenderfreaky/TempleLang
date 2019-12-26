namespace TempleLang.Compiler.NASM
{
    using Abstractions;
    using Intermediate.Expressions;
    using Intermediate.Primitives;
    using Intermediate.Statements;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Intermediate.Declarations;

    public class CodeCompiler
    {
        public CodeCompiler? Parent { get; }

        // GP = General Purpose
        private List<Register> FreeGeneralPurposeRegisters { get; }
        private List<NasmInstruction> CompiledInstructions { get; }
        private Dictionary<Local, Register> AssignedRegisters { get; }
        private Dictionary<Local, (int Start, int Last)> LiveIntervals { get; }

        public IEnumerable<NasmInstruction> Compile()
        {

        }

        private void CollectUsages(IInstruction instruction, int startIndex)
        {
            switch (instruction)
            {
                case 
            }
        }

        private IEnumerable<NasmInstruction> Compile(IInstruction instruction)
        {
            return instruction switch
            {
                UnaryComputationAssignment inst => CompileCore(inst),
                _ => throw new ArgumentException(nameof(instruction))
            };
        }

        private Dictionary<(UnaryOperatorType, PrimitiveType), string> UnaryOperators = new Dictionary<(UnaryOperatorType, PrimitiveType), string>
        {
            [(UnaryOperatorType.BitwiseNot, PrimitiveType.Long)] = "not",
            [(UnaryOperatorType.Negation, PrimitiveType.Long)] = "neg",
        };

        private IParameter GetMemory()

        private IEnumerable<NasmInstruction> CompileCore(UnaryComputationAssignment inst)
        {
            return new NasmInstruction(
                UnaryOperators[(inst.Operator, inst.OperandType)],

        }
    }
}
