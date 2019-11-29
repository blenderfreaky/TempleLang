namespace TempleLang.Lexer
{
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    /// <summary>
    /// Represents a string and its associated token type
    /// </summary>
    /// <typeparam name="TToken">The Token enum to classify the token type with</typeparam>
    public readonly struct Lexeme<TToken>
    {
        /// <summary>
        /// The Text this Lexeme contains
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// This tokens type
        /// </summary>
        public TToken Token { get; }

        /// <summary>
        /// The File the token was generated from
        /// </summary>
        public ISourceFile SourceFile { get; }

        /// <summary>
        /// The index of the token in the token sequence of its file
        /// </summary>
        public int LexemeIndex { get; }

        /// <summary>
        /// The tokens first characters index in the file
        /// </summary>
        public int FirstCharIndex { get; }

        /// <summary>
        /// The tokens last characters index in the file
        /// </summary>
        public int LastCharIndex { get; }

        public Lexeme(string text, TToken tokenType, ISourceFile sourceFile, int tokenIndex, int firstCharIndex, int lastCharIndex)
        {
            Text = text;
            Token = tokenType;
            SourceFile = sourceFile;
            LexemeIndex = tokenIndex;
            FirstCharIndex = firstCharIndex;
            LastCharIndex = lastCharIndex;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is Lexeme<TToken> token
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
            hashCode = (hashCode * -1521134295) + EqualityComparer<ISourceFile>.Default.GetHashCode(SourceFile);
            hashCode = (hashCode * -1521134295) + LexemeIndex.GetHashCode();
            hashCode = (hashCode * -1521134295) + FirstCharIndex.GetHashCode();
            hashCode = (hashCode * -1521134295) + LastCharIndex.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Lexeme<TToken> left, Lexeme<TToken> right) => left.Equals(right);

        public static bool operator !=(Lexeme<TToken> left, Lexeme<TToken> right) => !(left == right);

        public override string ToString() => $"\"{Text}\" {nameof(Token)}.{Token}:{SourceFile}@({FirstCharIndex}:{LastCharIndex})";
    }
}