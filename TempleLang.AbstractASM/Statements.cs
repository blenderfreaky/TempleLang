namespace TempleLang.AbstractASM
{
    using System.Collections.Generic;

    public struct Operation
    {
        public IEnumerable<IAsm> Statements { get; }
    }

    public interface IAsm
    {
    }

    public struct BinaryStatement : IAsm
    {
        public IWriteableMemory LeftOperand { get; }
        public IReadableMemory RightOperand { get; }

        public OpCode OpCode { get; }
    }

    public enum OpCode
    {
        MOV
    }
}
