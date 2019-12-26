namespace TempleLang.Compiler.Abstractions
{
    public interface IMemory : IAssignableValue, IReadableValue
    {
        int Size { get; }
        string DebugName { get; }
    }
}
