namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;
    using static Abstractions.ParserExtensions;
    using static Parser;
    using Lexeme = Lexer.Lexeme<Lexer.Token>;

    public static class ExpressionParser
    {
        public static readonly NamedParser<Expression, Token> DelimitedExpression =
            Tokens[Token.LeftExpressionDelimiter].Null<Lexeme, Expression, Token>()
            .And(Ref(() => Expression).WithName("Expression"))
            .And(Tokens[Token.RightExpressionDelimiter].Null<Lexeme, Expression, Token>())
            .Transform(x => x.Item1.Item2);

        public static readonly NamedParser<Atomic, Token> TrueAtomic = Or(
            AtomicParser.Identifier.Cast<Identifier, Atomic, Token>(),
            AtomicParser.Literal.Cast<Literal, Atomic, Token>());

        public static readonly NamedParser<Expression, Token> Atomic = Or(
            TrueAtomic.Cast<Atomic, Expression, Token>(),
            DelimitedExpression)
            .WithName(nameof(Atomic));

        public static readonly NamedParser<Expression, Token> PostfixExpression =
            Or(
                Atomic
                .And(Or(
                    Tokens[Token.Increment],
                    Tokens[Token.Decrement]))
                .Transform(x => (Expression)new PostfixExpression(x.Item1, x.Item2.Token)),
                Atomic)
            .WithName(nameof(PostfixExpression));

        public static readonly NamedParser<Expression, Token> PrefixExpression =
            Or(
                Or(
                    Tokens[Token.Increment],
                    Tokens[Token.Decrement])
                .And(PostfixExpression)
                .Transform(x => (Expression)new PrefixExpression(x.Item2, x.Item1.Token)),
                PostfixExpression)
            .WithName(nameof(PrefixExpression));

        public static readonly NamedParser<Expression, Token> MultiplicativeExpression =
            LRBinaryExpression(PrefixExpression, Or(Tokens[Token.Multiply], Tokens[Token.Divide]))
            .WithName(nameof(MultiplicativeExpression));

        public static readonly NamedParser<Expression, Token> AdditiveExpression =
            LRBinaryExpression(MultiplicativeExpression, Or(Tokens[Token.Add], Tokens[Token.Subtract]))
            .WithName(nameof(AdditiveExpression));

        public static readonly NamedParser<Expression, Token> ShiftExpression =
            LRBinaryExpression(AdditiveExpression, Or(Tokens[Token.BitshiftLeft], Tokens[Token.BitshiftRight]))
            .WithName(nameof(ShiftExpression));

        public static readonly NamedParser<Expression, Token> RelationalExpression =
            LRBinaryExpression(ShiftExpression, Or(
                Tokens[Token.ComparisonLessThan], Tokens[Token.ComparisonLessOrEqualThan],
                Tokens[Token.ComparisonGreaterThan], Tokens[Token.ComparisonGreaterOrEqualThan]))
            .WithName(nameof(RelationalExpression));

        public static readonly NamedParser<Expression, Token> EqualityExpression =
            LRBinaryExpression(RelationalExpression, Or(
                Tokens[Token.ComparisonEqual], Tokens[Token.ComparisonNotEqual]))
            .WithName(nameof(EqualityExpression));

        public static readonly NamedParser<Expression, Token> BitwiseAndExpression =
            LRBinaryExpression(EqualityExpression, Tokens[Token.BitwiseAnd]);

        public static readonly NamedParser<Expression, Token> BitwiseXorExpression =
            LRBinaryExpression(BitwiseAndExpression, Tokens[Token.BitwiseXor])
            .WithName(nameof(BitwiseXorExpression));

        public static readonly NamedParser<Expression, Token> BitwiseOrExpression =
            LRBinaryExpression(BitwiseXorExpression, Tokens[Token.BitwiseOr])
            .WithName(nameof(BitwiseOrExpression));

        public static readonly NamedParser<Expression, Token> LogicalAndExpression =
            LRBinaryExpression(BitwiseOrExpression, Tokens[Token.LogicalAnd])
            .WithName(nameof(LogicalAndExpression));

        public static readonly NamedParser<Expression, Token> LogicalOrExpression =
            LRBinaryExpression(LogicalAndExpression, Tokens[Token.LogicalOr])
            .WithName(nameof(LogicalOrExpression));

        public static readonly NamedParser<Expression, Token> TernaryExpression =
            Or(
                LogicalOrExpression,
                LogicalOrExpression.Then(
                    Tokens[Token.TernaryTruePrefix].Null<Lexeme, Expression, Token>(),
                    LogicalOrExpression,
                    Tokens[Token.TernaryFalsePrefix].Null<Lexeme, Expression, Token>(),
                    LogicalOrExpression)
               .Transform(x => (Expression)new TernaryExpression(x[0], x[2], x[4])))
            .WithName(nameof(TernaryExpression));

        public static readonly NamedParser<Expression, Token> AssignmentExpression =
            LRBinaryExpression(TernaryExpression, TokensWhere(IsAssignment))
            .WithName(nameof(AssignmentExpression));

        public static readonly NamedParser<Expression, Token> Expression =
            AssignmentExpression;

        private static bool IsAssignment(Token token) => token switch
        {
            Token.Assign => true,

            Token.AddCompoundAssign => true,
            Token.SubtractCompoundAssign => true,
            Token.MultiplyCompoundAssign => true,
            Token.DivideCompoundAssign => true,
            Token.RemainderCompoundAssign => true,
            Token.OrCompoundAssign => true,
            Token.BitwiseOrCompoundAssign => true,
            Token.AndCompoundAssign => true,
            Token.BitwiseAndCompoundAssign => true,
            Token.BitwiseXorCompoundAssign => true,
            Token.BitshiftLeftCompoundAssign => true,
            Token.BitshiftRightCompoundAssign => true,

            _ => false
        };

        private static NamedParser<Expression, Token> RLBinaryExpression<T>(NamedParser<T, Token> parser, NamedParser<Lexeme, Token> token)
            where T : Expression
        {
            var actualParser = new NamedParser<Expression, Token>("RL Binary operator");

            actualParser.OverrideParser(Or(
                actualParser.And(token).And(parser)
                .Transform(x => new BinaryExpression(x.Item1.Item1, x.Item2, x.Item1.Item2.Token))
                .Cast<BinaryExpression, Expression, Token>(),
                parser.Cast<T, Expression, Token>()));

            return actualParser;
        }

        private static NamedParser<Expression, Token> LRBinaryExpression<T>(NamedParser<T, Token> parser, NamedParser<Lexeme, Token> token)
            where T : Expression
        {
            var actualParser = new NamedParser<Expression, Token>("LR Binary operator");

            actualParser.OverrideParser(Or(
                parser.And(token).And(actualParser)
                .Transform(x => new BinaryExpression(x.Item1.Item1, x.Item2, x.Item1.Item2.Token))
                .Cast<BinaryExpression, Expression, Token>(),
                parser.Cast<T, Expression, Token>()));

            return actualParser;
        }
    }

    public abstract class Expression
    {
    }

    public class PostfixExpression : Expression
    {
        public readonly Expression Expression;

        public readonly Token Operator;

        public PostfixExpression(Expression expression, Token @operator)
        {
            Expression = expression;
            Operator = @operator;
        }

        public override string ToString() => $"({Expression}){Operator}";
    }

    public class PrefixExpression : Expression
    {
        public readonly Expression Expression;

        public readonly Token Operator;

        public PrefixExpression(Expression expression, Token @operator)
        {
            Expression = expression;
            Operator = @operator;
        }

        public override string ToString() => $"{Operator}({Expression})";
    }

    public class BinaryExpression : Expression
    {
        public readonly Expression Left;
        public readonly Expression Right;

        public readonly Token Operator;

        public BinaryExpression(Expression left, Expression right, Token @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public override string ToString() => $"({Left}) {Operator} ({Right})";
    }
}