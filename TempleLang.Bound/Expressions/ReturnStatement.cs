using System.Collections.Generic;
using TempleLang.Bound.Statements;
using TempleLang.Diagnostic;

namespace TempleLang.Bound.Expressions
{
    public struct ReturnStatement : IStatement
    {
        public IExpression Expression { get; }
        public FileLocation Location { get; }

        public ReturnStatement(IExpression expression, FileLocation location)
        {
            Expression = expression;
            Location = location;
        }

        public override string ToString() => $"return {Expression}";

        public override bool Equals(object? obj) => obj is ReturnStatement statement && EqualityComparer<IExpression>.Default.Equals(Expression, statement.Expression) && EqualityComparer<FileLocation>.Default.Equals(Location, statement.Location);

        public override int GetHashCode()
        {
            var hashCode = 49511305;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IExpression>.Default.GetHashCode(Expression);
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ReturnStatement left, ReturnStatement right) => left.Equals(right);

        public static bool operator !=(ReturnStatement left, ReturnStatement right) => !(left == right);
    }
}