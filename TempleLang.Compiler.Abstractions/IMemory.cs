namespace TempleLang.Compiler.Abstractions
{
    public interface IMemory
    {
        int Size { get; }
        string DebugName { get; }
        MemoryValueType Type { get; }
    }

    public enum MemoryValueType
    {
        Local,
        ReturnValue,
        Constant,
        BasePointer,
    }
}
