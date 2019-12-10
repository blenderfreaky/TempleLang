namespace TempleLang.AbstractASM
{
    using System.Collections;
    using System.Collections.Generic;

    public struct TheoreticalMemory : IWriteableMemory
    {
        public int RegisterPriority { get; }
        public int Size { get; }

        public string DebugName { get; }
        public ValueType Type { get; }

        public TheoreticalMemory(int registerPriority, int size, string debugName, ValueType type)
        {
            RegisterPriority = registerPriority;
            Size = size;
            DebugName = debugName;
            Type = type;
        }
    }
}
