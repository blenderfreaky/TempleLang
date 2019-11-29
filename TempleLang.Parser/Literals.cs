namespace TempleLang.Parser
{
    using System;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;
    using static Abstractions.ParserExtensions;
    using static Parser;

    public static class LiteralParser
    {
        public static readonly NamedParser<NullLiteral, Token> NullLiteral =
            Tokens[Token.NullLiteral].As(() => new NullLiteral());

        public static readonly NamedParser<BoolLiteral, Token> BoolLiteral =
            Tokens[Token.BooleanFalseLiteral].As(() => new BoolLiteral(false))
            .Or(Tokens[Token.BooleanTrueLiteral].As(() => new BoolLiteral(true)));

        public static readonly NamedParser<NumberLiteral, Token> NumberLiteral =
            Tokens[Token.IntegerLiteral]
            .And(Or(
                Tokens[Token.Int8Suffix].As(NumberFlags.Int8),
                Tokens[Token.Int16Suffix].As(NumberFlags.Int16),
                Tokens[Token.Int32Suffix].As(NumberFlags.Int32),
                Tokens[Token.Int64Suffix].As(NumberFlags.Int64),
                Tokens[Token.FloatSingleSuffix].As(NumberFlags.Single),
                Tokens[Token.FloatDoubleSuffix].As(NumberFlags.Double),
                Tokens[Token.UnsignedSuffix].As(NumberFlags.Unsigned))
            .Many(aggregator: (NumberFlags x, NumberFlags a) => a | x))
            .Transform(v => new NumberLiteral(v.Item1.Text, v.Item2));

        public static readonly NamedParser<StringLiteral, Token> StringLiteral =
            Tokens[Token.StringLiteral]
            .Transform(s => new StringLiteral(s.Text.Substring(1, s.Text.Length - 2)));
    }

    public abstract class Literal : Atomic
    {
    }

    public class NullLiteral : Literal
    {
    }

    public class BoolLiteral : Literal
    {
        public readonly bool Value;

        public BoolLiteral(bool value) => Value = value;
    }

    [Flags]
    public enum NumberFlags
    {
        None = 0,

        Int8 = 1 << 0,
        Int16 = 1 << 1,
        Int32 = 1 << 2,
        Int64 = 1 << 3,

        Single = 1 << 4,
        Double = 1 << 5,

        Unsigned = 1 << 6,
    }

    public class NumberLiteral : Literal
    {
        public readonly string Value;
        public readonly NumberFlags Flags;

        public NumberLiteral(string value, NumberFlags flags)
        {
            Value = value;
            Flags = flags;
        }
    }

    public class StringLiteral : Literal
    {
        public readonly string Value;

        public StringLiteral(string value)
        {
            Value = value;
        }
    }
}