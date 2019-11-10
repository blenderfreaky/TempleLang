namespace TempleLang.Parser.Abstractions
{
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    public readonly struct NamedParser<T, TLexeme, TToken, TSourceFile>
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public readonly string Name;
        public readonly Parser<T, TLexeme, TToken, TSourceFile> Parser;

        public ParserResult<T> Parse(LexemeString<TLexeme, TToken, TSourceFile> lexemeString) => Parser(lexemeString);

        /// </inheritdoc>
        public override bool Equals(object? obj) => obj is NamedParser<T, TLexeme, TToken, TSourceFile> parser && Name == parser.Name && EqualityComparer<Parser<T, TLexeme, TToken, TSourceFile>>.Default.Equals(Parser, parser.Parser);

        /// </inheritdoc>
        public override int GetHashCode()
        {
            var hashCode = -713333808;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Parser<T, TLexeme, TToken, TSourceFile>>.Default.GetHashCode(Parser);
            return hashCode;
        }

        /// </inheritdoc>
        public static bool operator ==(NamedParser<T, TLexeme, TToken, TSourceFile> left, NamedParser<T, TLexeme, TToken, TSourceFile> right) => left.Equals(right);

        /// </inheritdoc>
        public static bool operator !=(NamedParser<T, TLexeme, TToken, TSourceFile> left, NamedParser<T, TLexeme, TToken, TSourceFile> right) => !(left == right);
    }

    public delegate ParserResult<T> Parser<T, TLexeme, TToken, TSourceFile>(LexemeString<TLexeme, TToken, TSourceFile> lexemeString)
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile;

    public static class Parser
    {
        public static Parser<T, TLexeme, TToken, TSourceFile> Or<T, TLexeme, TToken, TSourceFile>(this Parser<T, TLexeme, TToken, TSourceFile> left, Parser<T, TLexeme, TToken, TSourceFile> right)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            lexemeString =>
            {
                var leftResult = left(lexemeString);

                if (leftResult.IsSuccessful) return leftResult;

                var rightResult = right(lexemeString);

                if (rightResult.IsSuccessful) return rightResult;

                return ParserResult<T>.Failure(ParserError.AggregateError(leftResult.Error!.Value, rightResult.Error!.Value));
            };

        public static Parser<T, TLexeme, TToken, TSourceFile> Then<T, TLexeme, TToken, TSourceFile>(Parser<T, TLexeme, TToken, TSourceFile> left, Parser<T, TLexeme, TToken, TSourceFile> right)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile =>
            lexemeString =>
            {
                var leftResult = left(lexemeString);

                if (!leftResult.IsSuccessful) return ParserResult<T>.Failure(leftResult.Error!.Value);

                var rightResult = right(lexemeString);

                if (!rightResult.IsSuccessful) return ParserResult<T>.Failure(rightResult.Error!.Value);

                return ParserResult<T>.Failure(ParserError.AggregateError(leftResult.Error!.Value, rightResult.Error!.Value));
            };
    }
}
