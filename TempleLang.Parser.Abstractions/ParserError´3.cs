using System.Collections.Generic;
using TempleLang.Lexer.Abstractions;

namespace TempleLang.Parser.Abstractions
{
    public readonly struct ParserError<TLexeme, TToken, TSourceFile>
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public readonly LexemeString<TLexeme, TToken, TSourceFile> LexemeString;
        public readonly TToken Expected;
        public readonly TLexeme Actual;

        public ParserError(LexemeString<TLexeme, TToken, TSourceFile> lexemeString, TToken expected, TLexeme actual)
        {
            LexemeString = lexemeString;
            Expected = expected;
            Actual = actual;
        }

        public readonly override bool Equals(object? obj) => obj is ParserError<TLexeme, TToken, TSourceFile> error && EqualityComparer<LexemeString<TLexeme, TToken, TSourceFile>>.Default.Equals(LexemeString, error.LexemeString) && EqualityComparer<TToken>.Default.Equals(Expected, error.Expected) && EqualityComparer<TLexeme>.Default.Equals(Actual, error.Actual);

        public readonly override int GetHashCode()
        {
            var hashCode = 893187206;
            hashCode = (hashCode * -1521134295) + EqualityComparer<LexemeString<TLexeme, TToken, TSourceFile>>.Default.GetHashCode(LexemeString);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TToken>.Default.GetHashCode(Expected);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TLexeme>.Default.GetHashCode(Actual);
            return hashCode;
        }

        public static bool operator ==(ParserError<TLexeme, TToken, TSourceFile> left, ParserError<TLexeme, TToken, TSourceFile> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ParserError<TLexeme, TToken, TSourceFile> left, ParserError<TLexeme, TToken, TSourceFile> right)
        {
            return !(left == right);
        }
    }
}