namespace TempleLang.Intermediate
{
    public interface IMemory : IAssignableValue, IReadableValue
    {
        int Size { get; }
        string DebugName { get; }
    }
}
