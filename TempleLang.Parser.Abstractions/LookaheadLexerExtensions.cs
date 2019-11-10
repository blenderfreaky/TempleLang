namespace TempleLang.Parser.Abstractions
{
    using Exceptions;
    using Lexer;
    using Lexer.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class LookaheadLexerExtensions
    {
        public static TLexeme MatchToken<TLexeme, TToken, TSourceFile>(this LexemeString<TLexeme, TToken, TSourceFile> lexer, TToken token)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile
        {
            var lexeme = lexer[0];

            if (EqualityComparer<TToken>.Default.Equals(lexeme.Token, token))
            {
                return lexeme;
            }

            throw UnexpectedLexemeException.Create<string, TLexeme, TToken, TSourceFile>(lexeme, "", token);
        }

        public static TLexeme MatchToken<TLexeme, TToken, TSourceFile>(this LexemeString<TLexeme, TToken, TSourceFile> lexer, params TToken[] tokens)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile
        {
            var lexeme = lexer[0];

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
