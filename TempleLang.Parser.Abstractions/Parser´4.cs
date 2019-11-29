namespace TempleLang.Parser.Abstractions
{
    using System;
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    public struct NamedParser<T, TToken>
    {
        public string Name { get; }
        public Parser<T, TToken> Parser { get; private set; }

        public ParserResult<T, TToken> Parse(LexemeString<TToken> lexemeString) =>
            Parser(lexemeString);

        public static NamedParser<T, TToken> Empty =>
            new NamedParser<T, TToken>(
                "Empty",
                s => ParserResult.Success(default(T)!, s));

        public NamedParser(string name, Parser<T, TToken> parser) : this()
        {
            Name = name;
            Parser = parser;
        }

        public NamedParser(string name) : this()
        {
            Name = name;
            Parser = s => ParserResult.Success(default(T)!, s);
        }

        public void OverrideParser(Parser<T, TToken> newParser) => Parser = newParser;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is NamedParser<T, TToken> parser
            && Name == parser.Name
            && EqualityComparer<Parser<T, TToken>>.Default.Equals(Parser, parser.Parser);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = -713333808;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Parser<T, TToken>>.Default.GetHashCode(Parser);
            return hashCode;
        }

        public override string ToString() => Name;

        /// <inheritdoc/>
        public static bool operator ==(NamedParser<T, TToken> left, NamedParser<T, TToken> right) => left.Equals(right);

        /// <inheritdoc/>
        public static bool operator !=(NamedParser<T, TToken> left, NamedParser<T, TToken> right) => !(left == right);

        public static implicit operator Parser<T, TToken>(NamedParser<T, TToken> parser) => parser.Parser;
    }

    public delegate ParserResult<T, TToken> Parser<T, TToken>(LexemeString<TToken> lexemeString);
}