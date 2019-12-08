namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class BoolLiteral : Literal
    {
        public bool Value { get; }

        public static readonly BoolLiteral False = new BoolLiteral(false);

        public static readonly BoolLiteral True = new BoolLiteral(true);

        public BoolLiteral(bool value) => Value = value;

        public override string ToString() => $"{Value}";

        public static new readonly Parser<BoolLiteral, Token> Parser =
            Parse.Token(Token.BooleanFalseLiteral).As(False)
            .Or(Parse.Token(Token.BooleanTrueLiteral).As(True));
    }
}
