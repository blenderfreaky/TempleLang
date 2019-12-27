﻿namespace TempleLang.CodeGenerator.NASM
{
    using System.Collections.Generic;

    public struct DataLocation : IMemory
    {
        public string LabelName { get; }
        public int Size { get; }

        public DataLocation(string labelName, int size)
        {
            LabelName = labelName;
            Size = size;
        }

        public override string ToString() => $"{LabelName} ({Size} bytes)";

        public override bool Equals(object? obj) => obj is DataLocation location && LabelName == location.LabelName && Size == location.Size;

        public override int GetHashCode()
        {
            var hashCode = -634764822;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(LabelName);
            hashCode = (hashCode * -1521134295) + Size.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(DataLocation left, DataLocation right) => left.Equals(right);

        public static bool operator !=(DataLocation left, DataLocation right) => !(left == right);
    }
}