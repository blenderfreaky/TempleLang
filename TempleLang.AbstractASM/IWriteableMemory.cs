namespace TempleLang.AbstractASM
{
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
