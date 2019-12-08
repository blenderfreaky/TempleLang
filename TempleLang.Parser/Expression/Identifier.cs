namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class Identifier : Expression
    {
        public string Value { get; }

        public Identifier(string value)
        {
            Value = value;
        }

        public override string ToString() => Value;

        public static new readonly Parser<Identifier, Token> Parser =
            Parse.Token(Token.Identifier)
            .Transform(x => new Identifier(x.Text));
    }
}
