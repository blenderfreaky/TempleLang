namespace TempleLang.Parser
{
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
            CreateParserLR(PrefixExpression.Parser, Parse.Token(Token.Multiply, Token.Divide));

        public static readonly Parser<Expression, Token> Additive =
            CreateParserLR(Multiplicative, Parse.Token(Token.Add, Token.Subtract));

        public static readonly Parser<Expression, Token> Bitshift =
            CreateParserLR(Additive, Parse.Token(Token.BitshiftLeft, Token.BitshiftRight));

        public static readonly Parser<Expression, Token> Relational =
            CreateParserLR(Bitshift, Parse.Token(
                Token.ComparisonLessThan, Token.ComparisonLessThanOrEqual,
                Token.ComparisonGreaterThan, Token.ComparisonGreaterThanOrEqual));

        public static readonly Parser<Expression, Token> Equality =
            CreateParserLR(Relational, Parse.Token(Token.ComparisonEqual, Token.ComparisonNotEqual));

        public static readonly Parser<Expression, Token> BitwiseAnd =
            CreateParserLR(Equality, Parse.Token(Token.BitwiseAnd));

        public static readonly Parser<Expression, Token> BitwiseXor =
            CreateParserLR(BitwiseAnd, Parse.Token(Token.BitwiseXor));

        public static readonly Parser<Expression, Token> BitwiseOr =
            CreateParserLR(BitwiseXor, Parse.Token(Token.BitwiseOr));

        public static readonly Parser<Expression, Token> LogicalAnd =
            CreateParserLR(BitwiseOr, Parse.Token(Token.LogicalAnd));

        public static readonly Parser<Expression, Token> LogicalOr =
            CreateParserLR(LogicalAnd, Parse.Token(Token.LogicalOr));

        public static readonly Parser<Expression, Token> Assignment =
            CreateParserLR(TernaryExpression.Parser, Parse.Token(
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

        public static Parser<Expression, Token> CreateParserLR(Parser<Expression, Token> parser, Parser<Lexeme<Token>, Token> @operator) =>
            Parse.Recursive<Expression, Token>(self =>
                (from lhs in parser
                from op in @operator
                from rhs in self
                select new BinaryExpression(lhs, rhs, op))
                .Or(parser));

        //public static Parser<Expression, Token> CreateParserRL(Parser<Expression, Token> parser, Parser<Lexeme<Token>, Token> @operator) =>
        //    parser == null ? throw new ArgumentNullException(nameof(parser)) : CreatePureParserRL(parser, @operator).Or(parser);

        //public static Parser<BinaryExpression, Token> CreatePureParserRL(Parser<Expression, Token> parser, Parser<Lexeme<Token>, Token> @operator)
        //{
        //    var parser =
        //        from start in self
        //        from remainder in internalParser
        //        select new BinaryExpression(start);

        //    var internalParser =
        //           from op in @operator
        //           from rhs in Parse.Ref<(Token op, Expression expr), Token>(() => internalParser)
        //           select (op.Token, rhs);

        //    internalParser = internalParser.Or(Parse.Value<(Token op, Expression expr), Token>());

        //    return from lhs in self
        //           select new BinaryExpression(lhs, rhs, op.Token);
        //}
    }
}
