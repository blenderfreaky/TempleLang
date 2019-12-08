namespace TempleLang.Test
{
    using System;
    using System.IO;
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

                IParserResult<Expression, Token> parserResult = Expression.Parser(lexemes);

                Console.WriteLine(parserResult);
            }
        }
    }
}
