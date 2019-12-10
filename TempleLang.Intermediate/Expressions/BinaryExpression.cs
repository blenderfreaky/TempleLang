using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Compiler;
using TempleLang.Diagnostic;
using TempleLang.Lexer;

namespace TempleLang.Intermediate.Expressions
{
    public struct BinaryExpression : IExpression
    {
        public IExpression Lhs { get; }
        public IExpression Rhs { get; }

        public Positioned<Token> Operation { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public BinaryExpression(IExpression lhs, IExpression rhs, Positioned<Token> operation, ITypeInfo returnType, FileLocation location)
        {
            Lhs = lhs;
            Rhs = rhs;
            Operation = operation;
            ReturnType = returnType;
            Location = location;
        }
    }

    /*  Add,
        Subtract,
        Multiply,
        Divide,
        Remainder,
        LogicalOr,
        BitwiseOr,
        LogicalAnd,
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
        ComparisonGreaterThanOrEqual,
        ComparisonLessThan,
        ComparisonLessThanOrEqual,
        ComparisonEqual,
        ComparisonNotEqual,*/
}
