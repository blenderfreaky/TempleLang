namespace TempleLang.Intermediate
{
    using System.Collections.Generic;

    public struct CallInstruction : IInstruction
    {
        public string Name { get; }

        public CallInstruction(string name)
        {
            Name = name;
        }

        public override string ToString() => $"{Name}:";

        public override bool Equals(object? obj) => obj is CallInstruction instruction && Name == instruction.Name;

        public override int GetHashCode() => 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);

        public static bool operator ==(CallInstruction left, CallInstruction right) => left.Equals(right);

        public static bool operator !=(CallInstruction left, CallInstruction right) => !(left == right);
    }
}
