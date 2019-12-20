namespace TempleLang.Compiler.Abstractions
{
    public struct RegisterMemory : IMemory
    {
        public string RegisterName { get; }
        public int Size { get; }

        public string DebugName { get; }
        public MemoryValueType Type { get; }

        public RegisterMemory(string registerName, int size, string debugName, MemoryValueType type)
        {
            RegisterName = registerName;
            Size = size;
            DebugName = debugName;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(RegisterMemory left, RegisterMemory right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RegisterMemory left, RegisterMemory right)
        {
            return !(left == right);
        }
    }
}
