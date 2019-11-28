namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer.Abstractions;

    public static class ParserResult
    {
        public static ParserResult<T, TLexeme, TToken, TSourceFile> Success<T, TLexeme, TToken, TSourceFile>(T result, LexemeString<TLexeme, TToken, TSourceFile> remainingLexemeString)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            new ParserResult<T, TLexeme, TToken, TSourceFile>(true, null, result, remainingLexemeString);

        public static ParserResult<T, TLexeme, TToken, TSourceFile> Failure<T, TLexeme, TToken, TSourceFile>(ParserError<TLexeme, TToken, TSourceFile> error)

            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            new ParserResult<T, TLexeme, TToken, TSourceFile>(false, error, default!, default!);

        public static ParserResult<T, TLexeme, TToken, TSourceFile> Failure<T, TLexeme, TToken, TSourceFile>(IEnumerable<ParserError<TLexeme, TToken, TSourceFile>> errors)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            Failure<T, TLexeme, TToken, TSourceFile>(errors.Last() /* TODO */);

        public static ParserResult<T, TLexeme, TToken, TSourceFile> Failure<T, TLexeme, TToken, TSourceFile>(IEnumerable<ParserError<TLexeme, TToken, TSourceFile>?> errors)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            Failure<T, TLexeme, TToken, TSourceFile>(errors.Where(x => x != null).Select(x => x!.Value));

        public static ParserResult<T, TLexeme, TToken, TSourceFile> Failure<T, TLexeme, TToken, TSourceFile>(LexemeString<TLexeme, TToken, TSourceFile> lexemeString, TToken expected, TLexeme actual)

            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            Failure<T, TLexeme, TToken, TSourceFile>(new ParserError<TLexeme, TToken, TSourceFile>(lexemeString, expected, actual));
    }
}
