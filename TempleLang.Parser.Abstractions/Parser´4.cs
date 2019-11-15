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

        public ParserResult<T, TLexeme, TToken, TSourceFile> Parse(LexemeString<TLexeme, TToken, TSourceFile> lexemeString) => Parser(lexemeString);

        public NamedParser(string name, Parser<T, TLexeme, TToken, TSourceFile> parser) : this()
        {
            Name = name;
            Parser = parser;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is NamedParser<T, TLexeme, TToken, TSourceFile> parser && Name == parser.Name && EqualityComparer<Parser<T, TLexeme, TToken, TSourceFile>>.Default.Equals(Parser, parser.Parser);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = -713333808;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Parser<T, TLexeme, TToken, TSourceFile>>.Default.GetHashCode(Parser);
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(NamedParser<T, TLexeme, TToken, TSourceFile> left, NamedParser<T, TLexeme, TToken, TSourceFile> right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(NamedParser<T, TLexeme, TToken, TSourceFile> left, NamedParser<T, TLexeme, TToken, TSourceFile> right) => !(left == right);

        public static implicit operator Parser<T, TLexeme, TToken, TSourceFile>(NamedParser<T, TLexeme, TToken, TSourceFile> parser) => parser.Parser; 
    }

    public delegate ParserResult<T, TLexeme, TToken, TSourceFile> Parser<T, TLexeme, TToken, TSourceFile>(LexemeString<TLexeme, TToken, TSourceFile> lexemeString)
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile;
}
