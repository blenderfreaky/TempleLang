namespace TempleLang.Compiler.Abstractions
{
    public struct StackMemory : IWriteableMemory
    {
        public int StackOffset { get; }
        public int Size { get; }

        public string DebugName { get; }
        public ValueType Type { get; }

        public StackMemory(int stackOffset, int size, string debugName, ValueType type)
        {
            StackOffset = stackOffset;
            Size = size;
            DebugName = debugName;
            Type = type;
        }
    }
}
