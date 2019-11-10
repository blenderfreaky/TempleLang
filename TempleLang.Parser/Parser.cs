namespace TempleLang.Parser
{
    using Abstractions;
    using TempleLang.Lexer;
    using TempleLang.Lexer.Abstractions;
    using Lexeme = TempleLang.Lexer.Lexeme<TempleLang.Lexer.Token, TempleLang.Lexer.SourceFile>;
    //using Lexer = Lexer.Lexer;

    public class Parser
    {
        private readonly LookaheadLexer<Lexer, Lexeme, Token, SourceFile> Lexer;

        public static CodeExpression ParseExpression()
        {

        }

        public static CodeExpression ParseTerm(Precedence precedence)
        {
            switch (precedence)
            {
                case Precedence.Postfix:

                    break;
                case Precedence.Prefix:
                    break;
                case Precedence.Multiplicative:
                    break;
                case Precedence.Additive:
                    break;
                case Precedence.Shift:
                    break;
                case Precedence.Relational:
                    break;
                case Precedence.Equality:
                    break;
                case Precedence.BitwiseAnd:
                    break;
                case Precedence.BitwiseXor:
                    break;
                case Precedence.BitwiseOr:
                    break;
                case Precedence.LogicalAnd:
                    break;
                case Precedence.LogicalOr:
                    break;
                case Precedence.Ternary:
                    break;
                case Precedence.Assignment:
                    Lexer.MatchToken(Token.);
                    break;
                default:
                    break;
            }
        }

        private bool IsPrefixOperator(Lexeme lexeme) => lexeme.Token switch
        {
            Token.Subtract => true,
            Token.BitwiseNot => true,
            Token.Not => true,
            _ => false
        };

        private bool IsBinaryOperator(Lexeme lexeme) => lexeme.Token switch
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
