namespace TempleLang.Compiler.Abstractions
{
    public struct StackMemory : IMemory
    {
        public int StackOffset { get; }
        public int Size { get; }

        public string DebugName { get; }
        public MemoryValueType Type { get; }

        public StackMemory(int stackOffset, int size, string debugName, MemoryValueType type)
        {
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

        public static bool operator ==(StackMemory left, StackMemory right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StackMemory left, StackMemory right)
        {
            return !(left == right);
        }
    }
}
