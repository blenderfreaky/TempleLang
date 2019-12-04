﻿namespace TempleLang.Parser.Abstractions
{
    using Lexer.Abstractions;
    using System;
    using System.Collections.Generic;

    public interface IParserResult<out T, TToken>
    {
        public bool IsSuccessful { get; }

        public string? ErrorMessage { get; }

        public T Result { get; }

        public LexemeString<TToken> RemainingLexemeString { get; }
    }

    public struct ParserResult<T, TToken> : IParserResult<T, TToken>
    {
        public bool IsSuccessful { get; }

        public string? ErrorMessage { get; }

        public T Result { get; }

        public LexemeString<TToken> RemainingLexemeString { get; }

        private ParserResult(bool isSuccessful, string? errorMessage, T result, LexemeString<TToken> remainingLexemeString)
        {
            IsSuccessful = isSuccessful;
            ErrorMessage = errorMessage;
            Result = result;
            RemainingLexemeString = remainingLexemeString;
        }

        public ParserResult(T result, LexemeString<TToken> remainingLexemeString)
            : this(true, null, result, remainingLexemeString)
        {
        }

        public ParserResult(string errorMessage, LexemeString<TToken> remainingLexemeString)
            : this(false, errorMessage, default!, remainingLexemeString)
        {
        }

        public override bool Equals(object obj) => obj is ParserResult<T, TToken> result
            && IsSuccessful == result.IsSuccessful
            && ErrorMessage == result.ErrorMessage
            && EqualityComparer<T>.Default.Equals(Result, result.Result)
            && RemainingLexemeString == result.RemainingLexemeString;

        public override int GetHashCode()
        {
            var hashCode = 176370183;
            hashCode = (hashCode * -1521134295) + IsSuccessful.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string?>.Default.GetHashCode(ErrorMessage);
            hashCode = (hashCode * -1521134295) + EqualityComparer<T>.Default.GetHashCode(Result);
            hashCode = (hashCode * -1521134295) + EqualityComparer<LexemeString<TToken>>.Default.GetHashCode(RemainingLexemeString);
            return hashCode;
        }

        public static bool operator ==(ParserResult<T, TToken> left, ParserResult<T, TToken> right) => left.Equals(right);

        public static bool operator !=(ParserResult<T, TToken> left, ParserResult<T, TToken> right) => !(left == right);
    }

    public static class ParserResult
    {
        public static ParserResult<T, TToken> Success<T, TToken>(T result, LexemeString<TToken> remainingLexemeString) =>
            new ParserResult<T, TToken>(result, remainingLexemeString);

        public static ParserResult<T, TToken> Failure<T, TToken>(string errorMessage, LexemeString<TToken> remainingLexemeString) =>
            new ParserResult<T, TToken>(errorMessage, remainingLexemeString);

        public static ParserResult<U, TToken> Failure<T, U, TToken>(IParserResult<T, TToken> error) =>
            error.IsSuccessful
            ? throw new ArgumentException(nameof(error))
            : new ParserResult<U, TToken>(error.ErrorMessage!, error.RemainingLexemeString);
    }
}