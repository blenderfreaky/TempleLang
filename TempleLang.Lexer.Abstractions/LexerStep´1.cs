namespace TempleLang.Lexer.Abstractions
{
    public struct LexerStep<TTokenType>
    {
        public LexerState<TTokenType> NextState;
        public bool ShouldPushToken;
        public TTokenType TokenType;

        public LexerStep(LexerState<TTokenType> nextState, TTokenType tokenType)
        {
            NextState = nextState;
            ShouldPushToken = true;
            TokenType = tokenType;
        }

        public LexerStep(LexerState<TTokenType> nextState)
        {
            NextState = nextState;
            ShouldPushToken = false;
            TokenType = default!;
        }
    }
}
