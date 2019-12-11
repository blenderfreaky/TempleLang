namespace TempleLang.Lexer
{
    public enum Token
    {
        StatementDelimiter,
        Declarator,
        TypeSetter,

        Return,

        If,
        Else,
        While,
        For,

        LeftExpressionDelimiter,
        RightExpressionDelimiter,
        LeftCodeDelimiter,
        RightCodeDelimiter,
        LeftEnumerationDelimiter,
        RightEnumerationDelimiter,

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
        OrCompoundAssign,
        BitwiseOrCompoundAssign,
        AndCompoundAssign,
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