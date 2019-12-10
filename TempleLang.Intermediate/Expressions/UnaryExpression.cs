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

        public Positioned<Token> Operation { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public UnaryExpression(IExpression value, Positioned<Token> operation, ITypeInfo returnType, FileLocation location)
        {
            Value = value;
            Operation = operation;
            ReturnType = returnType;
            Location = location;
        }
    }
}
