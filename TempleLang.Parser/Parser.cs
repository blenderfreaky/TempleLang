namespace TempleLang.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;
    using static Abstractions.ParserExtensions;
    using Lexeme = Lexer.Lexeme<Lexer.Token>;

    public static class Parser
    {
        public static readonly Dictionary<Token, NamedParser<Lexeme, Token>> Tokens =
            TokenParsers<Token>();

        public static NamedParser<Lexeme, Token> TokensWhere(Predicate<Token> predicate) =>
            Or(Tokens
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToArray());

        public static NamedParser<T, Token> TransformToken<T>(Token token, Func<Lexeme, T> func) =>
            Tokens[token].Transform(func);
    }
}