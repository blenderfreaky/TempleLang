namespace TempleLang.Parser.Abstractions
{
    using Lexer;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ParserExtensions
    {
        public static IEnumerable<TEnum> GetEnumVals<TEnum>()
            where TEnum : Enum => (IEnumerable<TEnum>)Enum.GetValues(typeof(TEnum));

        public static Dictionary<TToken, NamedParser<Lexeme<TToken>, TToken>> TokenParsers<TToken>()
            where TToken : Enum =>
            GetEnumVals<TToken>().ToDictionary(x => x, Match<TToken>);

        public static Parser<U, TToken> Bind<T, U, TToken>(this Parser<T, TToken> parser, Func<ParserResult<T, TToken>, Parser<U, TToken>> binder) =>
            lexemeString =>
            {
                var result = parser(lexemeString);
                return binder(result)(result.RemainingLexemeString);
            };

        public static Parser<U, TToken> SelectMany<T, U, TToken>(this Parser<T, TToken> parser, Func<ParserResult<T, TToken>, Parser<U, TToken>> binder) =>
            Bind(parser, binder);

        public static Parser<U, TToken> Bind<T, U, TToken>(this NamedParser<T, TToken> parser, Func<ParserResult<T, TToken>, Parser<U, TToken>> binder) =>
            parser.Parser.Bind(binder);

        public static Parser<U, TToken> SelectMany<T, U, TToken>(this NamedParser<T, TToken> parser, Func<ParserResult<T, TToken>, Parser<U, TToken>> binder) =>
            Bind(parser, binder);

        public static Parser<U, TToken> Select<T, U, TToken>(this Parser<T, TToken> parser, Func<ParserResult<T, TToken>, U> binder) =>
            lexemeString =>
            {
                ParserResult<T, TToken> result = parser(lexemeString);

                return result.IsSuccessful
                    ? ParserResult.Success(binder(result), result.RemainingLexemeString)
                    : result.CastError<U>();
            };

        public static Parser<U, TToken> Select<T, U, TToken>(this NamedParser<T, TToken> parser, Func<ParserResult<T, TToken>, U> binder) =>
            parser.Parser.Select(binder);

        public static Parser<T, TToken> Or<T, TToken>(params Parser<T, TToken>[] parsers)
        {
            if (parsers.Length == 0) throw new ArgumentException("Sequence contains no elements", nameof(parsers));

            Parser<T, TToken> parser = parsers[0];

            for (int i = 1; i < parsers.Length; i++)
            {
                parser = parser.Bind(r => r.IsSuccessful ? r.AsParser() : parsers[i]);
            }

            return parser;
        }

        public static Parser<T, TToken> Or<T, TToken>(this Parser<T, TToken> first, Parser<T, TToken> second) =>
            first.Bind(r => r.IsSuccessful
                ? r.AsParser()
                : second);

        public static Parser<T, TToken> AsParser<T, TToken>(this ParserResult<T, TToken> result) =>
            _ => result;

        public static Parser<Lexeme<TToken>, TToken> Match<TToken>(this Predicate<TToken> predicate) =>
            lexemeString =>
            {
                var lexeme = lexemeString[0];

                if (predicate(lexeme.Token))
                {
                    return ParserResult.Success(lexeme, lexemeString.Advance(1));
                }

                return ParserResult.Failure<Lexeme<TToken>, TToken>(lexemeString, default! /*TODO*/, lexeme);
            };

        public static NamedParser<Lexeme<TToken>, TToken> Match<TToken>(this TToken token) =>
            Match<TToken>(x => EqualityComparer<TToken>.Default.Equals(x, token))
            .WithName((token ?? throw new ArgumentNullException(nameof(token))).ToString());

        public static Parser<T, TToken> Maybe<T, TToken>(this Parser<T, TToken> parser) =>
            lexemeString =>
            {
                var result = parser(lexemeString);

                if (result.IsSuccessful) return result;

                return default;
            };

        public static Parser<List<T>, TToken> Many<T, TToken>(this Parser<T, TToken> parser, int least = 1, int most = int.MaxValue) =>
            _lexemeString =>
            {
                var lexemeString = _lexemeString;

                var elements = new List<T>(least);

                for (int i = 0; i < most; i++)
                {
                    var result = parser(lexemeString);

                    if (result.IsSuccessful)
                    {
                        lexemeString = result.RemainingLexemeString;

                        elements.Add(result.Result);
                    }
                    else
                    {
                        if (i < least) return ParserResult.Failure<List<T>, TToken>(result.Error!.Value);

                        return ParserResult.Success(elements, lexemeString);
                    }
                }

                return ParserResult.Success(elements, lexemeString);
            };

        public static Parser<U, TToken> Many<T, U, TToken>(this Parser<T, TToken> parser, Func<T, U, U> aggregator, U start = default, int least = 1, int most = int.MaxValue) =>
            _lexemeString =>
            {
                var lexemeString = _lexemeString;

                U aggregate = start;

                for (int i = 0; i < most; i++)
                {
                    var result = parser(lexemeString);

                    if (result.IsSuccessful)
                    {
                        lexemeString = result.RemainingLexemeString;

                        aggregate = aggregator(result.Result, aggregate);
                    }
                    else
                    {
                        if (i < least) return ParserResult.Failure<U, TToken>(result.Error!.Value);

                        return ParserResult.Success(aggregate, lexemeString);
                    }
                }

                return ParserResult.Success(aggregate, lexemeString);
            };

        public static Parser<U, TToken> Transform<T, U, TToken>(this Parser<T, TToken> parser, Func<T, U> func) =>
            lexemeString =>
            {
                var result = parser(lexemeString);

                if (result.IsSuccessful) return ParserResult.Success(func(result.Result), result.RemainingLexemeString);

                return ParserResult.Failure<U, TToken>(result.Error!.Value);
            };

        public static NamedParser<T, TToken> WithName<T, TToken>(this Parser<T, TToken> parser, string name) =>
            new NamedParser<T, TToken>(name, parser);

        public static NamedParser<T, TToken> WithName<T, TToken>(this NamedParser<T, TToken> parser, string name) =>
            new NamedParser<T, TToken>(name, parser.Parser);

    }
}