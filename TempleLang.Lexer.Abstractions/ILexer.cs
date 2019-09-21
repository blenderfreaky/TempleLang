namespace TempleLang.Lexer.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public interface ILexer<out TToken, out TTokenType>
        where TToken : IToken<TTokenType>
    {
        IEnumerable<TToken> Tokenize(TextReader text);
    }
}
