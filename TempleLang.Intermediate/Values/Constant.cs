namespace TempleLang.Intermediate
{
    using System.Collections.Generic;
    using TempleLang.Bound.Primitives;

    public struct Constant : IImmutableValue
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "<Pending>")]
        public byte[]? ValueBytes { get; }

        public string? ValueText { get; }

        public PrimitiveType Type { get; }

        public string DebugName { get; }

        public string ValueString => ValueBytes == null ? ValueText! : "0x" + ByteArrayToString(ValueBytes);

        public Constant(string valueText, PrimitiveType type, string debugName)
        {
            ValueBytes = null;
            Type = type;
            ValueText = valueText;
            DebugName = debugName;
        }

        public Constant(byte[] value, PrimitiveType type, string debugName)
        {
            ValueBytes = value;
            Type = type;
            ValueText = null;
            DebugName = debugName;
        }

        public override string ToString() => $"{ValueString}";

        private static string ByteArrayToString(byte[] ba)
        {
            var hex = new System.Text.StringBuilder(ba.Length * 2);
            foreach (byte b in ba) hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public override bool Equals(object? obj) =>
            obj is Constant constant
            && EqualityComparer<byte[]?>.Default.Equals(ValueBytes, constant.ValueBytes)
            && ValueText == constant.ValueText
            && Type == constant.Type
            && DebugName == constant.DebugName
            && ValueString == constant.ValueString;

        public override int GetHashCode()
        {
            var hashCode = -446177807;
            hashCode = (hashCode * -1521134295) + EqualityComparer<byte[]?>.Default.GetHashCode(ValueBytes);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(ValueText);
            hashCode = (hashCode * -1521134295) + EqualityComparer<PrimitiveType>.Default.GetHashCode(Type);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(DebugName);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(ValueString);
            return hashCode;
        }

        public static bool operator ==(Constant left, Constant right) => left.Equals(right);

        public static bool operator !=(Constant left, Constant right) => !(left == right);
    }
}