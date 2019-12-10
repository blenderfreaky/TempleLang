namespace TempleLang.AbstractASM
{
    public interface IStackScope
    {
        bool TryFindValue(string name, out IWriteableMemory valueReference);
        int StackSize { get; }
    }
}
