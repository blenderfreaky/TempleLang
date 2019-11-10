namespace TempleLang.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using TempleLang.Lexer;
    using TempleLang.Lexer.Abstractions;

    public static class Program
    {
        public static void Main(string[] args)
        {
            using var stringReader = new StringReader("abc[1] + 2.3 ^ .2 - (-3)");

            Console.WriteLine(string.Join('\n', new Lexer(
                stringReader,
                new SourceFile("Console", null))
                .EnumerateLexemes(Token.EoF)));
        }

        public static IEnumerable<TLexeme> EnumerateLexemes<TLexeme, TToken, TSourceFile>(this ILexer<TLexeme, TToken, TSourceFile> lexer, TToken eof)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile
        {
            while (true)
            {
                var lexeme = lexer.LexOne();
                yield return lexeme;

                if (EqualityComparer<TToken>.Default.Equals(lexeme.Token, eof)) yield break;
            }
        }
    }
}
