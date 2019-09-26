namespace TempleLang.Lexer
{
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    /// <inheritdoc/>
    public readonly struct Token<TTokenType, TFile> : IToken<TTokenType, TFile>
        where TFile : IFile
    {
        /// <inheritdoc/>
        public string Text { get; }

        /// <inheritdoc/>
        public TTokenType TokenType { get; }

        /// <inheritdoc/>
        public TFile File { get; }

        /// <inheritdoc/>
        public int TokenIndex { get; }

        /// <inheritdoc/>
        public int FirstCharIndex { get; }

        /// <inheritdoc/>
        public int LastCharIndex { get; }

        public Token(string text, TTokenType tokenType, TFile file, int tokenIndex, int firstCharIndex, int lastCharIndex)
        {
            Text = text;
            TokenType = tokenType;
            File = file;
            TokenIndex = tokenIndex;
            FirstCharIndex = firstCharIndex;
            LastCharIndex = lastCharIndex;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Token<TTokenType, TFile> token && Text == token.Text && EqualityComparer<TTokenType>.Default.Equals(TokenType, token.TokenType) && EqualityComparer<IFile>.Default.Equals(File, token.File) && TokenIndex == token.TokenIndex && FirstCharIndex == token.FirstCharIndex && LastCharIndex == token.LastCharIndex;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = -1476409121;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Text);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TTokenType>.Default.GetHashCode(TokenType);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TFile>.Default.GetHashCode(File);
            hashCode = (hashCode * -1521134295) + TokenIndex.GetHashCode();
            hashCode = (hashCode * -1521134295) + FirstCharIndex.GetHashCode();
            hashCode = (hashCode * -1521134295) + LastCharIndex.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Token<TTokenType, TFile> left, Token<TTokenType, TFile> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token<TTokenType, TFile> left, Token<TTokenType, TFile> right)
        {
            return !(left == right);
        }
    }
}