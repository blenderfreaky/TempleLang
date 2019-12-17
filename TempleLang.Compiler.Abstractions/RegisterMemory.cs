namespace TempleLang.Compiler.Abstractions
{
    public struct RegisterMemory : IMemory
    {
        public string RegisterName { get; }
        public int Size { get; }

        public string DebugName { get; }
        public ValueType Type { get; }

        public RegisterMemory(string registerName, int size, string debugName, ValueType type)
        {
            RegisterName = registerName;
            Size = size;
            DebugName = debugName;
            Type = type;
        }
    }
}
