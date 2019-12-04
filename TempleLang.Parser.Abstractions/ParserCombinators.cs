namespace TempleLang.Parser.Abstractions
{
    using System;

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

                var rightResult = right(leftResult.Result)(input);

                return rightResult;
            };

        public static Parser<V, TToken> SelectMany<T, U, V, TToken>(this Parser<T, TToken> left, Func<T, Parser<U, TToken>> right, Func<T, U, V> selector) =>
            input =>
            {
                var leftResult = left(input);

                if (!leftResult.IsSuccessful) return ParserResult.Failure<T, V, TToken>(leftResult);

                var rightResult = right(leftResult.Result)(input);

                if (!rightResult.IsSuccessful) return ParserResult.Failure<U, V, TToken>(rightResult);

                return ParserResult.Success(selector(leftResult.Result!, rightResult.Result!), rightResult.RemainingLexemeString);
            };

        public static Parser<T, TToken> Ref<T, TToken>(Func<Parser<T, TToken>> parser) =>
            input => parser()(input);
    }
}
