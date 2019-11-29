namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;

    public static class CommonParsers
    {
        public static NamedParser<List<T>, TToken> SeparatedBy<T, U, TToken>(this NamedParser<T, TToken> parser, NamedParser<U, TToken> separator) =>
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