namespace TempleLang.Lexer
{
    using TempleLang.Lexer.Abstractions;

    public readonly struct Token : IToken<TokenType>
    {
        public string Text { get; }
        public TokenType TokenType { get; }
        public IFile File { get; }
        public int TokenIndex { get; }
        public int FirstCharIndex { get; }
        public int LastCharIndex { get; }
    }
}