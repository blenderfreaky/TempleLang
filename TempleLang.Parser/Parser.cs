namespace TempleLang.Parser
{
    using System.Collections.Generic;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;
    using Lexeme = Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>;
    using LexemeString = Lexer.Abstractions.LexemeString<Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>, Lexer.Token, Lexer.SourceFile>;
    using static Abstractions.ParserExtensions;
    using System.Net.Http.Headers;
    using System.Linq;
    using System.Collections;
    using System;

    public static class Parser
    {
        public static readonly Dictionary<Token, NamedParser<Lexeme, Lexeme, Token, SourceFile>> Tokens =
            TokenParsers<Lexeme, Token, SourceFile>();

        public static NamedParser<Lexeme, Lexeme, Token, SourceFile> TokensWhere(Predicate<Token> predicate) =>
            Or(Tokens
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToArray());

        public static NamedParser<T, Lexeme, Token, SourceFile> TransformToken<T>(Token token, Func<Lexeme, T> func) =>
            Tokens[token].Transform(func);
    }
}
