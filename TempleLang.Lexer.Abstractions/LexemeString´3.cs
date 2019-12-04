﻿namespace TempleLang.Lexer.Abstractions
{
    using System.Collections;
    using System.Collections.Generic;

    public readonly struct LexemeString<TToken> : IEnumerable<Lexeme<TToken>>
    {
        private readonly Lexeme<TToken>[] _lexemes;

        private readonly int _offset;

        public readonly int Length => _lexemes.Length - _offset;

        public Lexeme<TToken> this[int i] => _lexemes[_offset + i];

        public LexemeString(Lexeme<TToken>[] lexemes, int offset = 0)
        {
            _lexemes = lexemes;
            _offset = offset;
        }

        public LexemeString<TToken> Advance(int distance = 1) =>
            new LexemeString<TToken>(_lexemes, _offset + distance);

        /// <inheritdoc/>
        /// <remarks>Performs a shallow comparison, not a deep one.</remarks>
        public override bool Equals(object? obj) => obj is LexemeString<TToken> @string
            && EqualityComparer<Lexeme<TToken>[]>.Default.Equals(_lexemes, @string._lexemes)
            && _offset == @string._offset;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 1275843362;
            hashCode = (hashCode * -1521134295) + EqualityComparer<Lexeme<TToken>[]>.Default.GetHashCode(_lexemes);
            hashCode = (hashCode * -1521134295) + _offset.GetHashCode();
            return hashCode;
        }

        public IEnumerator<Lexeme<TToken>> GetEnumerator() => ((IEnumerable<Lexeme<TToken>>)_lexemes).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<Lexeme<TToken>>)_lexemes).GetEnumerator();

        public static bool operator ==(LexemeString<TToken> left, LexemeString<TToken> right) => left.Equals(right);

        public static bool operator !=(LexemeString<TToken> left, LexemeString<TToken> right) => !(left == right);
    }
}