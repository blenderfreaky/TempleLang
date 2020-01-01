namespace TempleLang.Parser.Abstractions
{
    using Lexer.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public static class ParserResult
    {
        [DebuggerStepThrough]
        public static ParserResult<T, TToken> Success<T, TToken>(T result, LexemeString<TToken> remainingLexemeString) =>
            new ParserResult<T, TToken>(result, remainingLexemeString);

        [DebuggerStepThrough]
        public static ParserResult<T, TToken> Failure<T, TToken>(string errorMessage) =>
            new ParserResult<T, TToken>(errorMessage, default);

        [DebuggerStepThrough]
        public static ParserResult<U, TToken> Failure<T, U, TToken>(IParserResult<T, TToken> error) =>
            error.IsSuccessful
            ? throw new ArgumentException(nameof(error))
            : new ParserResult<U, TToken>(error.ErrorMessage!, default);

        [DebuggerStepThrough]
        public static IParserResult<T, TToken> Failure<T, TToken>(params IParserResult<T, TToken>[] errors) =>
            new ParserAggregateError<T, TToken>(errors);
    }
}