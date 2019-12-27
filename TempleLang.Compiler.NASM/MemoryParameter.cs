﻿namespace TempleLang.CodeGenerator.NASM
{
    using System;
    using System.Collections.Generic;

    public struct MemoryParameter : IParameter
    {
        public IMemory Memory { get; }

        public MemoryParameter(IMemory memory)
        {
            Memory = memory;
        }

        private static WordSize GetWordSize(int size) => (WordSize)size;

        public string ToNASM() =>
            GetWordSize(Memory.Size).ToString().ToLowerInvariant()
            + " "
            + (Memory switch
            {
                Register register => register.RegisterName,
                StackLocation stack => $"[rsp - {stack.Offset}]",
                DataLocation data => data.LabelName,
                _ => throw new InvalidOperationException(),
            });

        public override string ToString() => ToNASM();

        public override bool Equals(object? obj) => obj is MemoryParameter parameter && EqualityComparer<IMemory>.Default.Equals(Memory, parameter.Memory);

        public override int GetHashCode() => -140318722 + EqualityComparer<IMemory>.Default.GetHashCode(Memory);

        public static bool operator ==(MemoryParameter left, MemoryParameter right) => left.Equals(right);

        public static bool operator !=(MemoryParameter left, MemoryParameter right) => !(left == right);
    }
}