﻿namespace TempleLang.Lexer
{
    public enum TokenType
    {
        StatementDelimiter,
        Declarator,
        TypeSetter,
        KeywordReturn,
        KeywordIf,
        KeywordElse,
        KeywordWhile,
        KeywordFor,
        Not,
        Assign,
        LeftExpressionDelimiter,
        RightExpressionDelimiter,
        LeftCodeDelimiter,
        RightCodeDelimiter,
        LeftEnumerationDelimiter,
        RightEnumerationDelimiter,
        Add,
        Multiply,
        BitwiseXor,
        ComparisonGreaterThan,
        Or,
        BitwiseOr,
        Subtract,
        Divide,
        ComparisonLessThan,
        BinaryNegate,
        And,
        BitwiseAnd,
        ComparisonGreaterOrEqualThan,
        ComparisonLessOrEqualThan,
        StaticAccessor,
        ComparisonNotEquals,
        Identifier
    }
}