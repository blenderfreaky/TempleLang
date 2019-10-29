using System.Collections.Generic;
using TempleLang.Lexer.Abstractions;

namespace TempleLang.Parser.Abstractions
{
    public interface IParserInput<out TToken, out TTokenType, out TSourceFile>
        where TToken : IToken<TTokenType, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public IEnumerable<TToken> Tokens { get; }
    }
}