namespace TempleLang.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using TempleLang.Lexer;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Join('\n', Lexer.Tokenize(new StringReader("abc[1] + 2.3 ^ .2 - (-3)"), new SourceFile("Console", null))));
        }
    }
}
