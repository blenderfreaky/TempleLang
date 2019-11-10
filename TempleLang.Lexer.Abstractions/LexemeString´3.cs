namespace TempleLang.Lexer.Abstractions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public readonly struct LexemeString<TLexeme, TToken, TSourceFile>
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile
    {
        private readonly TLexeme[] _lexemes;

        private readonly int _offset;

        public TLexeme this[int i] => _lexemes[_offset + i];

        public LexemeString(TLexeme[] lexemes, int offset = 0)
        {
            _lexemes = lexemes;
            _offset = offset;
        }

        public LexemeString<TLexeme, TToken, TSourceFile> Advance(int distance = 1) =>
            new LexemeString<TLexeme, TToken, TSourceFile>(_lexemes, _offset + distance);

        /// <inheritdoc/>
        /// <remarks>Performs a shallow comparison, not a deep one.</remarks>
        public override bool Equals(object? obj) => obj is LexemeString<TLexeme, TToken, TSourceFile> @string && EqualityComparer<TLexeme[]>.Default.Equals(_lexemes, @string._lexemes) && _offset == @string._offset;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 1275843362;
            hashCode = (hashCode * -1521134295) + EqualityComparer<TLexeme[]>.Default.GetHashCode(_lexemes);
            hashCode = (hashCode * -1521134295) + _offset.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(LexemeString<TLexeme, TToken, TSourceFile> left, LexemeString<TLexeme, TToken, TSourceFile> right) => left.Equals(right);

        public static bool operator !=(LexemeString<TLexeme, TToken, TSourceFile> left, LexemeString<TLexeme, TToken, TSourceFile> right) => !(left == right);
    }
}
