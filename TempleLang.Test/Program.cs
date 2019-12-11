namespace TempleLang.Test
{
    using System;
    using System.IO;
    using TempleLang.Binder;
    using TempleLang.Lexer;
    using TempleLang.Lexer.Abstractions;
    using TempleLang.Parser;
    using TempleLang.Parser.Abstractions;

    public static class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                var text = Console.ReadLine();
                using var stringReader = new StringReader(text);

                var lexemes = new Lexer(
                    stringReader,
                    new SourceFile("Console", null))
                    .LexUntil(Token.EoF);

                Console.WriteLine(string.Join('\n', lexemes));
                Console.WriteLine();

                var parserResult =
                    (from r in Statement.Parser
                    from _ in Parse.Token(Token.EoF)
                    select r)(lexemes);

                Console.WriteLine(parserResult);
                Console.WriteLine();

                Binder binder = new Binder();

                var bound = binder.BindStatement(parserResult.Result);

                Console.WriteLine(bound);
                Console.WriteLine();

                foreach (var diagnostic in binder.Diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text));
            }
        }
    }
}
