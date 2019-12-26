namespace TempleLang.Intermediate
{
    using System.Collections.Generic;

    public struct StackMemory : IMemory
    {
        public int StackOffset { get; }
        public int Size { get; }

        public string DebugName { get; }

        public StackMemory(int stackOffset, int size, string debugName)
        {
            StackOffset = stackOffset;
            Size = size;
            DebugName = debugName;
        }

        public override bool Equals(object? obj) =>
            obj is StackMemory memory
            && StackOffset == memory.StackOffset
            && Size == memory.Size
            && DebugName == memory.DebugName;

        public override int GetHashCode()
        {
            var hashCode = -1484992320;
            hashCode = (hashCode * -1521134295) + StackOffset.GetHashCode();
            hashCode = (hashCode * -1521134295) + Size.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(DebugName);
            return hashCode;
        }

        public static bool operator ==(StackMemory left, StackMemory right) => left.Equals(right);

        public static bool operator !=(StackMemory left, StackMemory right) => !(left == right);
    }
}
