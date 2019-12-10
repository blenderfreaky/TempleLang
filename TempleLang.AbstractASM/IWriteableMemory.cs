namespace TempleLang.AbstractASM
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IReadableMemory
    {
        int Size { get; }
        string DebugName { get; }
        ValueType Type { get; }
    }

    public interface IWriteableMemory : IReadableMemory
    {
    }

    public enum ValueType
    {
        Local,
        ReturnValue,
        Constant,
        BasePointer,
    }
}
