using System.Collections.Generic;
using TempleLang.Lexer;
using TempleLang.Lexer.Abstractions;

namespace TempleLang.Parser.Abstractions
{
    public readonly struct ParserError<TToken>
    {
        public readonly LexemeString<TToken> LexemeString;
        public readonly TToken Expected;
        public readonly Lexeme<TToken> Actual;

        public ParserError(LexemeString<TToken> lexemeString, TToken expected, Lexeme<TToken> actual)
        {
            LexemeString = lexemeString;
            Expected = expected;
            Actual = actual;
        }

        public readonly override bool Equals(object? obj) => obj is ParserError<TToken> error
            && EqualityComparer<LexemeString<TToken>>.Default.Equals(LexemeString, error.LexemeString)
            && EqualityComparer<TToken>.Default.Equals(Expected, error.Expected)
            && EqualityComparer<Lexeme<TToken>>.Default.Equals(Actual, error.Actual);

        public readonly override int GetHashCode()
        {
            var hashCode = 893187206;
            hashCode = (hashCode * -1521134295) + EqualityComparer<LexemeString<TToken>>.Default.GetHashCode(LexemeString);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TToken>.Default.GetHashCode(Expected);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Lexeme<TToken>>.Default.GetHashCode(Actual);
            return hashCode;
        }

        public static bool operator ==(ParserError<TToken> left, ParserError<TToken> right) => left.Equals(right);

        public static bool operator !=(ParserError<TToken> left, ParserError<TToken> right) => !(left == right);
    }
}