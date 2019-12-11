namespace TempleLang.Parser
{
    using System;
    using System.Linq;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class BinaryExpression : Expression
    {
        public Expression Lhs { get; }
        public Expression Rhs { get; }

        public Lexeme<Token> Operator { get; }

        public BinaryExpression(Expression lhs, Expression rhs, Lexeme<Token> @operator) : base(lhs, rhs, @operator)
        {
            Lhs = lhs;
            Rhs = rhs;
            Operator = @operator;
        }

        public override string ToString() => $"({Lhs} {Operator.Text} {Rhs})";

        public static readonly Parser<Expression, Token> Multiplicative =
            CreateParserLeftToRight(PrefixExpression.Parser, Parse.Token(Token.Multiply, Token.Divide));

        public static readonly Parser<Expression, Token> Additive =
            CreateParserLeftToRight(Multiplicative, Parse.Token(Token.Add, Token.Subtract));

        public static readonly Parser<Expression, Token> Bitshift =
            CreateParserLeftToRight(Additive, Parse.Token(Token.BitshiftLeft, Token.BitshiftRight));

        public static readonly Parser<Expression, Token> Relational =
            CreateParserLeftToRight(Bitshift, Parse.Token(
                Token.ComparisonLessThan, Token.ComparisonLessThanOrEqual,
                Token.ComparisonGreaterThan, Token.ComparisonGreaterThanOrEqual));

        public static readonly Parser<Expression, Token> Equality =
            CreateParserLeftToRight(Relational, Parse.Token(Token.ComparisonEqual, Token.ComparisonNotEqual));

        public static readonly Parser<Expression, Token> BitwiseAnd =
            CreateParserLeftToRight(Equality, Parse.Token(Token.BitwiseAnd));

        public static readonly Parser<Expression, Token> BitwiseXor =
            CreateParserLeftToRight(BitwiseAnd, Parse.Token(Token.BitwiseXor));

        public static readonly Parser<Expression, Token> BitwiseOr =
            CreateParserLeftToRight(BitwiseXor, Parse.Token(Token.BitwiseOr));

        public static readonly Parser<Expression, Token> LogicalAnd =
            CreateParserLeftToRight(BitwiseOr, Parse.Token(Token.LogicalAnd));

        public static readonly Parser<Expression, Token> LogicalOr =
            CreateParserLeftToRight(LogicalAnd, Parse.Token(Token.LogicalOr));

        public static readonly Parser<Expression, Token> Assignment =
            CreateParserRightToLeft(TernaryExpression.Parser, Parse.Token(
                Token.Assign,
                Token.AddCompoundAssign,
                Token.SubtractCompoundAssign,
                Token.MultiplyCompoundAssign,
                Token.DivideCompoundAssign,
                Token.RemainderCompoundAssign,
                Token.OrCompoundAssign,
                Token.BitwiseOrCompoundAssign,
                Token.AndCompoundAssign,
                Token.BitwiseAndCompoundAssign,
                Token.BitwiseXorCompoundAssign,
                Token.BitshiftLeftCompoundAssign,
                Token.BitshiftRightCompoundAssign));

        public static Parser<Expression, Token> CreateParserLeftToRight(Parser<Expression, Token> parser, Parser<Lexeme<Token>, Token> @operator) =>
            input =>
            {
                var result = parser(input);

                if (!result.IsSuccessful) return result;

                Expression accumulator = result.Result;
                var remainder = result.RemainingLexemeString;

                while (true)
                {
                    var op = @operator(remainder);

                    if (!op.IsSuccessful) return ParserResult.Success(accumulator, remainder);

                    var res = parser(op.RemainingLexemeString);

                    if (!op.IsSuccessful) return ParserResult.Success(accumulator, remainder);

                    remainder = res.RemainingLexemeString;

                    accumulator = new BinaryExpression(accumulator, res.Result, op.Result);
                }
            };

        public static Parser<Expression, Token> CreateParserRightToLeft(Parser<Expression, Token> parser, Parser<Lexeme<Token>, Token> @operator) =>
            Parse.Recursive<Expression, Token>(self =>
                (from lhs in parser
                 from op in @operator
                 from rhs in self
                 select new BinaryExpression(lhs, rhs, op))
                .Or(parser));
    }
}
