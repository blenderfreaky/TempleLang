namespace TempleLang.Lexer.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class ILexerExtensions
    {
        public static LexemeString<TLexeme, TToken, TSourceFile> LexUntil<TLexeme, TToken, TSourceFile>(this ILexer<TLexeme, TToken, TSourceFile> lexer, TToken terminator)
            where TLexeme : ILexeme<TToken, TSourceFile>
            where TSourceFile : ISourceFile
        {
            var lexemeBuffer = new List<TLexeme>();

            while (true)
            {
                var lexeme = lexer.LexOne();

                lexemeBuffer.Add(lexeme);

                if (EqualityComparer<TToken>.Default.Equals(lexeme.Token, terminator))
                {
                    return new LexemeString<TLexeme, TToken, TSourceFile>(lexemeBuffer.ToArray());
                }

                lexemeBuffer.Add(lexeme);
            }
        }
    }
}
