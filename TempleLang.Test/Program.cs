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

                var parser = Expression.Parser;

                //var aparser = Parse.RightAssociative(
                //    NumberLiteral.Parser.OfType<Expression, Token>(),
                //    from expr in NumberLiteral.Parser
                //    from op in Parse.Token(Token.Add)
                //    select (op, expr),
                //    (a, x) => new BinaryExpression(x.expr, a, x.op));

                //var parser =
                //    aparser.Or(
                //    from _ in Parse.Token(Token.LeftExpressionDelimiter)
                //    from expr in aparser
                //    from __ in Parse.Token(Token.RightExpressionDelimiter)
                //    select expr);

                var eofParser = parser;
                    //(from r in parser
                    // from _ in Parse.Token(Token.EoF)
                    // select r);

                var parserResult = eofParser(lexemes);

                Console.WriteLine(parserResult);
                Console.WriteLine();

                //Binder binder = new Binder();

                //var bound = binder.BindStatement(parserResult.Result);

                //Console.WriteLine(bound);
                //Console.WriteLine();

                //foreach (var diagnostic in binder.Diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text));
            }
        }
    }
}
