using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TempleLang.Compiler.Abstractions
{
    public struct ConditionalLoop : IInstruction
    {
        public IReadableValue Condition { get; }
        public IEnumerable<IInstruction> ConditionComputationInstructions { get; }
        public IEnumerable<IInstruction> Instructions { get; }

        public bool IsDoLoop { get; }

        public ConditionalLoop(IReadableValue condition, IEnumerable<IInstruction> conditionComputationInstructions, IEnumerable<IInstruction> instructions, bool isDoLoop)
        {
            Condition = condition;
            ConditionComputationInstructions = conditionComputationInstructions;
            Instructions = instructions;
            IsDoLoop = isDoLoop;
        }

        public override bool Equals(object? obj) => obj is ConditionalLoop loop
            && EqualityComparer<IReadableValue>.Default.Equals(Condition, loop.Condition)
            && EqualityComparer<IEnumerable<IInstruction>>.Default.Equals(ConditionComputationInstructions, loop.ConditionComputationInstructions)
            && EqualityComparer<IEnumerable<IInstruction>>.Default.Equals(Instructions, loop.Instructions)
            && IsDoLoop == loop.IsDoLoop;

        public override int GetHashCode()
        {
            var hashCode = 1834674140;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadableValue>.Default.GetHashCode(Condition);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IEnumerable<IInstruction>>.Default.GetHashCode(ConditionComputationInstructions);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IEnumerable<IInstruction>>.Default.GetHashCode(Instructions);
            hashCode = (hashCode * -1521134295) + IsDoLoop.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ConditionalLoop left, ConditionalLoop right) => left.Equals(right);

        public static bool operator !=(ConditionalLoop left, ConditionalLoop right) => !(left == right);
    }
}
