namespace TempleLang.Intermediate.NASM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public struct AddressingParameter : IParameter
    {
        public IMemory Memory { get; }

        public AddressingParameter(IMemory memory) => Memory = memory;

        private static readonly Dictionary<int, WordSize> WordSizes = ((IEnumerable<WordSize>)Enum.GetValues(typeof(WordSize))).ToDictionary(x => (int)x, x => x);

        public string ToNASM() =>
            WordSizes[Memory.Size].ToString().ToLowerInvariant()
            + " "
            + (Memory switch
            {
                RegisterMemory register => register.RegisterName,
                StackMemory stack => $"[rsi + {stack.StackOffset}]",
                _ => throw new InvalidOperationException(),
            });

        public override bool Equals(object? obj) => obj is AddressingParameter parameter && EqualityComparer<IMemory>.Default.Equals(Memory, parameter.Memory);
        public override int GetHashCode() => -140318722 + EqualityComparer<IMemory>.Default.GetHashCode(Memory);

        public static bool operator ==(AddressingParameter left, AddressingParameter right) => left.Equals(right);

        public static bool operator !=(AddressingParameter left, AddressingParameter right) => !(left == right);
    }
}
