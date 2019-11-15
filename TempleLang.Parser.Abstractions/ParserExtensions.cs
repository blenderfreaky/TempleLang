namespace TempleLang.Parser.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer.Abstractions;

    public static class ParserExtensions
    {
        public static NamedParser<T, TLexeme, TToken, TSourceFile> Or<T, TLexeme, TToken, TSourceFile>(this NamedParser<T, TLexeme, TToken, TSourceFile> left, params NamedParser<T, TLexeme, TToken, TSourceFile>[] right)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            WithName<T, TLexeme, TToken, TSourceFile>(lexemeString =>
            {
                var errors = new ParserError<TLexeme, TToken, TSourceFile>?[right.Length + 1];

                var leftResult = left.Parse(lexemeString);

                if (leftResult.IsSuccessful) return leftResult;

                errors[0] = leftResult.Error!.Value;

                for (int i = 0; i < right.Length; i++)
                {
                    var rightResult = right[i].Parse(lexemeString);

                    if (rightResult.IsSuccessful) return rightResult;

                    errors[i + 1] = rightResult.Error!.Value;
                }

                return ParserResult.Failure<T, TLexeme, TToken, TSourceFile>(errors);
            }, left.Name + " | " + string.Join(" | ", right.Select(x => x.Name)));

        public static NamedParser<T[], TLexeme, TToken, TSourceFile> Then<T, TLexeme, TToken, TSourceFile>(this NamedParser<T, TLexeme, TToken, TSourceFile> left, params NamedParser<T, TLexeme, TToken, TSourceFile>[] right)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            WithName<T[], TLexeme, TToken, TSourceFile>(_lexemeString =>
            {
                var lexemeString = _lexemeString;
                var elements = new T[right.Length + 1];

                var leftResult = left.Parse(lexemeString);

                if (!leftResult.IsSuccessful) return ParserResult.Failure<T[], TLexeme, TToken, TSourceFile>(leftResult.Error!.Value);

                lexemeString = leftResult.RemainingLexemeString;
                elements[0] = leftResult.Result;

                for (int i = 0; i < right.Length; i++)
                {
                    var rightResult = right[i].Parse(lexemeString);

                    if (!rightResult.IsSuccessful) return ParserResult.Failure<T[], TLexeme, TToken, TSourceFile>(rightResult.Error!.Value);

                    lexemeString = rightResult.RemainingLexemeString;
                    elements[i + 1] = rightResult.Result;
                }

                return ParserResult.Success(elements, lexemeString);
            }, "(" + left.Name + ") & (" + string.Join(") & (", right.Select(x => x.Name)) + ")");

        public static NamedParser<(T, U), TLexeme, TToken, TSourceFile> Then<T, U, TLexeme, TToken, TSourceFile>(this NamedParser<T, TLexeme, TToken, TSourceFile> left, NamedParser<U, TLexeme, TToken, TSourceFile> right)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            WithName<(T, U), TLexeme, TToken, TSourceFile>(_lexemeString =>
            {
                var lexemeString = _lexemeString;

                var leftResult = left.Parse(lexemeString);

                if (!leftResult.IsSuccessful) return ParserResult.Failure<(T, U), TLexeme, TToken, TSourceFile>(leftResult.Error!.Value);

                lexemeString = leftResult.RemainingLexemeString;

                var rightResult = right.Parse(lexemeString);

                if (!rightResult.IsSuccessful) return ParserResult.Failure< (T, U), TLexeme, TToken, TSourceFile>(rightResult.Error!.Value);

                lexemeString = rightResult.RemainingLexemeString;

                return ParserResult.Success((leftResult.Result, rightResult.Result), lexemeString);
            }, "(" + left.Name + ") & (" + right.Name + ")");

        public static NamedParser<T, TLexeme, TToken, TSourceFile> Maybe<T, TLexeme, TToken, TSourceFile>(this NamedParser<T, TLexeme, TToken, TSourceFile> parser)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            WithName<T, TLexeme, TToken, TSourceFile>(lexemeString =>
            {
                var result = parser.Parse(lexemeString);

                if (result.IsSuccessful) return result;

                return default;
            }, parser.Name + "?");

        public static NamedParser<List<T>, TLexeme, TToken, TSourceFile> Many<T, TLexeme, TToken, TSourceFile>(this NamedParser<T, TLexeme, TToken, TSourceFile> parser, int least = 1, int most = int.MaxValue)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            WithName<List<T>, TLexeme, TToken, TSourceFile>(_lexemeString =>
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
                        if (i < least) return ParserResult.Failure<List<T>, TLexeme, TToken, TSourceFile>(result.Error!.Value);

                        return ParserResult.Success(elements, lexemeString);
                    }
                }

                return ParserResult.Success(elements, lexemeString);
            }, parser.Name + "[" + least + ".." + most + "]");

        public static NamedParser<U, TLexeme, TToken, TSourceFile> Transform<T, U, TLexeme, TToken, TSourceFile>(this NamedParser<T, TLexeme, TToken, TSourceFile> parser, Func<T, U> func)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            WithName<U, TLexeme, TToken, TSourceFile>(lexemeString =>
            {
                var result = parser.Parse(lexemeString);

                if (result.IsSuccessful) return ParserResult.Success(func(result.Result), result.RemainingLexemeString);

                return ParserResult.Failure<U, TLexeme, TToken, TSourceFile>(result.Error!.Value);
            }, "f(" + parser.Name + ")");

        public static NamedParser<T, TLexeme, TToken, TSourceFile> WithName<T, TLexeme, TToken, TSourceFile>(this Parser<T, TLexeme, TToken, TSourceFile> parser, string name)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            new NamedParser<T, TLexeme, TToken, TSourceFile>(name, parser);

        public static NamedParser<T, TLexeme, TToken, TSourceFile> WithName<T, TLexeme, TToken, TSourceFile>(this NamedParser<T, TLexeme, TToken, TSourceFile> parser, string name)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            new NamedParser<T, TLexeme, TToken, TSourceFile>(name, parser.Parser);

        public static NamedParser<TLexeme, TLexeme, TToken, TSourceFile> Match<TLexeme, TToken, TSourceFile>(this TToken token)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            WithName<TLexeme, TLexeme, TToken, TSourceFile>(lexemeString =>
            {
                var lexeme = lexemeString[0];

                if (EqualityComparer<TToken>.Default.Equals(lexeme.Token, token))
                {
                    return ParserResult.Success(lexeme, lexemeString.Advance(1));
                }

                return ParserResult.Failure<TLexeme, TLexeme, TToken, TSourceFile>(lexemeString, token, lexeme);
            }, token?.ToString() ?? "NULL");
    }
}
