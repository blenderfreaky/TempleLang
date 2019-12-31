namespace TempleLang.Intermediate
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public struct CallInstruction : IInstruction
    {
        public string Name { get; }
        public IReadOnlyList<IReadableValue> Parameters { get; }

        public CallInstruction(string name, IReadOnlyList<IReadableValue> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public override string ToString() => $"call {Name}({string.Join(", ", Parameters)})";
        public override bool Equals(object? obj) => obj is CallInstruction instruction && Name == instruction.Name && EqualityComparer<IReadOnlyList<IReadableValue>>.Default.Equals(Parameters, instruction.Parameters);

        public override int GetHashCode()
        {
            var hashCode = 497090031;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyList<IReadableValue>>.Default.GetHashCode(Parameters);
            return hashCode;
        }

        public static bool operator ==(CallInstruction left, CallInstruction right) => left.Equals(right);

        public static bool operator !=(CallInstruction left, CallInstruction right) => !(left == right);
    }
}
