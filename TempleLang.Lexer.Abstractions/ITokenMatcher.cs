using System;
using System.Collections.Generic;
using System.Text;

namespace TempleLang.Lexer.Abstractions
{
    public interface ITokenMatcher<out TToken, out TTokenType, in TFile>
        where TToken : IToken<TTokenType, TFile>
        where TFile : IFile
    {
        TTokenType TokenType { get; }

        bool StillMatches(char next);

        TToken ResultingToken(string text);

        void Reset();
    }
}
