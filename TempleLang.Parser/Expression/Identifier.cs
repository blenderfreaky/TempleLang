namespace TempleLang.Parser
{
    using Diagnostic;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class Identifier : Expression
    {
        public string Value { get; }

        public Identifier(Positioned<string> name) : base(name)
        {
            Value = name.Value;
        }

        public override string ToString() => Value;

        public static new readonly Parser<Identifier, Token> Parser =
            Parse.Token(Token.Identifier)
            .Transform(x => new Identifier(x.PositionedText));
    }
}
