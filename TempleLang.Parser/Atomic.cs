namespace TempleLang.Parser
{
    using System;
    using System.Linq;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class Atomic : Expression
    {
    }

    public class BoolLiteral : Atomic
    {
        public bool Value { get; }

        public BoolLiteral(bool value) => Value = value;

        public static readonly Parser<BoolLiteral, Token> Parser =
            Parse.Token(Token.BooleanFalseLiteral).Transform(new BoolLiteral(false))
            .Or(Parse.Token(Token.BooleanTrueLiteral).Transform(new BoolLiteral(true)));
    }

    [Flags]
    public enum NumberFlags
    {
        None = 0,

        SuffixSingle = 1 << 0,
        SuffixDouble = 1 << 1,

        SuffixInt8 = 1 << 2,
        SuffixInt16 = 1 << 3,
        SuffixInt32 = 1 << 4,
        SuffixInt64 = 1 << 5,

        Unsigned = 1 << 6,
    }

    public class NumberLiteral : Atomic
    {
        public string Value { get; }

        public NumberFlags Flags { get; }

        public NumberLiteral(string value, NumberFlags flags = NumberFlags.None)
        {
            Value = value;
            Flags = flags;
        }

        private static readonly Parser<string, Token> _numberParser =
            Parse.Token(Token.IntegerLiteral)
            .Or(Parse.Token(Token.FloatLiteral))
            .Transform(x => x.Text);

        private static readonly Parser<NumberFlags, Token> _suffix =
            Parse.Token(Token.FloatSingleSuffix).As(NumberFlags.SuffixSingle)
            .Or(Parse.Token(Token.FloatDoubleSuffix).As(NumberFlags.SuffixDouble))
            .Or(Parse.Token(Token.Int8Suffix).As(NumberFlags.SuffixInt8))
            .Or(Parse.Token(Token.Int16Suffix).As(NumberFlags.SuffixInt16))
            .Or(Parse.Token(Token.Int32Suffix).As(NumberFlags.SuffixInt32))
            .Or(Parse.Token(Token.Int64Suffix).As(NumberFlags.SuffixInt64))
            .Or(Parse.Token(Token.UnsignedSuffix).As(NumberFlags.Unsigned))
            .Many().Transform(x => x.Aggregate((a, x) => a | x));

        public static readonly Parser<NumberLiteral, Token> Parser =
            from number in _numberParser
            from suffix in _suffix
            select new NumberLiteral(number, suffix);
    }
}
