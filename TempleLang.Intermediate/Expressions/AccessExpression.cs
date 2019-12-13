using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Intermediate;
using TempleLang.Diagnostic;

namespace TempleLang.Intermediate.Expressions
{
    public struct AccessExpression : IValue
    {
        public IExpression Accessee { get; }

        public AccessOperationType AccessOperator { get; }

        public Positioned<string> Accessor { get; }

        public ValueFlags Flags { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public AccessExpression(IExpression accessee, AccessOperationType accessOperator, Positioned<string> accessor, ValueFlags flags, ITypeInfo returnType, FileLocation location)
        {
            Accessee = accessee;
            AccessOperator = accessOperator;
            Accessor = accessor;
            Flags = flags;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"({Accessee} {AccessOperator} {Accessor})";
    }

    public enum AccessOperationType
    {
        Regular,

        ERROR,
    }
}
