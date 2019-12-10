namespace TempleLang.Parser.Abstractions
{
    using Diagnostic;
    using Lexer.Abstractions;
    using System;
    using System.Collections.Generic;

    public static class ParserCombinators
    {
        public static Parser<T, TToken> Or<T, TToken>(this Parser<T, TToken> left, Parser<T, TToken> right) =>
            input =>
            {
                var leftResult = left(input);

                if (leftResult.IsSuccessful) return leftResult;

                var rightResult = right(input);

                if (rightResult.IsSuccessful) return rightResult;

                // TODO: Combinate errors
                return leftResult;
            };

        public static Parser<U, TToken> SelectMany<T, U, TToken>(this Parser<T, TToken> left, Func<T, Parser<U, TToken>> right) =>
            input =>
            {
                var leftResult = left(input);

                if (!leftResult.IsSuccessful) return ParserResult.Failure<T, U, TToken>(leftResult);

                var rightResult = right(leftResult.Result)(leftResult.RemainingLexemeString);

                return rightResult;
            };

        public static Parser<V, TToken> SelectMany<T, U, V, TToken>(this Parser<T, TToken> left, Func<T, Parser<U, TToken>> right, Func<T, U, V> selector) =>
            input =>
            {
                var leftResult = left(input);

                if (!leftResult.IsSuccessful) return ParserResult.Failure<T, V, TToken>(leftResult);

                var rightResult = right(leftResult.Result)(leftResult.RemainingLexemeString);

                if (!rightResult.IsSuccessful) return ParserResult.Failure<U, V, TToken>(rightResult);

                return ParserResult.Success(selector(leftResult.Result!, rightResult.Result!), rightResult.RemainingLexemeString);
            };

        public static Parser<U, TToken> Transform<T, U, TToken>(this Parser<T, TToken> parser, Func<T, U> selector) =>
            input =>
            {
                var result = parser(input);

                if (!result.IsSuccessful) return ParserResult.Failure<T, U, TToken>(result);

                return ParserResult.Success(selector(result.Result!), result.RemainingLexemeString);
            };

        public static Parser<U, TToken> As<T, U, TToken>(this Parser<T, TToken> parser, U value) =>
            parser.Transform(_ => value);

        public static Parser<Positioned<U>, TToken> AsPositioned<T, U, TToken>(this Parser<T, TToken> parser, U value)
            where T : IPositioned =>
            parser.Transform(x => x.Location.WithValue(value));

        public static Parser<List<T>, TToken> Many<T, TToken>(this Parser<T, TToken> parser, int least = 0, int most = int.MaxValue) =>
            input =>
            {
                List<T> elements = new List<T>();
                LexemeString<TToken> remainder = input;

                for (int i = 1; i <= most; i++)
                {
                    var result = parser(input);

                    if (!result.IsSuccessful)
                    {
                        if (i < least)
                        {
                            return ParserResult.Failure<List<T>, TToken>("Too few elements. Expected at least " + least, remainder);
                        }

                        return ParserResult.Success(elements, remainder);
                    }

                    remainder = result.RemainingLexemeString;
                }

                return ParserResult.Success(elements, remainder);
            };

        public static Parser<T, TToken> OfType<T, TToken>(this Parser<T, TToken> parser) => parser;
    }
}
