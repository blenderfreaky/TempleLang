namespace TempleLang.Compiler.NASM
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Compiler.Abstractions;

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
                Constant constant => constant.ValueString,
                _ => throw new InvalidOperationException(),
            });

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(AddressingParameter left, AddressingParameter right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AddressingParameter left, AddressingParameter right)
        {
            return !(left == right);
        }
    }
}
