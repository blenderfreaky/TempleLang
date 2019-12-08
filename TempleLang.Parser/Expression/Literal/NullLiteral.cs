namespace TempleLang.Parser
{
    using Abstractions;
    using TempleLang.Lexer;

    public sealed class NullLiteral : Literal
    {
        public static readonly NullLiteral Null = new NullLiteral();

        private NullLiteral() { }

        public override string ToString() => "null";

        public static new readonly Parser<NullLiteral, Token> Parser =
            Parse.Token(Token.NullLiteral).As(Null);
    }
}
