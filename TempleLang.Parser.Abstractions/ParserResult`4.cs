namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    public readonly struct ParserResult<T, TLexeme, TToken, TSourceFile>
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public readonly bool IsSuccessful;
        public readonly ParserError<TLexeme, TToken, TSourceFile>? Error;
        public readonly T Result;
        public readonly LexemeString<TLexeme, TToken, TSourceFile> RemainingLexemeString;

        internal ParserResult(bool isSuccessful, ParserError<TLexeme, TToken, TSourceFile>? error, T result, LexemeString<TLexeme, TToken, TSourceFile> remainingLexemeString)
        {
            IsSuccessful = isSuccessful;
            Error = error;
            Result = result;
            RemainingLexemeString = remainingLexemeString;
        }

        public override bool Equals(object? obj) => obj is ParserResult<T, TLexeme, TToken, TSourceFile> result && IsSuccessful == result.IsSuccessful && EqualityComparer<ParserError<TLexeme, TToken, TSourceFile>?>.Default.Equals(Error, result.Error) && EqualityComparer<T>.Default.Equals(Result, result.Result);

        public override int GetHashCode()
        {
            var hashCode = 176370183;
            hashCode = (hashCode * -1521134295) + IsSuccessful.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<ParserError<TLexeme, TToken, TSourceFile>?>.Default.GetHashCode(Error);
            hashCode = (hashCode * -1521134295) + EqualityComparer<T>.Default.GetHashCode(Result);
            return hashCode;
        }

        public static bool operator ==(ParserResult<T, TLexeme, TToken, TSourceFile> left, ParserResult<T, TLexeme, TToken, TSourceFile> right) => left.Equals(right);

        public static bool operator !=(ParserResult<T, TLexeme, TToken, TSourceFile> left, ParserResult<T, TLexeme, TToken, TSourceFile> right) => !(left == right);

        public override string ToString() => IsSuccessful ? "Success(" + Result + ")" : "Error(" + Error + ")";
    }
}
