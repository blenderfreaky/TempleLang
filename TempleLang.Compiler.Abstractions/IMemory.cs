namespace TempleLang.Compiler.Abstractions
{
    public interface IMemory
    {
        int Size { get; }
        string DebugName { get; }
        ValueType Type { get; }
    }

    public enum ValueType
    {
        Local,
        ReturnValue,
        Constant,
        BasePointer,
    }
}
