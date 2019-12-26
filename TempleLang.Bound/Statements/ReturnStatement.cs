﻿namespace TempleLang.Bound.Statements
{
    using TempleLang.Diagnostic;
    using TempleLang.Bound.Expressions;

    public struct ReturnStatement : IStatement
    {
        public IExpression Expression { get; }

        public FileLocation Location { get; }

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(ReturnStatement left, ReturnStatement right) => left.Equals(right);

        public static bool operator !=(ReturnStatement left, ReturnStatement right) => !(left == right);
    }
}