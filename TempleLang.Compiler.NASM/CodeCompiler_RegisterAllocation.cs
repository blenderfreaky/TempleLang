namespace TempleLang.CodeGenerator.NASM
{
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Intermediate;

    public class RegisterAllocation
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

            AssignedLocations = new Dictionary<Variable, IMemory>();
            FreeGeneralPurposeRegisters = new Stack<Register>(
                Register.All
                .Where(x => (x.Flags & RegisterFlags.GeneralPurpose) != 0));

            Active = new List<LiveInterval>();

            AllocateRegisters();
        }

        public Dictionary<Variable, IMemory> AssignedLocations { get; }
        private Stack<Register> FreeGeneralPurposeRegisters { get; }
        public int StackSize { get; private set; }

        private List<LiveInterval> Active { get; }

        private Dictionary<Variable, (int First, int Last)> LiveIntervalsByVariable { get; }
        private List<LiveInterval> LiveIntervals { get; }

        private void CalculateLiveIntervals()
        {
            void adaptLiveIntervals(int index, IReadableValue value)
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
                    AssignedLocations[interval.Variable] = register;
                }
            }
        }

        private void ExpireOld(LiveInterval interval)
        {
            foreach (var other in LiveIntervals)
            {
                if (other.LastIndex >= interval.FirstIndex) return;

                Active.Remove(other);
                var memory = AssignedLocations[other.Variable];
                if (memory is Register register) FreeGeneralPurposeRegisters.Push(register);
            }
        }

        private void Spill(LiveInterval interval)
        {
            var spilled = Active.Last();

            if (spilled.LastIndex > interval.LastIndex)
            {
                AssignedLocations[interval.Variable] = AssignedLocations[spilled.Variable];
                AssignedLocations[spilled.Variable] = StackAlloc(8);

                Active.Remove(spilled);
                Active.Add(interval);
            }
            else
            {
                AssignedLocations[interval.Variable] = StackAlloc(8);
            }
        }

        private StackLocation StackAlloc(int size)
        {
            var location = new StackLocation(StackSize, size);
            StackSize += size;
            return location;
        }
    }
}
