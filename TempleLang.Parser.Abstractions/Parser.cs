namespace TempleLang.Parser.Abstractions
{
    using Lexer;
    using System;
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    public delegate IParserResult<T, TToken> Parser<out T, TToken>(LexemeString<TToken> lexemeString);

    public static class Parse
    {
        public static Parser<Lexeme<TToken>, TToken> Token<TToken>(TToken token) =>
            input =>
            {
                if (input.Length == 0)
                {
                    return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected {token}", input);
                }

                if (!EqualityComparer<TToken>.Default.Equals(input[0].Token, token))
                {
                    return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected {token}", input);
                }

                return ParserResult.Success(input[0], input.Advance(1));
            };

        public static Parser<Lexeme<TToken>, TToken> Token<TToken>(params TToken[] tokens) =>
            input =>
            {
                if (input.Length == 0)
                {
                    return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected [{string.Join(", ", tokens)}]", input);
                }

                foreach (var token in tokens)
                {
                    if (!EqualityComparer<TToken>.Default.Equals(input[0].Token, token))
                    {
                        continue;
                    }

                    return ParserResult.Success(input[0], input.Advance(1));
                }

                return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected [{string.Join(", ", tokens)}]", input);
            };

        public static Parser<T, TToken> Ref<T, TToken>(Func<Parser<T, TToken>> parser) =>
            input => parser()(input);

        public static Parser<T, TToken> Value<T, TToken>(T value = default) =>
            input => ParserResult.Success<T, TToken>(value, input);

        private class RefContainer<T>
            where T : class
        {
            public T? Value { get; set; }

            public RefContainer() => Value = null;
            public RefContainer(T? value) => Value = value;
        }

        public static Parser<T, TToken> Recursive<T, TToken>(Func<Parser<T, TToken>, Parser<T, TToken>> creator)
        {
            var refContainer = new RefContainer<Parser<T, TToken>>();

            var result = creator(Ref(() => refContainer.Value!));

            refContainer.Value = result;

            return result;
        }

        //public static Parser<T, TToken> LeftRecursive<T, U, TToken>(
        //    Parser<T, TToken> nonRecursiveSubstitutions,
        //    Parser<T, TToken> recursionlessRemainder,
        //    Func<T, U, U> aggregator)
        //{
        //    var tail =
        //        Recursive<U, TToken>(self =>
        //            (from remainder in recursionlessRemainder
        //             from rest in self
        //             select aggregator(remainder, rest))
        //            .Or(recursionlessRemainder));

        //    return
        //        (from substitution in nonRecursiveSubstitutions
        //        from rest in tail
        //        select aggregator(substitution, rest))
        //        .Or(nonRecursiveSubstitutions);
        //}
    }
}
