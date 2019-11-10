using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Lexer.Abstractions;

namespace TempleLang.Parser.Abstractions
{
    public class SeparatedListParser<T, TLexeme, TToken, TSourceFile>
        : IParser<SeparatedList<T>, TLexeme, TToken, TSourceFile>
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public SeparatedList<T> Parse(LexemeString<TLexeme, TToken, TSourceFile> lexer)
        {
            
        }
    }
}
