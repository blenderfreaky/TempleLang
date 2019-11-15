namespace TempleLang.Parser
{
    using Lexer.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;
    using Lexeme = Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>;
    using LexemeString = Lexer.Abstractions.LexemeString<Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>, Lexer.Token, Lexer.SourceFile>;

    public class Parser
    {
        private readonly LexemeString LexemeString;

        public static readonly Dictionary<Token, NamedParser<Lexeme, Lexeme, Token, SourceFile>> Tokens = ((IEnumerable<Token>)Enum.GetValues(typeof(Token))).ToDictionary(x => x, x => x.Match<Lexeme, Token, SourceFile>());

        public static NamedParser<Literals, Lexeme, Token, SourceFile> 

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


        private static bool IsAssignment(oken token) => token switch
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
}
