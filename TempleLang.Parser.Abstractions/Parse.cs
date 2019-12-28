namespace TempleLang.Parser.Abstractions
{
    using Lexer;
    using System;
    using System.Collections.Generic;

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
            input => ParserResult.Success(value, input);

        public static Parser<object?, TToken> Epsilon<TToken>() => Value<object?, TToken>(null);

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

        public static Parser<T, TToken> BinaryOperatorRightToLeft<T, U, TToken>(Parser<T, TToken> parser, Parser<U, TToken> @operator, Func<T, U, T, T> factory) =>
            Recursive<T, TToken>(self =>
                (from lhs in parser
                 from op in @operator
                 from rhs in self
                 select factory(lhs, op, rhs))
                .Or(parser));

        public static Parser<T, TToken> BinaryOperatorLeftToRight<T, U, TToken>(Parser<T, TToken> parser, Parser<U, TToken> @operator, Func<T, U, T, T> factory) =>
            BinaryOperatorLeftToRight(parser, parser, @operator, factory);

        public static Parser<T, TToken> BinaryOperatorLeftToRight<T, U, V, TToken>(Parser<T, TToken> lhsParser, Parser<V, TToken> rhsParser, Parser<U, TToken> @operator, Func<T, U, V, T> factory) =>
            input =>
            {
                var result = lhsParser(input);

                if (!result.IsSuccessful) return result;

                T accumulator = result.Result;
                var remainder = result.RemainingLexemeString;

                while (true)
                {
                    var op = @operator(remainder);

                    if (!op.IsSuccessful) return ParserResult.Success(accumulator, remainder);

                    var res = rhsParser(op.RemainingLexemeString);

                    if (!op.IsSuccessful) return ParserResult.Success(accumulator, remainder);

                    remainder = res.RemainingLexemeString;

                    accumulator = factory(accumulator, op.Result, res.Result);
                }
            };
    }
}