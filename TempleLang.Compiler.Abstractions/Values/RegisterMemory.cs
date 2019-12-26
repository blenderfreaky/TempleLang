namespace TempleLang.Compiler.Abstractions
{
    using System.Collections.Generic;

    public struct RegisterMemory : IMemory
    {
        public string RegisterName { get; }
        public int Size { get; }

        public string DebugName { get; }

        public RegisterMemory(string registerName, int size, string debugName)
        {
            RegisterName = registerName;
            Size = size;
            DebugName = debugName;
        }

        public override bool Equals(object? obj) =>
            obj is RegisterMemory memory
            && RegisterName == memory.RegisterName
            && Size == memory.Size
            && DebugName == memory.DebugName;

        public override int GetHashCode()
        {
            var hashCode = -1210858169;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(RegisterName);
            hashCode = (hashCode * -1521134295) + Size.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(DebugName);
            return hashCode;
        }

        public static bool operator ==(RegisterMemory left, RegisterMemory right) => left.Equals(right);

        public static bool operator !=(RegisterMemory left, RegisterMemory right) => !(left == right);
    }
}
