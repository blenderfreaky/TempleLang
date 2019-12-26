﻿namespace TempleLang.Bound.Statements
{
    using System.Collections.Generic;
    using TempleLang.Diagnostic;
    using TempleLang.Bound.Expressions;

    public struct ExpressionStatement : IStatement
    {
        public IExpression Expression { get; }

        public FileLocation Location { get; }

        public ExpressionStatement(IExpression expression, FileLocation location)
        {
            Expression = expression;
            Location = location;
        }

        public override string ToString() => $"{Expression}";

        public override bool Equals(object? obj) => obj is ExpressionStatement statement && EqualityComparer<IExpression>.Default.Equals(Expression, statement.Expression) && EqualityComparer<FileLocation>.Default.Equals(Location, statement.Location);

        public override int GetHashCode()
        {
            var hashCode = 49511305;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IExpression>.Default.GetHashCode(Expression);
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ExpressionStatement left, ExpressionStatement right) => left.Equals(right);

        public static bool operator !=(ExpressionStatement left, ExpressionStatement right) => !(left == right);
    }
}
