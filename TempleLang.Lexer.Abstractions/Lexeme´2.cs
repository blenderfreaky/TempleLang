namespace TempleLang.Lexer
{
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    /// <inheritdoc/>
    public readonly struct Lexeme<TToken, TSourceFile> : ILexeme<TToken, TSourceFile>

        where TSourceFile : ISourceFile
    {
        /// <inheritdoc/>
        public string Text { get; }

        /// <inheritdoc/>
        public TToken Token { get; }

        /// <inheritdoc/>
        public TSourceFile SourceFile { get; }

        /// <inheritdoc/>
        public int LexemeIndex { get; }

        /// <inheritdoc/>
        public int FirstCharIndex { get; }

        /// <inheritdoc/>
        public int LastCharIndex { get; }

        public Lexeme(string text, TToken tokenType, TSourceFile sourceFile, int tokenIndex, int firstCharIndex, int lastCharIndex)
        {
            Text = text;
            Token = tokenType;
            SourceFile = sourceFile;
            LexemeIndex = tokenIndex;
            FirstCharIndex = firstCharIndex;
            LastCharIndex = lastCharIndex;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Lexeme<TToken, TSourceFile> token
            && Text == token.Text
            && EqualityComparer<TToken>.Default.Equals(Token, token.Token)
            && EqualityComparer<ISourceFile>.Default.Equals(SourceFile, token.SourceFile)
            && LexemeIndex == token.LexemeIndex && FirstCharIndex == token.FirstCharIndex
            && LastCharIndex == token.LastCharIndex;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = -1476409121;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Text);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TToken>.Default.GetHashCode(Token);
            hashCode = (hashCode * -1521134295) + EqualityComparer<TSourceFile>.Default.GetHashCode(SourceFile);
            hashCode = (hashCode * -1521134295) + LexemeIndex.GetHashCode();
            hashCode = (hashCode * -1521134295) + FirstCharIndex.GetHashCode();
            hashCode = (hashCode * -1521134295) + LastCharIndex.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Lexeme<TToken, TSourceFile> left, Lexeme<TToken, TSourceFile> right) => left.Equals(right);

        public static bool operator !=(Lexeme<TToken, TSourceFile> left, Lexeme<TToken, TSourceFile> right) => !(left == right);

        public override string ToString() => $"\"{Text}\" {nameof(Token)}.{Token}:{LexemeIndex}@{SourceFile}:({FirstCharIndex}-{LastCharIndex})";
    }
}