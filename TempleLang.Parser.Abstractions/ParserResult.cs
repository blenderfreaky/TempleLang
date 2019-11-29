namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer;
    using TempleLang.Lexer.Abstractions;

    public static class ParserResult
    {
        public static ParserResult<T, TToken> Success<T, TToken>(T result, LexemeString<TToken> remainingLexemeString) =>
            new ParserResult<T, TToken>(true, null, result, remainingLexemeString);

        public static ParserResult<T, TToken> Failure<T, TToken>(ParserError<TToken> error) =>
            new ParserResult<T, TToken>(false, error, default!, default!);

        public static ParserResult<T, TToken> Failure<T, TToken>(IEnumerable<ParserError<TToken>> errors) =>
            Failure<T, TToken>(errors.Last() /* TODO */);

        public static ParserResult<T, TToken> Failure<T, TToken>(IEnumerable<ParserError<TToken>?> errors) =>
            Failure<T, TToken>(errors.Where(x => x != null).Select(x => x!.Value));

        public static ParserResult<T, TToken> Failure<T, TToken>(LexemeString<TToken> lexemeString, TToken expected, Lexeme<TToken> actual) =>
            Failure<T, TToken>(new ParserError<TToken>(lexemeString, expected, actual));
    }
}