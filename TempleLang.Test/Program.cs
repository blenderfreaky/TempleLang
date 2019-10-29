namespace TempleLang.Test
{
    using System;
    using System.IO;
    using TempleLang.Lexer;

    class Program
    {
        static void Main(string[] args)
        {
            using var stringReader = new StringReader("abc[1] + 2.3 ^ .2 - (-3)");

            Console.WriteLine(string.Join('\n', Lexer.(
                stringReader,
                new SourceFile("Console", null))));
        }
    }
}
