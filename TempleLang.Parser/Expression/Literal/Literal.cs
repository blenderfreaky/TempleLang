namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class Literal : Expression
    {
        public static new readonly Parser<Literal, Token> Parser =
            NullLiteral.Parser
            .Or<Literal, Token>(NumberLiteral.Parser)
            .Or(BoolLiteral.Parser)
            .Or(StringLiteral.Parser);
    }
}
