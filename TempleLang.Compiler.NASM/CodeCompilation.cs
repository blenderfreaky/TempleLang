namespace TempleLang.CodeGenerator.NASM
{
    using Bound.Expressions;
    using Bound.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Intermediate;

    public class CodeCompilation
    {
        public List<IInstruction> Instructions { get; }
        public Dictionary<Constant, DataLocation> ConstantTable { get; }
        public Dictionary<Variable, IMemory> AssignedLocations { get; }

        public CodeCompilation(
            List<IInstruction> instructions,
            List<Constant> constantTable,
            Dictionary<Variable, IMemory> assignedLocations)
        {
            Instructions = instructions;
            ConstantTable = constantTable.Distinct().ToDictionary(x => x, x => new DataLocation(x.DebugName.Replace(' ', '_'), x.Type.Size));
            AssignedLocations = assignedLocations;
        }

        public CodeCompilation(
            List<IInstruction> instructions,
            Dictionary<Constant, DataLocation> constantTable,
            Dictionary<Variable, IMemory> assignedLocations)
        {
            Instructions = instructions;
            ConstantTable = constantTable;
            AssignedLocations = assignedLocations;
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

        public IEnumerable<NasmInstruction> CompileInstructions() => Instructions.SelectMany(CompileInstruction);//.BasicOptimization();

        private IEnumerable<NasmInstruction> CompileInstruction(IInstruction instruction)
        {
            return instruction switch
            {
                UnaryComputationAssignment inst => CompileCore(inst),
                BinaryComputationAssignment inst => CompileCore(inst),
                ConditionalJump inst => CompileCore(inst),
                UnconditionalJump inst => CompileCore(inst),
                LabelInstruction inst => CompileCore(inst),
                _ => throw new ArgumentException(nameof(instruction))
            };
        }

        private IMemory? GetMemory(IReadableValue memory) => memory switch
        {
            Variable mem => AssignedLocations[mem],
            Constant mem => ConstantTable[mem],
            DiscardValue mem => null,
            _ => throw new ArgumentException(nameof(memory)),
        };

        private NasmInstruction Move(IMemory target, IParameter source) =>
            new NasmInstruction(
                    "mov",
                    new MemoryParameter(target),
                    source);

        private readonly OperatorTable<UnaryOperatorType> _unaryOperators = new OperatorTable<UnaryOperatorType>
        {
            [UnaryOperatorType.BitwiseNot, PrimitiveType.Long] = "not",
            [UnaryOperatorType.Negation, PrimitiveType.Long] = "neg",
            [UnaryOperatorType.Reference] = "lea",
        };

        private IEnumerable<NasmInstruction> CompileCore(UnaryComputationAssignment inst)
        {
            var operandMemory = GetMemory(inst.Operand) ?? throw new ArgumentException(nameof(inst));
            var targetMemory = GetMemory(inst.Target);

            if (targetMemory == null) yield break;

            if (inst.Operator == UnaryOperatorType.Dereference)
            {
                if (inst.OperandType != PrimitiveType.Long) throw new ArgumentException(nameof(inst));

                yield return Move(targetMemory,
                    new DereferenceParameter(new MemoryParameter(operandMemory)));
                yield break;
            }

            yield return new NasmInstruction(
                _unaryOperators[inst.Operator, inst.OperandType],
                new MemoryParameter(targetMemory),
                new MemoryParameter(operandMemory));
        }

        private readonly OperatorTable<BinaryOperatorType> _binaryOperators = new OperatorTable<BinaryOperatorType>
        {
            [BinaryOperatorType.Add, PrimitiveType.Long] = "add",
            [BinaryOperatorType.Subtract, PrimitiveType.Long] = "sub",
            [BinaryOperatorType.Multiply, PrimitiveType.Long] = "imul",
            //[BinaryOperatorType.Divide, PrimitiveType.Long] = "idiv",
            [BinaryOperatorType.Assign] = "mov",
        };

        private IEnumerable<NasmInstruction> CompileCore(BinaryComputationAssignment inst)
        {
            var lhsMemory = GetMemory(inst.Lhs) ?? throw new ArgumentException(nameof(inst));
            var rhsMemory = GetMemory(inst.Rhs) ?? throw new ArgumentException(nameof(inst));
            var targetMemory = GetMemory(inst.Target);

            if (targetMemory == null)
            {
                if (inst.Operator == BinaryOperatorType.Assign) targetMemory = lhsMemory;
                else yield break;
            }

            if (lhsMemory != targetMemory) yield return Move(targetMemory, new MemoryParameter(lhsMemory));

            yield return new NasmInstruction(
                _binaryOperators[inst.Operator, inst.OperandType],
                new MemoryParameter(targetMemory),
                new MemoryParameter(rhsMemory));
        }

        private IEnumerable<NasmInstruction> CompileCore(ConditionalJump inst)
        {
            var conditionMemory = GetMemory(inst.Condition) ?? throw new ArgumentException(nameof(inst));

            yield return new NasmInstruction(
                "test",
                new MemoryParameter(conditionMemory),
                new MemoryParameter(conditionMemory));

            yield return new NasmInstruction(
                inst.Inverted ? "jz" : "jnz",
                new LabelParameter(inst.Target.Name));
        }

        private IEnumerable<NasmInstruction> CompileCore(UnconditionalJump inst)
        {
            yield return new NasmInstruction(
                "jmp",
                new LabelParameter(inst.Target.Name));
        }

        private IEnumerable<NasmInstruction> CompileCore(LabelInstruction inst)
        {
            yield return new NasmInstruction(inst.Name);
        }
    }
}
