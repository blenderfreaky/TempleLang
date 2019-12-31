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
        public int StackRegisterTemporary { get; }
        public int StackHomeSpace { get; }
        public int StackEnd { get; }

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
            StackRegisterTemporary = Align(RegisterAllocation.StackOffset);

            int maxCallDeposit = Instructions
                .Select((x, i) => (x, i))
                // Get all CallInstructions with their indicies
                .Where(x => x.x is CallInstruction)
                // Check how many variables are live at the instruction and get the max
                .Max(x => RegisterAllocation.GetAllLiveAt(x.i).Count());

            StackHomeSpace = Align(StackRegisterTemporary + maxCallDeposit);

            int maxParameters = Instructions
                .Where(x => x is CallInstruction)
                .Max(x => ((CallInstruction)x).Parameters.Count);

            StackEnd = StackHomeSpace + 32 + ((maxParameters - 4) * 8);
        }

        private int Align(int val) => (val / 8) % 2 == 0 ? val : val + 8;

        public IEnumerable<NasmInstruction> CompileInstructions()
        {
            int stackSize = StackEnd;

            yield return new NasmInstruction("sub", new MemoryParameter(Register.Get(RegisterName.RSP)), new LiteralParameter(stackSize.ToString()));

            foreach (var instruction in Instructions.SelectMany((x, i) => CompileInstruction(i, x))) yield return instruction;

            yield return new NasmInstruction(".__exit");
            yield return new NasmInstruction("add", new MemoryParameter(Register.Get(RegisterName.RSP)), new LiteralParameter(stackSize.ToString()));
            yield return new NasmInstruction(name: "ret");
        }

        private IEnumerable<NasmInstruction> CompileInstruction(int i, IInstruction instruction) => instruction switch
        {
            UnaryComputationAssignment inst => CompileCore(inst),
            BinaryComputationAssignment inst => CompileCore(inst),
            ConditionalJump inst => CompileCore(inst),
            UnconditionalJump inst => CompileCore(inst),
            LabelInstruction inst => CompileCore(inst),
            CallInstruction inst => CompileCore(i, inst),
            ReturnInstruction inst => CompileCore(inst),
            ParameterQueryAssignment inst => CompileCore(inst),
            _ => throw new ArgumentException(nameof(instruction))
        };

        private IMemory? GetMemory(IReadableValue memory) => memory switch
        {
            Variable mem => RegisterAllocation.AssignedLocation[mem],
            Constant mem => ConstantTable[mem],
            DiscardValue mem => null,
            _ => throw new ArgumentException(nameof(memory)),
        };

        private IMemory ParameterLocation(int index)
        {
            return index switch
            {
                0 => Register.Get(RegisterName.RCX),
                1 => Register.Get(RegisterName.RDX),
                2 => Register.Get(RegisterName.R8),
                3 => Register.Get(RegisterName.R9),
                _ => new StackLocation(StackEnd - ((index - 4) * 8), 8),
            };
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

            if (inst.Operator == UnaryOperatorType.Reference)
            {
                if (inst.OperandType != PrimitiveType.Long) throw new ArgumentException(nameof(inst));

                yield return new NasmInstruction("lea",
                    new MemoryParameter(targetMemory),
                    new DereferenceParameter(new MemoryParameter(operandMemory)));
                yield break;
            }

            yield return Move(targetMemory, new MemoryParameter(operandMemory));

            yield return new NasmInstruction(
                _unaryOperators[inst.Operator, inst.OperandType],
                new MemoryParameter(targetMemory));
        }

        private readonly OperatorTable<BinaryOperatorType> _binaryOperators = new OperatorTable<BinaryOperatorType>
        {
            [BinaryOperatorType.Add, PrimitiveType.Long] = "add",
            [BinaryOperatorType.Subtract, PrimitiveType.Long] = "sub",
            [BinaryOperatorType.Multiply, PrimitiveType.Long] = "imul",
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
                .Select((x, i) => (Variable: x, Temporary: new StackLocation(RegisterAllocation.StackOffset + 8 + (i * 8), 8)))
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

            if (inst.Target != null && !(inst.Target is DiscardValue))
            {
                yield return Move(GetMemory(inst.Target) ?? throw new ArgumentException(nameof(inst)), new MemoryParameter(Register.Get(RegisterName.RAX)));
            }

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

            yield return new NasmInstruction("jmp", new LabelParameter(".__exit"));
        }

        private IEnumerable<NasmInstruction> CompileCore(ParameterQueryAssignment inst)
        {
            if (inst.Target != null && !(inst.Target is DiscardValue))
            {
                yield return Move(GetMemory(inst.Target) ?? throw new ArgumentException(nameof(inst)), new MemoryParameter(ParameterLocation(inst.ParameterIndex)));
            }
        }
    }
}