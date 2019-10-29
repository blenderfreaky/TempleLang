namespace TempleLang.Lexer.Abstractions
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public readonly struct LookaheadLexer<TLexer, TLexeme, TToken, TSourceFile>
        where TLexer : ILexer<TLexeme, TToken, TSourceFile>
        where TLexeme : ILexeme<TToken, TSourceFile>
        where TSourceFile : ISourceFile
    {
        public readonly TLexer ActualLexer { get; }

        private readonly List<TLexeme> LookaheadBuffer { get; }

        public LookaheadLexer(TLexer actualLexer)
        {
            ActualLexer = actualLexer;
            LookaheadBuffer = new List<TLexeme>();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public readonly TLexeme Peek(int distance = 0)
        {
            while (LookaheadBuffer.Count < distance) LookaheadBuffer.Add(ActualLexer.LexOne());

            return LookaheadBuffer[distance];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public readonly TLexeme Advance(int distance = 1)
        {
            int lookaheadCount = LookaheadBuffer.Count;
            if (lookaheadCount > 0)
            {
                int diff = lookaheadCount - distance;
                if (diff == 0)
                {
                    var result = LookaheadBuffer[lookaheadCount - 1];
                    LookaheadBuffer.Clear();
                    return result;
                }
                else if (diff < 0)
                {
                    LookaheadBuffer.Clear();
                    for (int i = 0; i < -diff - 1; i++) ActualLexer.LexOne();
                    return ActualLexer.LexOne();
                }
                else
                {
                    LookaheadBuffer.RemoveRange(0, distance - 1);
                    return LookaheadBuffer[0];
                }
            }

            for (int i = 0; i < distance - 1; i++) ActualLexer.LexOne();
            return ActualLexer.LexOne();
        }

        public override bool Equals(object? obj) => obj is LookaheadLexer<TLexer, TLexeme, TToken, TSourceFile> lexer
            && EqualityComparer<TLexer>.Default.Equals(ActualLexer, lexer.ActualLexer)
            && EqualityComparer<List<TLexeme>>.Default.Equals(LookaheadBuffer, lexer.LookaheadBuffer);

        public override int GetHashCode()
        {
            var hashCode = 323465068;
            hashCode = (hashCode * -1521134295) + EqualityComparer<TLexer>.Default.GetHashCode(ActualLexer);
            hashCode = (hashCode * -1521134295) + EqualityComparer<List<TLexeme>>.Default.GetHashCode(LookaheadBuffer);
            return hashCode;
        }

        public static bool operator ==(LookaheadLexer<TLexer, TLexeme, TToken, TSourceFile> left, LookaheadLexer<TLexer, TLexeme, TToken, TSourceFile> right) => left.Equals(right);

        public static bool operator !=(LookaheadLexer<TLexer, TLexeme, TToken, TSourceFile> left, LookaheadLexer<TLexer, TLexeme, TToken, TSourceFile> right) => !(left == right);
    }
}
