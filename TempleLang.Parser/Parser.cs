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

        public static Parser<Lexeme, Token> TokensWhere(Predicate<Token> predicate) =>
            from t in Tokens
            where predicate(t.Key)
            select t.Value;

        public static Parser<T, Token> TransformToken<T>(Token token, Func<Lexeme, T> func) =>
            from t in Tokens[token] select func(t.Result);
    }
}