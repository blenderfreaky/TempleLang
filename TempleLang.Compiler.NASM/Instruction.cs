namespace TempleLang.Compiler.NASM
{
    using System.Linq;

    public struct Instruction
    {
        public string? Label { get; }
        public string? Name { get; }
        public IParameter[]? Parameters { get; }

        public Instruction(string label)
        {
            Label = label;
            Name = null;
            Parameters = null;
        }

        public Instruction(string name, params IParameter[] parameters)
        {
            Label = null;
            Name = name;
            Parameters = parameters;
        }

        public Instruction(string label, string name, params IParameter[] parameters)
        {
            Label = label;
            Name = name;
            Parameters = parameters;
        }

        public string ToNASM() => string.Concat(
            (Label == null ? string.Empty : (Label + ':')).PadRight(20),
            (Name ?? string.Empty).PadRight(6),
            Parameters == null ? string.Empty : string.Join(", ", Parameters.Select(x => x.ToNASM()))
            );

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(Instruction left, Instruction right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Instruction left, Instruction right)
        {
            return !(left == right);
        }
    }
}
