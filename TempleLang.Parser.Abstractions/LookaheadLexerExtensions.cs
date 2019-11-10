namespace TempleLang.Parser.Abstractions
{
    using Exceptions;
    using Lexer.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class LookaheadLexerExtensions
    {
        public static TLexeme MatchToken<TLexer, TLexeme, TToken, TSourceFile>(this LookaheadLexer<TLexer, TLexeme, TToken, TSourceFile> lexer, TToken token)
            where TLexer : ILexer<TLexeme, TToken, TSourceFile>
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile
        {
            var lexeme = lexer.Advance();

            if (EqualityComparer<TToken>.Default.Equals(lexeme.Token, token))
            {
                return lexeme;
            }

            throw UnexpectedLexemeException.Create<string, TLexeme, TToken, TSourceFile>(lexeme, "", token);
        }

        public static TLexeme MatchToken<TLexer, TLexeme, TToken, TSourceFile>(this LookaheadLexer<TLexer, TLexeme, TToken, TSourceFile> lexer, params TToken[] tokens)
            where TLexer : ILexer<TLexeme, TToken, TSourceFile>
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile
        {
            var lexeme = lexer.Advance();

            foreach (var token in tokens)
            {
                if (EqualityComparer<TToken>.Default.Equals(lexeme.Token, token))
                {
                    return lexeme;
                }
            }

            throw UnexpectedLexemeException.Create<string, TLexeme, TToken, TSourceFile>(lexeme, "", tokens);
        }
    }
}
