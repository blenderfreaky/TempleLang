namespace TempleLang.Compiler.Abstractions
{
    using System.Collections.Generic;

    public struct Constant : IMemory
    {
        public byte[] Value { get; }

        public int Size => Value.Length;

        public string DebugName { get; }

        public ValueType Type => ValueType.Constant;
    }
}
