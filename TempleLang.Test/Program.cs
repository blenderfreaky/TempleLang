namespace TempleLang.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using TempleLang.Lexer;
    using TempleLang.Lexer.Abstractions;
    using TempleLang.Parser.Abstractions;
    using Lexeme = Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>;

    public static class Program
    {
        public static void Main(string[] args)
        {
            //using var stringReader = new StringReader("abc[1] + 2.3 ^ .2 - (-3)");
            using var stringReader = new StringReader(Console.ReadLine());

            var lexemes = new Lexer(
                stringReader,
                new SourceFile("Console", null))
                .LexUntil(Token.EoF);

            Console.WriteLine(string.Join('\n', lexemes));

            var add = Token.IntegerLiteral.Match<Lexeme, Token, SourceFile>().SeparatedBy(Token.Add.Match<Lexeme, Token, SourceFile>());

            ParserResult<List<Lexeme<Token, SourceFile>>, Lexeme<Token, SourceFile>, Token, SourceFile> parserResult = add.Parse(lexemes);

            Console.WriteLine(parserResult.Result);
        }
    }
}
