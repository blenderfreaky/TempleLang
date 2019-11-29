namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;
    using static Abstractions.ParserExtensions;
    using static Parser;

    public static class AtomicParser
    {
        public static readonly NamedParser<Identifier, Token> Identifier =
            TransformToken(Token.Identifier, l => new Identifier(l.Text));

        public static readonly NamedParser<Literal, Token> Literal = Or(
            LiteralParser.NullLiteral.Cast<NullLiteral, Literal, Token>(),
            LiteralParser.BoolLiteral.Cast<BoolLiteral, Literal, Token>(),
            LiteralParser.NumberLiteral.Cast<NumberLiteral, Literal, Token>(),
            LiteralParser.StringLiteral.Cast<StringLiteral, Literal, Token>());
    }

    public class Atomic : Expression
    {
    }

    public class Identifier : Atomic
    {
        public readonly string Name;

        public Identifier(string name) => Name = name;
    }
}