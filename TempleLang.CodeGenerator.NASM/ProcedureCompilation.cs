namespace TempleLang.CodeGenerator.NASM
{
    using Bound.Declarations;
    using Bound.Expressions;
    using Bound.Primitives;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Intermediate;

    public class ProcedureCompilation
    {
        public Procedure Procedure { get; }
        public List<IInstruction> Instructions { get; }
        public Dictionary<Constant, DataLocation> ConstantTable { get; }
        public DataLocation FalseConstant { get; }
        public DataLocation TrueConstant { get; }
        public RegisterAllocation RegisterAllocation { get; }
        public int StackRegisterTemporary { get; }
        public int StackHomeSpace { get; }
        public int StackEnd { get; }

        public ProcedureCompilation(
            Procedure procedure,
            List<IInstruction> instructions,
            Dictionary<Constant, DataLocation> constantTable,
            DataLocation falseConstant,
            DataLocation trueConstant,
            RegisterAllocation registerAllocation)
        {
            Procedure = procedure;
            Instructions = instructions;
            ConstantTable = constantTable;
            FalseConstant = falseConstant;
            TrueConstant = trueConstant;
            RegisterAllocation = registerAllocation;
            StackRegisterTemporary = Align(RegisterAllocation.StackOffset);

            var calls = Instructions
                .Select((x, i) => (x, i))
                // Get all CallInstructions with their indicies
                .Where(x => x.x is CallInstruction)
                .Select(x => (Call: (CallInstruction)x.x, Index: x.i))
                .ToList();

            int maxCallDeposit = calls.Count > 0
                ? // Check how many variables are live at the instruction and get the max
                  calls.Max(x => RegisterAllocation.GetAllLiveAt(x.Index).Count())
                : 0;

            StackHomeSpace = Align(StackRegisterTemporary + (maxCallDeposit * 8));

            int maxParameters = calls.Count > 0
                ? calls.Max(x => x.Call.Parameters.Count)
                : 0;

            StackEnd = StackHomeSpace + 32 + ((maxParameters - 4) * 8);
        }

        private int Align(int val) => (val / 8) % 2 == 0 ? val : val + 8;

        public IEnumerable<NasmInstruction> CompileInstructions()
        {
            int stackSize = StackEnd;

            yield return NasmInstruction.Call("sub", Memory(Register.Get(RegisterName.RSP)), new LiteralParameter(stackSize.ToString())).WithComment("Allocate stack");
            yield return NasmInstruction.Empty();

            foreach (var instruction in Instructions.SelectMany((x, i) => CompileInstruction(i, x))) yield return instruction;

            yield return NasmInstruction.Label(".__exit").WithComment("Function exit/return label");
            yield return NasmInstruction.Call("add", Memory(Register.Get(RegisterName.RSP)), new LiteralParameter(stackSize.ToString())).WithComment("Return stack");
            yield return NasmInstruction.Call(name: "ret");
            yield return NasmInstruction.Empty();
        }

        private IEnumerable<NasmInstruction> CompileInstruction(int i, IInstruction instruction)
        {
            yield return NasmInstruction.Comment(instruction.ToString());
            foreach (var inst in instruction switch
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
            })
            {
                yield return inst;
            }
            yield return NasmInstruction.Comment("/");
            yield return NasmInstruction.Empty();
        }

        private IMemory? TryGetMemory(IReadableValue memory) => memory switch
        {
            Variable mem => RegisterAllocation.AssignedLocation[mem],
            Constant mem => ConstantTable[mem],
            DiscardValue mem => null,
            _ => throw new ArgumentException(nameof(memory)),
        };

        private IMemory GetMemory(IReadableValue memory) => TryGetMemory(memory) ?? throw new InvalidOperationException($"Unmapped memory value {memory}");

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

        private static NasmInstruction Move(IMemory target, IParameter source) =>
            NasmInstruction.Call("mov", Memory(target), source);

        private static NasmInstruction Move(IMemory target, IMemory source) =>
            NasmInstruction.Call("mov", Memory(target), Memory(source));

        private static NasmInstruction Jump(string label) =>
            NasmInstruction.Call("jmp", new LabelParameter(label));

        private static MemoryParameter Memory(IMemory memory) => new MemoryParameter(memory);

        private static MemoryParameter Memory(RegisterName registerName) => new MemoryParameter(Register.Get(registerName));

        private MemoryParameter Memory(IReadableValue memory) => new MemoryParameter(GetMemory(memory));

        private int _counter = 0;

        private string RequestName() => "CG" + _counter++;

        private readonly OperatorTable<UnaryOperatorType> _unaryOperators = new OperatorTable<UnaryOperatorType>
        {
            [UnaryOperatorType.BitwiseNot, PrimitiveType.Long] = "not",
            [UnaryOperatorType.Negation, PrimitiveType.Long] = "neg",
            [UnaryOperatorType.PreIncrement, PrimitiveType.Long] = "inc",
            [UnaryOperatorType.PostIncrement, PrimitiveType.Long] = "inc",
            [UnaryOperatorType.PreDecrement, PrimitiveType.Long] = "dec",
            [UnaryOperatorType.PostDecrement, PrimitiveType.Long] = "dec",
        };

        private IEnumerable<NasmInstruction> CompileCore(UnaryComputationAssignment inst)
        {
            var operandMemory = GetMemory(inst.Operand);
            var targetMemory = GetMemory(inst.Target);

            switch (inst.Operator)
            {
                case UnaryOperatorType.Dereference:
                    if (inst.OperandType != PrimitiveType.Long) throw new InvalidOperationException("Dereferncing operand of invalid type");

                    yield return Move(targetMemory, new DereferenceParameter(Memory(operandMemory))).WithComment($"Dereference {inst.Operand}");
                    break;

                case UnaryOperatorType.Reference:
                    if (inst.OperandType != PrimitiveType.Long) throw new ArgumentException(nameof(inst));

                    yield return NasmInstruction.Call("lea", Memory(targetMemory), new DereferenceParameter(Memory(operandMemory))).WithComment($"Create reference to {inst.Operand}");
                    break;

                case UnaryOperatorType.PreDecrement:
                case UnaryOperatorType.PreIncrement:
                    yield return NasmInstruction.Call(_unaryOperators[inst.Operator, inst.OperandType], Memory(operandMemory));

                    if (targetMemory != operandMemory) yield return Move(targetMemory, operandMemory).WithComment("Assign operand to target");
                    break;

                case UnaryOperatorType.PostDecrement:
                case UnaryOperatorType.PostIncrement:
                    if (targetMemory != operandMemory) yield return Move(targetMemory, operandMemory).WithComment("Assign operand to target");

                    yield return NasmInstruction.Call(_unaryOperators[inst.Operator, inst.OperandType], Memory(operandMemory));
                    break;

                default:
                    if (targetMemory != operandMemory) yield return Move(targetMemory, operandMemory).WithComment("Assign operand to target");

                    yield return NasmInstruction.Call(_unaryOperators[inst.Operator, inst.OperandType], Memory(targetMemory));
                    break;
            }
        }

        private readonly OperatorTable<BinaryOperatorType> _binaryOperators = new OperatorTable<BinaryOperatorType>
        {
            [BinaryOperatorType.Add, PrimitiveType.Long] = "add",
            [BinaryOperatorType.Add, PrimitiveType.StringPointer] = "add",
            [BinaryOperatorType.Subtract, PrimitiveType.Long] = "sub",
            [BinaryOperatorType.Subtract, PrimitiveType.StringPointer] = "sub",
            [BinaryOperatorType.Multiply, PrimitiveType.Long] = "imul",
            [BinaryOperatorType.Assign] = "mov",
        };

        private readonly Dictionary<BinaryOperatorType, string> _binaryArithmeticComparisonOperators = new Dictionary<BinaryOperatorType, string>
        {
            [BinaryOperatorType.ComparisonGreaterThan] = "jg",
            [BinaryOperatorType.ComparisonGreaterThanOrEqual] = "jge",
            [BinaryOperatorType.ComparisonLessThan] = "jl",
            [BinaryOperatorType.ComparisonLessThanOrEqual] = "jle",
            [BinaryOperatorType.ComparisonEqual] = "je",
            [BinaryOperatorType.ComparisonNotEqual] = "jne",
        };

        private IEnumerable<NasmInstruction> CompileCore(BinaryComputationAssignment inst)
        {
            var lhsMemory = GetMemory(inst.Lhs);
            var rhsMemory = GetMemory(inst.Rhs);
            var targetMemory = TryGetMemory(inst.Target);

            if (targetMemory == null)
            {
                if (inst.Operator == BinaryOperatorType.Assign) targetMemory = lhsMemory;
                else yield break;
            }

            if (_binaryArithmeticComparisonOperators.TryGetValue(inst.Operator, out var jmp))
            {
                var trueLabel = "." + RequestName();
                var exitLabel = "." + RequestName();

                yield return NasmInstruction.Call("cmp", Memory(lhsMemory), Memory(rhsMemory)).WithComment("Set condition codes according to operands");
                yield return NasmInstruction.Call(jmp, new LabelParameter(trueLabel)).WithComment("Jump to True if the comparison is true");

                yield return Move(targetMemory, FalseConstant).WithComment("Assign false to output");
                yield return Jump(exitLabel).WithComment("Jump to Exit");

                yield return NasmInstruction.Label(trueLabel).WithComment("True");
                yield return Move(targetMemory, TrueConstant).WithComment("Assign true to output");

                yield return NasmInstruction.Label(exitLabel).WithComment("Exit");
            }
            else if (inst.OperandType == PrimitiveType.Long
                && (inst.Operator == BinaryOperatorType.Divide || inst.Operator == BinaryOperatorType.Remainder))
            {
                yield return NasmInstruction.Call("xor", Memory(RegisterName.RDX), Memory(RegisterName.RDX)).WithComment("Empty out higher bits of dividend");
                yield return Move(Register.Get(RegisterName.RAX), GetMemory(inst.Lhs)).WithComment("Assign lhs to dividend");

                IMemory divisor = GetMemory(inst.Rhs);

                if (!(divisor is Register))
                {
                    var register = Register.Get(RegisterName.RBX);
                    yield return Move(register, divisor).WithComment("Move divisor into RBX, as a register is required for idiv");
                    divisor = register;
                }

                yield return NasmInstruction.Call("idiv", Memory(divisor)).WithComment("Assign remainder to RDX, quotient to RAX");

                var result = inst.Operator switch
                {
                    BinaryOperatorType.Divide => Register.Get(RegisterName.RAX),
                    BinaryOperatorType.Remainder => Register.Get(RegisterName.RDX),
                    _ => throw new InvalidOperationException(),
                };

                yield return Move(GetMemory(inst.Target), result).WithComment("Assign result to target memory");
            }
            else
            {
                if (lhsMemory != targetMemory) yield return Move(targetMemory, lhsMemory).WithComment("Assign LHS to target memory");

                yield return NasmInstruction.Call(_binaryOperators[inst.Operator, inst.OperandType], Memory(targetMemory), Memory(rhsMemory));
            }
        }

        private IEnumerable<NasmInstruction> CompileCore(ConditionalJump inst)
        {
            var conditionMemory = GetMemory(inst.Condition);

            if (conditionMemory is DataLocation location)
            {
                var constant = ConstantTable.First(x => x.Value == location).Key;

                if ((int.TryParse(constant.ValueText, out var num) && num == 0) ^ inst.Inverted)
                {
                    yield return NasmInstruction.Comment("Always evaluates to false => No Jump");
                }
                else
                {
                    yield return Jump(inst.Target.Name).WithComment("Always evaluates to true => Unconditional Jump");
                }
            }
            else
            {
                yield return NasmInstruction.Call("test", Memory(conditionMemory), Memory(conditionMemory)).WithComment("Set condition codes according to condition");

                yield return NasmInstruction.Call(inst.Inverted ? "jz" : "jnz", new LabelParameter(inst.Target.Name)).WithComment("Jump if condition is " + (inst.Inverted ? "false/zero" : "true/non-zero"));
            }
        }

        private IEnumerable<NasmInstruction> CompileCore(UnconditionalJump inst)
        {
            yield return Jump(inst.Target.Name);
        }

        private IEnumerable<NasmInstruction> CompileCore(LabelInstruction inst)
        {
            yield return NasmInstruction.Label(inst.Name);
        }

        private IEnumerable<NasmInstruction> CompileCore(int index, CallInstruction inst)
        {
            var lives = RegisterAllocation
                .GetAllLiveAt(index)
                .Select((x, i) => (Variable: x, Temporary: new StackLocation(RegisterAllocation.StackOffset + 8 + (i * 8), 8)))
                .ToList();

            foreach (var live in lives)
            {
                yield return Move(live.Temporary, RegisterAllocation.AssignedLocation[live.Variable]).WithComment("Store live variable onto stack");
            }

            for (int i = 0; i < inst.Parameters.Count; i++)
            {
                yield return Move(ParameterLocation(i), GetMemory(inst.Parameters[i])).WithComment($"Pass parameter #{i}");
            }

            yield return NasmInstruction.Call("call", new LabelParameter(inst.Name));

            if (inst.Target != null && !(inst.Target is DiscardValue))
            {
                yield return Move(GetMemory(inst.Target), Register.Get(RegisterName.RAX)).WithComment($"Assign return value to {inst.Target}");
            }

            foreach (var live in lives)
            {
                yield return Move(RegisterAllocation.AssignedLocation[live.Variable], live.Temporary).WithComment("Restore live variable from stack");
            }
        }

        private IEnumerable<NasmInstruction> CompileCore(ReturnInstruction inst)
        {
            if (inst.ReturnValue != null)
            {
                yield return Move(Register.Get(RegisterName.RAX), GetMemory(inst.ReturnValue)).WithComment($"Return {inst.ReturnValue}");
            }

            yield return Jump(".__exit");
        }

        private IEnumerable<NasmInstruction> CompileCore(ParameterQueryAssignment inst)
        {
            if (inst.Target != null && !(inst.Target is DiscardValue))
            {
                yield return Move(GetMemory(inst.Target), ParameterLocation(inst.ParameterIndex)).WithComment($"Read parameter #{inst.ParameterIndex}");
            }
        }
    }
}