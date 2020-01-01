namespace TempleLang.Parser.Abstractions
{
    using Lexer.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public interface IParserResult<out T, TToken>
    {
        public bool IsSuccessful { get; }

        public string? ErrorMessage { get; }

        public T Result { get; }

        public LexemeString<TToken> RemainingLexemeString { get; }
    }
}