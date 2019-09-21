namespace TempleLang.Lexer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TempleLang.Lexer.Abstractions;

    public class Lexer : ILexer<Token, TokenType>
    {
        public IEnumerable<Token> Tokenize(TextReader text)
        {

        }
    }
}
