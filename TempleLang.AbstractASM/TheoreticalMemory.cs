namespace TempleLang.AbstractASM
{
    public struct TheoreticalMemory : IWriteableMemory
    {
        public int RegisterPriority { get; }
        public int StackOffset { get; }
        public int Size { get; }

        public string DebugName { get; }
        public ValueType Type { get; }

        public TheoreticalMemory(int registerPriority, int stackOffset, int size, string debugName, ValueType type)
        {
            RegisterPriority = registerPriority;
            StackOffset = stackOffset;
            Size = size;
            DebugName = debugName;
            Type = type;
        }
    }
}
