namespace TempleLang.Intermediate
{
    using System.Collections.Generic;

    public struct Conditional : IInstruction
    {
        public IReadableValue Condition { get; }
        public IReadOnlyList<IInstruction> TrueInstructions { get; }
        public IReadOnlyList<IInstruction> FalseInstructions { get; }

        public Conditional(IReadableValue condition, IReadOnlyList<IInstruction> trueInstructions, IReadOnlyList<IInstruction> falseInstructions)
        {
            Condition = condition;
            TrueInstructions = trueInstructions;
            FalseInstructions = falseInstructions;
        }

        public override bool Equals(object? obj) =>
            obj is Conditional conditional
            && EqualityComparer<IReadableValue>.Default.Equals(Condition, conditional.Condition)
            && EqualityComparer<IReadOnlyList<IInstruction>>.Default.Equals(TrueInstructions, conditional.TrueInstructions)
            && EqualityComparer<IReadOnlyList<IInstruction>>.Default.Equals(FalseInstructions, conditional.FalseInstructions);

        public override int GetHashCode()
        {
            var hashCode = -2016997293;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadableValue>.Default.GetHashCode(Condition);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyList<IInstruction>>.Default.GetHashCode(TrueInstructions);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyList<IInstruction>>.Default.GetHashCode(FalseInstructions);
            return hashCode;
        }

        public static bool operator ==(Conditional left, Conditional right) => left.Equals(right);

        public static bool operator !=(Conditional left, Conditional right) => !(left == right);
    }
}
