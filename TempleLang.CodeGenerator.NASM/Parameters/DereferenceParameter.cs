namespace TempleLang.CodeGenerator.NASM
{
    using System.Collections.Generic;

    public struct DereferenceParameter : IParameter
    {
        public IParameter Parameter { get; }

        public DereferenceParameter(IParameter parameter)
        {
            Parameter = parameter;
        }

        public string ToNASM() => "[" + Parameter.ToNASM() + "]";

        public override string ToString() => ToNASM();

        public override bool Equals(object? obj) => obj is DereferenceParameter parameter && EqualityComparer<IParameter>.Default.Equals(Parameter, parameter.Parameter);

        public override int GetHashCode() => -882409680 + EqualityComparer<IParameter>.Default.GetHashCode(Parameter);

        public static bool operator ==(DereferenceParameter left, DereferenceParameter right) => left.Equals(right);

        public static bool operator !=(DereferenceParameter left, DereferenceParameter right) => !(left == right);
    }
}