using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Compiler;
using TempleLang.Diagnostic;
using TempleLang.Lexer;

namespace TempleLang.Intermediate.Expressions
{
    public struct UnaryExpression : IExpression
    {
        public IExpression Value { get; }

        public UnaryOperatorType Operator { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public UnaryExpression(IExpression value, UnaryOperatorType @operator, ITypeInfo returnType, FileLocation location)
        {
            Value = value;
            Operator = @operator;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"{Operator}({Value}) : {ReturnType}";
    }

    public enum UnaryOperatorType
    {
        PreIncrement, PostIncrement,

        PreDecrement, PostDecrement,

        LogicalNot, BitwiseNot,

        ERROR,
    }
}
