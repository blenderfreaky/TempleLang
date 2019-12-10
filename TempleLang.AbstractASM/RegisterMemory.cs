namespace TempleLang.AbstractASM
{
    using System.Collections;
    using System.Collections.Generic;

    public struct RegisterMemory : IWriteableMemory
    {
        public int RegisterIndex { get; }
        public int Size { get; }

        public string DebugName { get; }
        public ValueType Type { get; }

        public RegisterMemory(int registerIndex, int size, string debugName, ValueType type)
        {
            RegisterIndex = registerIndex;
            Size = size;
            DebugName = debugName;
            Type = type;
        }
    }
}
