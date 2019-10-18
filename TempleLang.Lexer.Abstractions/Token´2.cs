namespace TempleLang.Lexer
{
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    /// <inheritdoc/>
    public readonly struct Token<TTokenType, TSourceFile> : IToken<TTokenType, TSourceFile>
        where TSourceFile : ISourceFile
    {
        /// <inheritdoc/>
        public string Text { get; }

        /// <inheritdoc/>
        public TTokenType TokenType { get; }

        /// <inheritdoc/>
        public TSourceFile SourceFile { get; }

        /// <inheritdoc/>
        public int TokenIndex { get; }

        /// <inheritdoc/>
        public int FirstCharIndex { get; }

        /// <inheritdoc/>
        public int LastCharIndex { get; }

        public Token(string text, TTokenType tokenType, TSourceFile sourceFile, int tokenIndex, int firstCharIndex, int lastCharIndex)
        {
            Text = text;
            TokenType = tokenType;
            SourceFile = sourceFile;
            TokenIndex = tokenIndex;
            FirstCharIndex = firstCharIndex;
            LastCharIndex = lastCharIndex;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Token<TTokenType, TSourceFile> token
            && Text == token.Text
            && EqualityComparer<TTokenType>.Default.Equals(TokenType, token.TokenType)
            && EqualityComparer<ISourceFile>.Default.Equals(SourceFile, token.SourceFile)
            && TokenIndex == token.TokenIndex && FirstCharIndex == token.FirstCharIndex
            && LastCharIndex == token.LastCharIndex;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = -1476409121;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Text);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TTokenType>.Default.GetHashCode(TokenType);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TSourceFile>.Default.GetHashCode(SourceFile);
            hashCode = (hashCode * -1521134295) + TokenIndex.GetHashCode();
            hashCode = (hashCode * -1521134295) + FirstCharIndex.GetHashCode();
            hashCode = (hashCode * -1521134295) + LastCharIndex.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Token<TTokenType, TSourceFile> left, Token<TTokenType, TSourceFile> right) => left.Equals(right);

        public static bool operator !=(Token<TTokenType, TSourceFile> left, Token<TTokenType, TSourceFile> right) => !(left == right);
    }
}