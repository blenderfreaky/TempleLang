namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer.Abstractions;

    public static class CommonParsers
    {
        public static NamedParser<List<T>, TLexeme, TToken, TSourceFile> SeparatedBy<T, U, TLexeme, TToken, TSourceFile>(this NamedParser<T, TLexeme, TToken, TSourceFile> parser, NamedParser<U, TLexeme, TToken, TSourceFile> separator)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            parser
            .And(separator
                .And(parser)
                .Transform(x => x.Item2)
                .Many(least: 0))
            .Transform(x =>
            {
                x.Item2.Insert(0, x.Item1);
                return x.Item2;
            });
    }
}
