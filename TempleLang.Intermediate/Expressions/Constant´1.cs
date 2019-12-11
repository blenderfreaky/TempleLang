using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Compiler;
using TempleLang.Diagnostic;

namespace TempleLang.Intermediate.Expressions
{
    public struct Constant<T> : IValue
    {
        public T Value { get; }

        public ValueFlags Flags => ValueFlags.Readable | ValueFlags.Constant;

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public Constant(T value, ITypeInfo returnType, FileLocation location)
        {
            Value = value;
            ReturnType = returnType;
            Location = location;
        }

        public override string ToString() => $"Const {Value} : {ReturnType}";
    }
}
