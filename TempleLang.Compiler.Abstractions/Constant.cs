namespace TempleLang.Compiler.Abstractions
{
    public struct Constant : IMemory
    {
        public byte[]? ValueBytes { get; }
        public string? ValueText { get; }

        public int Size { get; }

        public string DebugName { get; }

        public MemoryValueType Type => MemoryValueType.Constant;

        public string ValueString => ValueBytes == null ? ValueText : "0x" + ByteArrayToString(ValueBytes);

        public Constant(string valueText, int size, string debugName)
        {
            ValueBytes = null;
            Size = size;
            ValueText = valueText;
            DebugName = debugName;
        }

        public Constant(byte[] value, string debugName)
        {
            ValueBytes = value;
            Size = ValueBytes.Length;
            ValueText = null;
            DebugName = debugName;
        }

        private static string ByteArrayToString(byte[] ba)
        {
            var hex = new System.Text.StringBuilder(ba.Length * 2);
            foreach (byte b in ba) hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(Constant left, Constant right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Constant left, Constant right)
        {
            return !(left == right);
        }
    }
}
