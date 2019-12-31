namespace TempleLang.CodeGenerator.NASM
{
    using Bound.Expressions;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Intermediate;

    public sealed class RegisterAllocation
    {
        private class LiveInterval
        {
            public Variable Variable { get; }
            public int FirstIndex { get; }
            public int LastIndex { get; }

            public LiveInterval(Variable variable, int firstIndex, int lastIndex)
            {
                Variable = variable;
                FirstIndex = firstIndex;
                LastIndex = lastIndex;
            }

            public override string ToString() => $"{Variable}: {FirstIndex} - {LastIndex}";
        }

        public List<IInstruction> Instructions { get; }

        public static RegisterAllocation Generate(List<IInstruction> instructions) => new RegisterAllocation(instructions);

        private RegisterAllocation(List<IInstruction> instructions)
        {
            Instructions = instructions;

            LiveIntervalsByVariable = new Dictionary<Variable, (int First, int Last)>();

            CalculateLiveIntervals();

            LiveIntervals = LiveIntervalsByVariable
                .Select(x => new LiveInterval(x.Key, x.Value.First, x.Value.Last))
                .OrderBy(x => x.FirstIndex)
                .ToList();

            AssignedLocation = new Dictionary<Variable, IMemory>();
            FreeGeneralPurposeRegisters = new Stack<Register>(
                Register.All
                .Where(x => x.Size == RegisterSize.Bytes8 && (x.Flags & RegisterFlags.GeneralPurpose) != 0));

            Active = new List<LiveInterval>();

            AllocateRegisters();
        }

        public Dictionary<Variable, IMemory> AssignedLocation { get; }
        private Stack<Register> FreeGeneralPurposeRegisters { get; }

        public int StackOffset { get; private set; } =
              8 // Return address
            + 8 // Saved frame pointer
            ;

        private List<LiveInterval> Active { get; }

        private Dictionary<Variable, (int First, int Last)> LiveIntervalsByVariable { get; }
        private List<LiveInterval> LiveIntervals { get; }

        public IEnumerable<Variable> GetAllLiveAt(int index) => LiveIntervals.Where(x => x.FirstIndex <= index && x.LastIndex >= index).Select(x => x.Variable);

        private void CalculateLiveIntervals()
        {
            void adaptLiveIntervals(int index, IReadableValue? value)
            {
                if (!(value is Variable var)) return;

                if (!LiveIntervalsByVariable.TryGetValue(var, out var pos))
                {
                    LiveIntervalsByVariable[var] = (index, index);
                }
                else
                {
                    LiveIntervalsByVariable[var] = (pos.First, index);
                }
            }

            for (int i = 0; i < Instructions.Count; i++)
            {
                switch (Instructions[i])
                {
                    case BinaryComputationAssignment inst:
                        adaptLiveIntervals(i, inst.Target);
                        adaptLiveIntervals(i, inst.Lhs);
                        adaptLiveIntervals(i, inst.Rhs);
                        break;

                    case UnaryComputationAssignment inst:
                        adaptLiveIntervals(i, inst.Target);
                        adaptLiveIntervals(i, inst.Operand);
                        break;

                    case ConditionalJump inst:
                        adaptLiveIntervals(i, inst.Condition);
                        break;

                    case ReturnInstruction inst:
                        adaptLiveIntervals(i, inst.ReturnValue);
                        break;

                    case CallInstruction inst:
                        foreach (var param in inst.Parameters) adaptLiveIntervals(i, param);
                        break;
                }
            }
        }

        private void AllocateRegisters()
        {
            foreach (var interval in LiveIntervals)
            {
                ExpireOld(interval);
                if (FreeGeneralPurposeRegisters.Count == 0)
                {
                    Spill(interval);
                }
                else
                {
                    var register = FreeGeneralPurposeRegisters.Pop();
                    Active.Add(interval);
                    AssignedLocation[interval.Variable] = register;
                }
            }
        }

        private void ExpireOld(LiveInterval interval)
        {
            for (int i = 0; i < Active.Count; i++)
            {
                var other = Active[i];

                if (other.LastIndex >= interval.FirstIndex) continue;

                Active.RemoveAt(i);
                i--;

                var memory = AssignedLocation[other.Variable];
                if (memory is Register register) FreeGeneralPurposeRegisters.Push(register);
            }
        }

        private void Spill(LiveInterval interval)
        {
            var spilled = Active.Last();

            if (spilled.LastIndex > interval.LastIndex)
            {
                AssignedLocation[interval.Variable] = AssignedLocation[spilled.Variable];
                AssignedLocation[spilled.Variable] = StackAlloc(8);

                Active.Remove(spilled);
                Active.Add(interval);
            }
            else
            {
                AssignedLocation[interval.Variable] = StackAlloc(8);
            }
        }

        private StackLocation StackAlloc(int size)
        {
            var location = new StackLocation(StackOffset, size);
            StackOffset += size;
            return location;
        }

        public StackLocation StackAllocAfterSpills(int size)
        {
            var location = new StackLocation(StackOffset, size);
            return location;
        }
    }
}