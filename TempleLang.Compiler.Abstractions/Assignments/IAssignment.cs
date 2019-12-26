namespace TempleLang.Compiler.Abstractions
{
    public interface IAssignment : IInstruction
    {
        IAssignableValue Target { get; }
    }
}
