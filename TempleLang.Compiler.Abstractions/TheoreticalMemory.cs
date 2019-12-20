namespace TempleLang.Compiler.Abstractions
{
    public struct TheoreticalMemory : IMemory
    {
        public int RegisterPriority { get; }
        public int StackOffset { get; }
        public int Size { get; }

        public string DebugName { get; }
        public MemoryValueType Type { get; }

        public TheoreticalMemory(int registerPriority, int stackOffset, int size, string debugName, MemoryValueType type)
        {
            RegisterPriority = registerPriority;
            StackOffset = stackOffset;
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

        public static bool operator ==(TheoreticalMemory left, TheoreticalMemory right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TheoreticalMemory left, TheoreticalMemory right)
        {
            return !(left == right);
        }
    }
}
