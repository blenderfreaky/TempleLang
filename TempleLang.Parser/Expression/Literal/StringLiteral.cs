namespace TempleLang.Parser
{
    using Abstractions;
    using TempleLang.Lexer;

    public sealed class StringLiteral : Literal
    {
        public string Value { get; }

        public StringLiteral(string value)
        {
            Value = value;
        }

        public override string ToString() => $"\"{Value}\"";

        public static new readonly Parser<StringLiteral, Token> Parser =
            Parse.Token(Token.StringLiteral)
            .Transform(x => new StringLiteral(
                x.Text.Substring(1, x.Text.Length-2))); // Substring to get rid of double quotes
    }
}
