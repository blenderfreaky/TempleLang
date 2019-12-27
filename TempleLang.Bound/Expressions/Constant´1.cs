﻿namespace TempleLang.Bound.Expressions
{
    using System.Collections.Generic;
    using TempleLang.Bound;
    using TempleLang.Diagnostic;

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

        public override bool Equals(object? obj) => obj is Constant<T> constant && EqualityComparer<T>.Default.Equals(Value, constant.Value) && Flags == constant.Flags && EqualityComparer<ITypeInfo>.Default.Equals(ReturnType, constant.ReturnType) && EqualityComparer<FileLocation>.Default.Equals(Location, constant.Location);

        public override int GetHashCode()
        {
            var hashCode = 1719327819;
            hashCode = (hashCode * -1521134295) + EqualityComparer<T>.Default.GetHashCode(Value);
            hashCode = (hashCode * -1521134295) + Flags.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<ITypeInfo>.Default.GetHashCode(ReturnType);
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Constant<T> left, Constant<T> right) => left.Equals(right);

        public static bool operator !=(Constant<T> left, Constant<T> right) => !(left == right);
    }
}
