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
        Not,
        BitwiseNot,
        Or,
        BitwiseOr,
        And,
        BitwiseAnd,
        BitwiseXor,
        BitshiftLeft,
        BitshiftRight,

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
        ComparisonGreaterOrEqualThan,
        ComparisonLessThan,
        ComparisonLessOrEqualThan,
        ComparisonEqual,
        ComparisonNotEqual,

        IntegerLiteral,
        RealLiteral,
        Int8Suffix,
        Int16Suffix,
        Int32Suffix,
        Int64Suffix,
        FloatSingleSuffix,
        FloatDoubleSuffix,
        StringLiteral,
        CharacterLiteral,
        BooleanLiteral,
        Identifier,
        StaticAccessor,

        EoF,
    }
}