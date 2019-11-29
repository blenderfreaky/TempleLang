namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    public readonly struct ParserResult<T, TToken>
    {
        public readonly bool IsSuccessful;
        public readonly ParserError<TToken>? Error;
        public readonly T Result;
        public readonly LexemeString<TToken> RemainingLexemeString;

        internal ParserResult(bool isSuccessful, ParserError<TToken>? error, T result, LexemeString<TToken> remainingLexemeString)
        {
            IsSuccessful = isSuccessful;
            Error = error;
            Result = result;
            RemainingLexemeString = remainingLexemeString;
        }

        public override bool Equals(object? obj) => obj is ParserResult<T, TToken> result
            && IsSuccessful == result.IsSuccessful
            && EqualityComparer<ParserError<TToken>?>.Default.Equals(Error, result.Error)
            && EqualityComparer<T>.Default.Equals(Result, result.Result);

        public override int GetHashCode()
        {
            var hashCode = 176370183;
            hashCode = (hashCode * -1521134295) + IsSuccessful.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<ParserError<TToken>?>.Default.GetHashCode(Error);
            hashCode = (hashCode * -1521134295) + EqualityComparer<T>.Default.GetHashCode(Result);
            return hashCode;
        }

        public static bool operator ==(ParserResult<T, TToken> left, ParserResult<T, TToken> right) => left.Equals(right);

        public static bool operator !=(ParserResult<T, TToken> left, ParserResult<T, TToken> right) => !(left == right);

        public override string ToString() => IsSuccessful ? "Success(" + Result + ")" : "Error(" + Error + ")";
    }
}