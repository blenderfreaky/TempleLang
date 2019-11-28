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

    public static partial class Parser
    {
        public static readonly Dictionary<Token, NamedParser<Lexeme, Lexeme, Token, SourceFile>> Tokens =
            TokenParsers<Lexeme, Token, SourceFile>();

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
            Token.LogicalOr => true,
            Token.BitwiseOr => true,
            Token.LogicalAnd => true,
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

        private static NamedParser<T, Lexeme, Token, SourceFile> TransformToken<T>(Token token, Func<Lexeme, T> func) =>
            Tokens[token].Transform(func);
    }
}
