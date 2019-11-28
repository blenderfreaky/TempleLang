namespace TempleLang.Parser
{
    using System.Collections.Generic;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;
    using Lexeme = Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>;
    using LexemeString = Lexer.Abstractions.LexemeString<Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>, Lexer.Token, Lexer.SourceFile>;
    using static Abstractions.ParserExtensions;
    using System.Net.Http.Headers;
    using System.Linq;
    using System.Collections;
    using System;
    using System.Collections.Immutable;
    using static Parser;
    using static AtomicParser;

    public static class ExpressionParser
    {
        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> DelimitedExpression =
            Tokens[Token.LeftExpressionDelimiter].Null<Lexeme, Expression, Lexeme, Token, SourceFile>()
            .And(Expression)
            .And(Tokens[Token.RightExpressionDelimiter].Null<Lexeme, Expression, Lexeme, Token, SourceFile>())
            .Transform(x => x.Item1.Item2);

        public static readonly NamedParser<Atomic, Lexeme, Token, SourceFile> TrueAtomic = Or(
            AtomicParser.Identifier.Cast<Identifier, Atomic, Lexeme, Token, SourceFile>(),
            AtomicParser.Literal.Cast<Literal, Atomic, Lexeme, Token, SourceFile>());

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> Atomic = Or(
            TrueAtomic.Cast<Atomic, Expression, Lexeme, Token, SourceFile>(),
            DelimitedExpression)
            .WithName(nameof(Atomic));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> PostfixExpression =
            Or(
                Atomic
                .And(Or(
                    Tokens[Token.Increment],
                    Tokens[Token.Decrement]))
                .Transform(x => (Expression)new PostfixExpression(x.Item1, x.Item2.Token)),
                Atomic)
            .WithName(nameof(PostfixExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> PrefixExpression =
            Or(
                Or(
                    Tokens[Token.Increment],
                    Tokens[Token.Decrement])
                .And(PostfixExpression)
                .Transform(x => (Expression)new PrefixExpression(x.Item2, x.Item1.Token)),
                PostfixExpression)
            .WithName(nameof(PrefixExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> MultiplicativeExpression =
            LRBinaryExpression(PrefixExpression, Or(Tokens[Token.Multiply], Tokens[Token.Divide]))
            .WithName(nameof(MultiplicativeExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> AdditiveExpression =
            LRBinaryExpression(MultiplicativeExpression, Or(Tokens[Token.Add], Tokens[Token.Subtract]))
            .WithName(nameof(AdditiveExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> ShiftExpression =
            LRBinaryExpression(AdditiveExpression, Or(Tokens[Token.BitshiftLeft], Tokens[Token.BitshiftRight]))
            .WithName(nameof(ShiftExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> RelationalExpression =
            LRBinaryExpression(ShiftExpression, Or(
                Tokens[Token.ComparisonLessThan], Tokens[Token.ComparisonLessOrEqualThan],
                Tokens[Token.ComparisonGreaterThan], Tokens[Token.ComparisonGreaterOrEqualThan]))
            .WithName(nameof(RelationalExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> EqualityExpression =
            LRBinaryExpression(RelationalExpression, Or(
                Tokens[Token.ComparisonEqual], Tokens[Token.ComparisonNotEqual]))
            .WithName(nameof(EqualityExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> BitwiseAndExpression =
            LRBinaryExpression(EqualityExpression, Tokens[Token.BitwiseAnd]);

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> BitwiseXorExpression =
            LRBinaryExpression(BitwiseAndExpression, Tokens[Token.BitwiseXor])
            .WithName(nameof(BitwiseXorExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> BitwiseOrExpression =
            LRBinaryExpression(BitwiseXorExpression, Tokens[Token.BitwiseOr])
            .WithName(nameof(BitwiseOrExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> LogicalAndExpression =
            LRBinaryExpression(BitwiseOrExpression, Tokens[Token.LogicalAnd])
            .WithName(nameof(LogicalAndExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> LogicalOrExpression =
            LRBinaryExpression(LogicalAndExpression, Tokens[Token.LogicalOr])
            .WithName(nameof(LogicalOrExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> TernaryExpression =
            Or(
                LogicalOrExpression,
                LogicalOrExpression.Then(
                    Tokens[Token.TernaryTruePrefix].Null<Lexeme, Expression, Lexeme, Token, SourceFile>(),
                    LogicalOrExpression,
                    Tokens[Token.TernaryFalsePrefix].Null<Lexeme, Expression, Lexeme, Token, SourceFile>(),
                    LogicalOrExpression)
               .Transform(x => (Expression)new TernaryExpression(x[0], x[2], x[4])))
            .WithName(nameof(TernaryExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> AssignmentExpression =
            LRBinaryExpression(TernaryExpression, TokensWhere(IsAssignment))
            .WithName(nameof(AssignmentExpression));

        public static readonly NamedParser<Expression, Lexeme, Token, SourceFile> Expression =
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

        private static NamedParser<Expression, Lexeme, Token, SourceFile> RLBinaryExpression<T>(NamedParser<T, Lexeme, Token, SourceFile> parser, NamedParser<Lexeme, Lexeme, Token, SourceFile> token)
            where T : Expression
        {
            var actualParser = new NamedParser<Expression, Lexeme, Token, SourceFile>("RL Binary operator");

            actualParser.OverrideParser(Or(
                actualParser.And(token).And(parser)
                .Transform(x => new BinaryExpression(x.Item1.Item1, x.Item2, x.Item1.Item2.Token))
                .Cast<BinaryExpression, Expression, Lexeme, Token, SourceFile>(),
                parser.Cast<T, Expression, Lexeme, Token, SourceFile>()));

            return actualParser;
        }

        private static NamedParser<Expression, Lexeme, Token, SourceFile> LRBinaryExpression<T>(NamedParser<T, Lexeme, Token, SourceFile> parser, NamedParser<Lexeme, Lexeme, Token, SourceFile> token)
            where T : Expression
        {
            var actualParser = new NamedParser<Expression, Lexeme, Token, SourceFile>("LR Binary operator");

            actualParser.OverrideParser(Or(
                parser.And(token).And(actualParser)
                .Transform(x => new BinaryExpression(x.Item1.Item1, x.Item2, x.Item1.Item2.Token))
                .Cast<BinaryExpression, Expression, Lexeme, Token, SourceFile>(),
                parser.Cast<T, Expression, Lexeme, Token, SourceFile>()));

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
    }
}