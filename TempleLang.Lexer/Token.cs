namespace TempleLang.Lexer
{
    public enum Token
    {
        Semicolon,
        Colon,
        Let,

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
        Break,
        Continue,

        LParens,
        RParens,
        LBraces,
        RBraces,
        LBrackets,
        RBrackets,

        Comma,
        Arrow,

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
        ReferenceAssign,

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

        QuestionMark,

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