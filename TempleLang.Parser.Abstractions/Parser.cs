namespace TempleLang.Parser.Abstractions
{
    using Lexer;
    using System;
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    public delegate IParserResult<T, TToken> Parser<out T, TToken>(LexemeString<TToken> lexemeString);

    public static class Parse
    {
        public static Parser<Lexeme<TToken>, TToken> Token<TToken>(TToken token) =>
            input =>
            {
                if (input.Length == 0)
                {
                    return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected {token}", input);
                }

                if (EqualityComparer<TToken>.Default.Equals(input[0].Token, token))
                {
                    return ParserResult.Failure<Lexeme<TToken>, TToken>($"Expected {token}", input);
                }

                return ParserResult.Success(input[0], input.Advance(1));
            };
    }
}
