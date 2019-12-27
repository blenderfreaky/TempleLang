namespace TempleLang.CodeGenerator.NASM
{
    using System.Collections.Generic;
    using System.Linq;

    public struct NasmInstruction
    {
        public string? Label { get; }
        public string? Name { get; }
        public IReadOnlyList<IParameter>? Parameters { get; }

        public NasmInstruction(string label)
        {
            Label = label;
            Name = null;
            Parameters = null;
        }

        public NasmInstruction(string name, params IParameter[] parameters)
        {
            Label = null;
            Name = name;
            Parameters = parameters;
        }

        public NasmInstruction(string label, string name, params IParameter[] parameters)
        {
            Label = label;
            Name = name;
            Parameters = parameters;
        }

        public string ToNASM() => string.Concat(
            (Label == null ? string.Empty : (Label + ':')).PadRight(4),
            (Name ?? string.Empty).PadRight(6),
            Parameters == null ? string.Empty : string.Join(", ", Parameters.Select(x => x.ToNASM()))
            );
        public override bool Equals(object? obj) => obj is NasmInstruction instruction
            && Label == instruction.Label
            && Name == instruction.Name
            && EqualityComparer<IReadOnlyList<IParameter>?>.Default.Equals(Parameters, instruction.Parameters);

        public override int GetHashCode()
        {
            var hashCode = 913549532;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(Label);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyList<IParameter>?>.Default.GetHashCode(Parameters);
            return hashCode;
        }

        public static bool operator ==(NasmInstruction left, NasmInstruction right) => left.Equals(right);

        public static bool operator !=(NasmInstruction left, NasmInstruction right) => !(left == right);
    }
}
