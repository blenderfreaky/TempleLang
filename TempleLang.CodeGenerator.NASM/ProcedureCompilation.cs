namespace TempleLang.CodeGenerator.NASM
{
    using Bound.Declarations;
    using Bound.Expressions;
    using Bound.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.CodeGenerator.NASM;
    using TempleLang.Intermediate;

    public class ProcedureCompilation
    {
        public Procedure Procedure { get; }
        public List<IInstruction> Instructions { get; }
        public Dictionary<Constant, DataLocation> ConstantTable { get; }
        public RegisterAllocation RegisterAllocation { get; }
        public int OwnedStackSpaceEnd { get; }

        public ProcedureCompilation(
            Procedure procedure,
            List<IInstruction> instructions,
            Dictionary<Constant, DataLocation> constantTable,
            RegisterAllocation registerAllocation)
        {
            Procedure = procedure;
            Instructions = instructions;
            ConstantTable = constantTable;
            RegisterAllocation = registerAllocation;
            OwnedStackSpaceEnd = (RegisterAllocation.StackOffset / 8) % 2 != 0 ? RegisterAllocation.StackOffset + 8 : RegisterAllocation.StackOffset;
        }

        public IEnumerable<NasmInstruction> CompileInstructions()
        {
            int stackSize = OwnedStackSpaceEnd + 32;

            yield return new NasmInstruction("sub", new MemoryParameter(Register.Get(RegisterName.RSP)), new LiteralParameter(stackSize.ToString()));

            foreach (var instruction in Instructions.SelectMany((x, i) => CompileInstruction(i, x))) yield return instruction;

            yield return new NasmInstruction("__exit");
            yield return new NasmInstruction("add", new MemoryParameter(Register.Get(RegisterName.RSP)), new LiteralParameter(stackSize.ToString()));
            yield return new NasmInstruction(name: "ret");
        }

        private IEnumerable<NasmInstruction> CompileInstruction(int i, IInstruction instruction)
        {
            return instruction switch
            {
                UnaryComputationAssignment inst => CompileCore(inst),
                BinaryComputationAssignment inst => CompileCore(inst),
                ConditionalJump inst => CompileCore(inst),
                UnconditionalJump inst => CompileCore(inst),
                LabelInstruction inst => CompileCore(inst),
                CallInstruction inst => CompileCore(i, inst),
                ReturnInstruction inst => CompileCore(inst),
                _ => throw new ArgumentException(nameof(instruction))
            };
        }

        private IMemory? GetMemory(IReadableValue memory) => memory switch
        {
            Variable mem => RegisterAllocation.AssignedLocation[mem],
            Constant mem => ConstantTable[mem],
            DiscardValue mem => null,
            _ => throw new ArgumentException(nameof(memory)),
        };

        private IMemory ParameterLocation(int index)
        {
            switch (index)
            {
                case 0:
                    return Register.Get(RegisterName.RCX);
                case 1:
                    return Register.Get(RegisterName.RDX);
                case 2:
                    return Register.Get(RegisterName.R8);
                case 3:
                    return Register.Get(RegisterName.R9);
                default:
                    return new StackLocation(OwnedStackSpaceEnd + index, 8);
            }
        }

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

        private IEnumerable<NasmInstruction> CompileCore(int index, CallInstruction inst)
        {
            var lives = RegisterAllocation
                .GetAllLiveAt(index)
                .Select((x,i) => (Variable: x, Temporary: new StackLocation(RegisterAllocation.StackOffset + (i * 8), 8)))
                .ToList();

            foreach (var live in lives)
            {
                yield return Move(live.Temporary, new MemoryParameter(RegisterAllocation.AssignedLocation[live.Variable]));
            }

            for (int i = 0; i < inst.Parameters.Count; i++)
            {
                yield return Move(ParameterLocation(i), new MemoryParameter(GetMemory(inst.Parameters[i]) ?? throw new ArgumentException(nameof(inst))));
            }

            yield return new NasmInstruction("call", new LiteralParameter(inst.Name));

            foreach (var live in lives)
            {
                yield return Move(RegisterAllocation.AssignedLocation[live.Variable], new MemoryParameter(live.Temporary));
            }
        }

        private IEnumerable<NasmInstruction> CompileCore(ReturnInstruction inst)
        {
            if (inst.ReturnValue != null)
            {
                yield return Move(Register.Get(RegisterName.RAX), new MemoryParameter(GetMemory(inst.ReturnValue) ?? throw new ArgumentException(nameof(inst))));
            }

            yield return new NasmInstruction("jmp", new LiteralParameter("__exit"));
        }
    }
}