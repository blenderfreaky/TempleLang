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
                //using var stringReader = new StringReader("abc[1] + 2.3 ^ .2 - (-3)");
                using var stringReader = new StringReader(Console.ReadLine());

                var lexemes = new Lexer(
                    stringReader,
                    new SourceFile("Console", null))
                    .LexUntil(Token.EoF);

                Console.WriteLine(string.Join('\n', lexemes));

                //var add = Token.IntegerLiteral.Match<Lexeme, Token, SourceFile>().SeparatedBy(Token.Add.Match<Lexeme, Token, SourceFile>());
                //ParserResult<List<Lexeme<Token, SourceFile>>, Lexeme<Token, SourceFile>, Token, SourceFile> parserResult = add.Parse(lexemes);

                var parserResult =
                    (from r in Statement.Parser
                    from _ in Parse.Token(Token.EoF)
                    select r)(lexemes);

                Console.WriteLine(parserResult);

                Binder binder = new Binder();

                var bound = binder.BindStatement(parserResult.Result);

                Console.WriteLine(bound);

                foreach (var diagnostic in binder.Diagnostics) Console.WriteLine(diagnostic);
            }
        }
    }
}
