using System;
using System.Collections.Generic;
using System.Linq;

namespace TempleLang.Lexer.Abstractions
{
    public struct LexerState<TTokenType>
    {
        public string Name { get; }

        public LexerStateStep<TTokenType> Step { get; }

        public LexerState(string name, LexerStateStep<TTokenType> step)
        {
            Name = name;
            Step = step;
        }

        public static LexerState<TTokenType> Combine(string name, LexerStateStep<TTokenType> fallback, params PartialLexerStateStep<TTokenType>[] partial) =>
            new LexerState<TTokenType>(name, character =>
            {
                for (int i = partial.Length; i >= 0; i--)
                {
                    var step = partial[i](character);
                    if (step.HasValue) return step.Value;
                }
                return fallback(character);
            });
    }

    public delegate LexerStep<TTokenType> LexerStateStep<TTokenType>(int character);
    public delegate LexerStep<TTokenType>? PartialLexerStateStep<TTokenType>(int character);
}
