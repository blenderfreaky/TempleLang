namespace TempleLang.Parser.Abstractions
{
    using Lexer;
    using System;
    using System.Collections.Generic;

    public static partial class Parse
    {
        public static Parser<Lexeme<TToken>, TToken> Token<TToken>(TToken token) =>
            input =>
            {
                if (input.Length == 0)
                {
                    return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected {token}");
                }

                if (!EqualityComparer<TToken>.Default.Equals(input[0].Token, token))
                {
                    return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected {token}");
                }

                return ParserResult.Success(input[0], input.Advance(1));
            };

        public static Parser<Lexeme<TToken>, TToken> Token<TToken>(params TToken[] tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            return input =>
            {
                if (input.Length == 0)
                {
                    return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected [{string.Join(", ", tokens)}]");
                }

                foreach (var token in tokens)
                {
                    if (!EqualityComparer<TToken>.Default.Equals(input[0].Token, token))
                    {
                        continue;
                    }

                    return ParserResult.Success(input[0], input.Advance(1));
                }

                return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected [{string.Join(", ", tokens)}]");
            };
        }

        public static Parser<T, TToken> Ref<T, TToken>(Func<Parser<T, TToken>> parser)
        {
            if (parser is null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            return input => parser()(input);
        }

        public static Parser<T, TToken> Value<T, TToken>(T value = default) =>
            input => ParserResult.Success(value, input);

        public static Parser<object?, TToken> Epsilon<TToken>() => Value<object?, TToken>(null);

        public static Parser<T, TToken> Epsilon<T, TToken>() => Value<T, TToken>(default!);

        private class RefContainer<T>
            where T : class
        {
            public T? Value { get; set; }

            public RefContainer() => Value = null;

            public RefContainer(T? value) => Value = value;
        }

        public static Parser<T, TToken> Recursive<T, TToken>(Func<Parser<T, TToken>, Parser<T, TToken>> creator)
        {
            if (creator is null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            var refContainer = new RefContainer<Parser<T, TToken>>();

            var result = creator(Ref(() => refContainer.Value!));

            refContainer.Value = result;

            return result;
        }

        public static Parser<T, TToken> BinaryOperatorRightToLeft<T, U, TToken>(Parser<T, TToken> parser, Parser<U, TToken> @operator, Func<T, U, T, T> factory)
        {
            if (parser is null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            if (@operator is null)
            {
                throw new ArgumentNullException(nameof(@operator));
            }

            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return Recursive<T, TToken>(self =>
                (from lhs in parser
                 from op in @operator
                 from rhs in self
                 select factory(lhs, op, rhs))
                .Or(parser));
        }

        public static Parser<T, TToken> BinaryOperatorLeftToRight<T, U, TToken>(Parser<T, TToken> parser, Parser<U, TToken> @operator, Func<T, U, T, T> factory)
        {
            if (parser is null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            if (@operator is null)
            {
                throw new ArgumentNullException(nameof(@operator));
            }

            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return BinaryOperatorLeftToRight(parser, parser, @operator, factory);
        }

        public static Parser<T, TToken> BinaryOperatorLeftToRight<T, U, V, TToken>(Parser<T, TToken> lhsParser, Parser<V, TToken> rhsParser, Parser<U, TToken> @operator, Func<T, U, V, T> factory)
        {
            if (lhsParser is null)
            {
                throw new ArgumentNullException(nameof(lhsParser));
            }

            if (rhsParser is null)
            {
                throw new ArgumentNullException(nameof(rhsParser));
            }

            if (@operator is null)
            {
                throw new ArgumentNullException(nameof(@operator));
            }

            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return input =>
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

        public static Parser<T, TToken> Lookahead<T, TToken>(Parser<T, TToken> parser)
        {
            if (parser is null)
            {
                throw new ArgumentNullException(nameof(parser));
            }

            return input =>
            {
                var result = parser(input);

                return result.IsSuccessful
                ? ParserResult.Success(result.Result, input)
                : ParserResult.Failure<T,TToken>(result.ErrorMessage!);
            };
        }
    }
}