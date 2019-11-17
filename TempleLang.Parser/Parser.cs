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

    public class Parser
    {
        private readonly LexemeString LexemeString;

        public static readonly Dictionary<Token, NamedParser<Lexeme, Lexeme, Token, SourceFile>> Tokens = TokenParsers<Lexeme, Token, SourceFile>();

        public static NamedParser<Literal, Lexeme, Token, SourceFile> Literal = Or(
            BoolLiteral.Cast<BoolLiteral, Literal, Lexeme, Token, SourceFile>(),
            IntLiteral.Cast<IntLiteral, Literal, Lexeme, Token, SourceFile>());

        public static NamedParser<NullLiteral, Lexeme, Token, SourceFile> NullLiteral =
            Tokens[Token.NullLiteral].As(() => new NullLiteral());

        public static NamedParser<BoolLiteral, Lexeme, Token, SourceFile> BoolLiteral =
            Tokens[Token.BooleanFalseLiteral].As(()=>new BoolLiteral(false))
            .Or(Tokens[Token.BooleanTrueLiteral].As(() => new BoolLiteral(true)));

        public static NamedParser<IntLiteral, Lexeme, Token, SourceFile> IntLiteral =
            Tokens[Token.IntegerLiteral]
            .And(Or(
                Tokens[Token.Int8Suffix].As(IntFlags.Int8),
                Tokens[Token.Int16Suffix].As(IntFlags.Int16),
                Tokens[Token.Int32Suffix].As(IntFlags.Int32),
                Tokens[Token.Int64Suffix].As(IntFlags.Int64),
                Tokens[Token.UnsignedSuffix].As(IntFlags.Unsigned)))
            .Transform(v => new IntLiteral(long.Parse(v.Item1.Text), v.Item2));

        public static NamedParser<FloatLiteral, Lexeme, Token, SourceFile> FloatLiteral =
            Tokens[Token.FloatLiteral]
            .And(Or(
                Tokens[Token.FloatSingleSuffix].As(FloatFlags.Single),
                Tokens[Token.FloatDoubleSuffix].As(FloatFlags.Double),
                Tokens[Token.UnsignedSuffix].As(FloatFlags.Unsigned)))
            .Transform(v => new FloatLiteral(double.Parse(v.Item1.Text), v.Item2));

        public static NamedParser<StringLiteral, Lexeme, Token, SourceFile> StringLiteral =
            Tokens[Token.StringLiteral]
            .Transform(s => new StringLiteral(s.Text.Substring(1, s.Text.Length-2)));

        public static NamedParser<IdentifierExpression, Lexeme, Token, SourceFile> IdentifierExpression =
            Tokens[Token.Identifier]
            .Transform(i => new IdentifierExpression(i.Text));

        public enum Precedence
        {
            Postfix,
            Prefix,
            Multiplicative,
            Additive,
            Shift,
            Relational,
            Equality,
            BitwiseAnd,
            BitwiseXor,
            BitwiseOr,
            LogicalAnd,
            LogicalOr,
            Ternary,
            Assignment
        }

        public static Dictionary<Precedence, NamedParser<Expression, Lexeme, Token, SourceFile>> Expression = new Dictionary<Precedence, NamedParser<Expression, Lexeme<Token, SourceFile>, Token, SourceFile>>
        {
            
        };

        private static NamedParser<BinaryExpression, Lexeme, Token, SourceFile> BinaryExpression(NamedParser<BinaryExpression, Lexeme, Token, SourceFile> parser, Token token) =>
            parser
            .SeparatedBy(Tokens[token])
            .Transform(x => new BinaryExpression(x, token))
            .WithName(" Binary operator " + token.ToString());

        public static NamedParser<TernaryExpression, Lexeme, Token, SourceFile> TernaryExpression =
            Expression[Precedence.Ternary-1]
            .Then(
                Tokens[Token.TernaryTruePrefix].Transform(_ => (Expression)null),
                Expression[Precedence.Ternary - 1],
                Tokens[Token.TernaryFalsePrefix].Transform(_ => (Expression)null),
                Expression[Precedence.Ternary - 1])
            .Transform(x => new TernaryExpression(x[0], x[2], x[4]))
            .WithName("Ternary Expression");

        private static NamedParser<Lexeme, Lexeme, Token, SourceFile> TokensWhere(Predicate<Token> predicate) =>
            Or(Tokens
                .Where(x => predicate(x.Key))
                .Select(x => x.Value)
                .ToArray());

        private static bool IsPrefixOperator(Token token) => token switch
        {
            Token.Subtract => true,
            Token.BitwiseNot => true,
            Token.Not => true,
            _ => false
        };

        private static bool IsBinaryOperator(Token token) => token switch
        {
            Token.Add => true,
            Token.Subtract => true,
            Token.Multiply => true,
            Token.Divide => true,
            Token.Remainder => true,
            Token.Not => true,
            Token.BitwiseNot => true,
            Token.Or => true,
            Token.BitwiseOr => true,
            Token.And => true,
            Token.BitwiseAnd => true,
            Token.BitwiseXor => true,
            Token.BitshiftLeft => true,
            Token.BitshiftRight => true,

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

            Token.ComparisonGreaterThan => true,
            Token.ComparisonGreaterOrEqualThan => true,
            Token.ComparisonLessThan => true,
            Token.ComparisonLessOrEqualThan => true,
            Token.ComparisonEqual => true,
            Token.ComparisonNotEqual => true,

            _ => false
        };

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

            Token.ComparisonGreaterThan => true,
            Token.ComparisonGreaterOrEqualThan => true,
            Token.ComparisonLessThan => true,
            Token.ComparisonLessOrEqualThan => true,
            Token.ComparisonEqual => true,
            Token.ComparisonNotEqual => true,

            _ => false
        };
    }
}
