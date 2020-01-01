namespace TempleLang.Lexer
{
    public enum Token
    {
        Semicolon,
        Let,
        TypeSetter,
        Accessor,
        Dereference,
        Reference,

        Using,
        Namespace,
        Func,

        If,
        Else,
        While,
        Do,
        For,
        Return,

        LeftExpressionDelimiter,
        RightExpressionDelimiter,
        LeftCodeDelimiter,
        RightCodeDelimiter,
        LeftEnumerationDelimiter,
        RightEnumerationDelimiter,

        Comma,

        Add,
        Subtract,
        Multiply,
        Divide,
        Remainder,
        LogicalNot,
        BitwiseNot,
        LogicalOr,
        BitwiseOr,
        LogicalAnd,
        BitwiseAnd,
        BitwiseXor,
        BitshiftLeft,
        BitshiftRight,

        Increment,
        Decrement,

        Assign,

        AddCompoundAssign,
        SubtractCompoundAssign,
        MultiplyCompoundAssign,
        DivideCompoundAssign,
        RemainderCompoundAssign,
        LogicalOrCompoundAssign,
        BitwiseOrCompoundAssign,
        LogicalAndCompoundAssign,
        BitwiseAndCompoundAssign,
        BitwiseXorCompoundAssign,
        BitshiftLeftCompoundAssign,
        BitshiftRightCompoundAssign,

        ComparisonGreaterThan,
        ComparisonGreaterThanOrEqual,
        ComparisonLessThan,
        ComparisonLessThanOrEqual,
        ComparisonEqual,
        ComparisonNotEqual,

        TernaryTruePrefix,
        TernaryFalsePrefix,

        IntegerLiteral,
        FloatLiteral,
        Int8Suffix,
        Int16Suffix,
        Int32Suffix,
        Int64Suffix,
        UnsignedSuffix,
        FloatSingleSuffix,
        FloatDoubleSuffix,
        StringLiteral,
        CharacterLiteral,
        BooleanFalseLiteral,
        BooleanTrueLiteral,
        NullLiteral,
        Identifier,
        StaticAccessor,

        EoF,
    }
}