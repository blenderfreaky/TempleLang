namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;

    public readonly struct ParserResult<T>
    {
        public readonly bool IsSuccessful;
        public readonly ParserError? Error;
        public readonly T Result;

        private ParserResult(bool isSuccessful, ParserError? error, T result)
        {
            IsSuccessful = isSuccessful;
            Error = error;
            Result = result;
        }

        public override bool Equals(object? obj) => obj is ParserResult<T> result && IsSuccessful == result.IsSuccessful && EqualityComparer<ParserError?>.Default.Equals(Error, result.Error) && EqualityComparer<T>.Default.Equals(Result, result.Result);

        public override int GetHashCode()
        {
            var hashCode = 176370183;
            hashCode = hashCode * -1521134295 + IsSuccessful.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ParserError?>.Default.GetHashCode(Error);
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(Result);
            return hashCode;
        }

        public static bool operator ==(ParserResult<T> left, ParserResult<T> right) => left.Equals(right);

        public static bool operator !=(ParserResult<T> left, ParserResult<T> right) => !(left == right);

        public static ParserResult<T> Success(T result) => new ParserResult<T>(true, null, result);
        public static ParserResult<T> Failure(ParserError error) => new ParserResult<T>(true, error, default!);
    }
}
