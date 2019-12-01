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

        //public Parser<U, TToken> Bind<T, U, TToken>(this Parser<T, TToken> parser, Func<ParserResult<T, TToken>, Parser<U, TToken>> binder) =>
        //    lexemeString => binder(parser(lexemeString));

        public static NamedParser<T, TToken> Or<T, TToken>(this NamedParser<T, TToken> left, params NamedParser<T, TToken>[] right) =>
            WithName<T, TToken>(lexemeString =>
            {
                var errors = new ParserError<TToken>?[right.Length + 1];

                var leftResult = left.Parse(lexemeString);

                if (leftResult.IsSuccessful) return leftResult;

                errors[0] = leftResult.Error!.Value;

                for (int i = 0; i < right.Length; i++)
                {
                    var rightResult = right[i].Parse(lexemeString);

                    if (rightResult.IsSuccessful) return rightResult;

                    errors[i + 1] = rightResult.Error!.Value;
                }

                return ParserResult.Failure<T, TToken>(errors);
            }, left.Name + " | " + string.Join(" | ", right.Select(x => x.Name)));

        public static NamedParser<T, TToken> Or<T, TToken>(params NamedParser<T, TToken>[] parsers) =>
            WithName<T, TToken>(lexemeString =>
            {
                var errors = new ParserError<TToken>?[parsers.Length];

                for (int i = 0; i < parsers.Length; i++)
                {
                    var rightResult = parsers[i].Parse(lexemeString);

                    if (rightResult.IsSuccessful) return rightResult;

                    errors[i] = rightResult.Error!.Value;
                }

                return ParserResult.Failure<T, TToken>(errors);
            }, string.Join(" | ", parsers.Select(x => x.Name)));

        public static NamedParser<T[], TToken> Then<T, TToken>(this NamedParser<T, TToken> left, params NamedParser<T, TToken>[] right) =>
            WithName<T[], TToken>(_lexemeString =>
            {
                var lexemeString = _lexemeString;
                var elements = new T[right.Length + 1];

                var leftResult = left.Parse(lexemeString);

                if (!leftResult.IsSuccessful) return ParserResult.Failure<T[], TToken>(leftResult.Error!.Value);

                lexemeString = leftResult.RemainingLexemeString;
                elements[0] = leftResult.Result;

                for (int i = 0; i < right.Length; i++)
                {
                    var rightResult = right[i].Parse(lexemeString);

                    if (!rightResult.IsSuccessful) return ParserResult.Failure<T[], TToken>(rightResult.Error!.Value);

                    lexemeString = rightResult.RemainingLexemeString;
                    elements[i + 1] = rightResult.Result;
                }

                return ParserResult.Success(elements, lexemeString);
            }, "(" + left.Name + ") & (" + string.Join(") & (", right.Select(x => x.Name)) + ")");

        public static NamedParser<(T, U), TToken> And<T, U, TToken>(this NamedParser<T, TToken> left, NamedParser<U, TToken> right) =>
            WithName<(T, U), TToken>(_lexemeString =>
            {
                var lexemeString = _lexemeString;

                var leftResult = left.Parse(lexemeString);

                if (!leftResult.IsSuccessful) return ParserResult.Failure<(T, U), TToken>(leftResult.Error!.Value);

                lexemeString = leftResult.RemainingLexemeString;

                var rightResult = right.Parse(lexemeString);

                if (!rightResult.IsSuccessful) return ParserResult.Failure<(T, U), TToken>(rightResult.Error!.Value);

                lexemeString = rightResult.RemainingLexemeString;

                return ParserResult.Success((leftResult.Result, rightResult.Result), lexemeString);
            }, "(" + left.Name + ") & (" + right.Name + ")");

        public static NamedParser<T, TToken> Maybe<T, TToken>(this NamedParser<T, TToken> parser) =>
            WithName<T, TToken>(lexemeString =>
            {
                var result = parser.Parse(lexemeString);

                if (result.IsSuccessful) return result;

                return default;
            }, parser.Name + "?");

        public static NamedParser<List<T>, TToken> Many<T, TToken>(this NamedParser<T, TToken> parser, int least = 1, int most = int.MaxValue) =>
            WithName<List<T>, TToken>(_lexemeString =>
            {
                var lexemeString = _lexemeString;

                var elements = new List<T>(least);

                for (int i = 0; i < most; i++)
                {
                    var result = parser.Parse(lexemeString);

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
            }, parser.Name + "[" + least + ".." + most + "]");

        public static NamedParser<U, TToken> Many<T, U, TToken>(this NamedParser<T, TToken> parser, Func<T, U, U> aggregator, U start = default, int least = 1, int most = int.MaxValue) =>
            WithName<U, TToken>(_lexemeString =>
            {
                var lexemeString = _lexemeString;

                U aggregate = start;

                for (int i = 0; i < most; i++)
                {
                    var result = parser.Parse(lexemeString);

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
            }, parser.Name + "[" + least + ".." + most + "]");

        public static NamedParser<U, TToken> Transform<T, U, TToken>(this NamedParser<T, TToken> parser, Func<T, U> func) =>
            WithName<U, TToken>(lexemeString =>
            {
                var result = parser.Parse(lexemeString);

                if (result.IsSuccessful) return ParserResult.Success(func(result.Result), result.RemainingLexemeString);

                return ParserResult.Failure<U, TToken>(result.Error!.Value);
            }, "f(" + parser.Name + ")");

        public static NamedParser<U, TToken> Cast<T, U, TToken>(this NamedParser<T, TToken> parser)
            where T : U =>
            WithName<U, TToken>(lexemeString =>
            {
                var result = parser.Parse(lexemeString);

                if (result.IsSuccessful) return ParserResult.Success((U)result.Result, result.RemainingLexemeString);

                return ParserResult.Failure<U, TToken>(result.Error!.Value);
            }, "f(" + parser.Name + ")");

        public static NamedParser<U, TToken> As<T, U, TToken>(this NamedParser<T, TToken> parser, U val) =>
            WithName<U, TToken>(lexemeString =>
            {
                var result = parser.Parse(lexemeString);

                if (result.IsSuccessful) return ParserResult.Success(val, result.RemainingLexemeString);

                return ParserResult.Failure<U, TToken>(result.Error!.Value);
            }, "f(" + parser.Name + ")");

        public static NamedParser<U, TToken> As<T, U, TToken>(this NamedParser<T, TToken> parser, Func<U> val) =>
            WithName<U, TToken>(lexemeString =>
            {
                var result = parser.Parse(lexemeString);

                if (result.IsSuccessful) return ParserResult.Success(val(), result.RemainingLexemeString);

                return ParserResult.Failure<U, TToken>(result.Error!.Value);
            }, "f(" + parser.Name + ")");

        public static NamedParser<U, TToken> Null<T, U, TToken>(this NamedParser<T, TToken> parser) =>
            WithName<U, TToken>(lexemeString =>
            {
                var result = parser.Parse(lexemeString);

                if (result.IsSuccessful) return ParserResult.Success(default(U)!, result.RemainingLexemeString);

                return ParserResult.Failure<U, TToken>(result.Error!.Value);
            }, "null(" + parser.Name + ")");

        public static NamedParser<T, TToken> WithName<T, TToken>(this Parser<T, TToken> parser, string name)

             =>
            new NamedParser<T, TToken>(name, parser);

        public static NamedParser<T, TToken> WithName<T, TToken>(this NamedParser<T, TToken> parser, string name) =>
            new NamedParser<T, TToken>(name, parser.Parser);

        public static NamedParser<Lexeme<TToken>, TToken> Match<TToken>(this TToken token) =>
            WithName<Lexeme<TToken>, TToken>(lexemeString =>
            {
                var lexeme = lexemeString[0];

                if (EqualityComparer<TToken>.Default.Equals(lexeme.Token, token))
                {
                    return ParserResult.Success(lexeme, lexemeString.Advance(1));
                }

                return ParserResult.Failure<Lexeme<TToken>, TToken>(lexemeString, token, lexeme);
            }, token?.ToString() ?? "NULL");

        public static Parser<T, TToken> Ref<T, TToken>(Func<Parser<T, TToken>> getter) =>
            lexemeString => getter()(lexemeString);

        public static Parser<T, TToken> Ref<T, TToken>(Func<NamedParser<T, TToken>> getter) =>
            lexemeString => getter().Parse(lexemeString);
    }
}