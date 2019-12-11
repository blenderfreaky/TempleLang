namespace TempleLang.AbstractASM
{
    using System.Collections.Generic;

    public struct Operation
    {
        public IEnumerable<IStatement> Statements { get; }
    }

    public interface IStatement
    {
    }

    public struct ScopeStatement : IStatement
    {
        public CodeBlock Scope { get; }
        public IEnumerable<IStatement> Instructions { get; }
    }

    public struct ReturnStatement : IStatement
    {
        public IReadableMemory Value { get; }
    }

    public struct BinaryStatement : IStatement
    {
        public IWriteableMemory LeftOperand { get; }
        public IReadableMemory RightOperand { get; }

        public Architecture Architecture { get; }

        public string Command { get; }
    }

    public struct UnaryStatement : IStatement
    {
        public IWriteableMemory Operand { get; }

        public Architecture Operation { get; }

        public string Command { get; }
    }

    public enum Architecture
    {
        x64,
    }

    public struct Constant : IReadableMemory
    {
        public byte[] Value { get; }

        public int Size => Value.Length;

        public string DebugName { get; }

        public ValueType Type => ValueType.Constant;
    }
}
