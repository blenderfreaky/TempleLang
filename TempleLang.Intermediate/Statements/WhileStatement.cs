using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Diagnostic;
using TempleLang.Intermediate.Expressions;

namespace TempleLang.Intermediate.Statements
{
    public struct WhileStatement : IStatement
    {
        public IExpression Condition { get; }

        public IStatement Statement { get; }

        public bool IsDoLoop { get; }

        public FileLocation Location { get; }

        public WhileStatement(IExpression condition, IStatement statement, bool isDoLoop, FileLocation location)
        {
            Condition = condition;
            Statement = statement;
            IsDoLoop = isDoLoop;
            Location = location;
        }

        public override bool Equals(object? obj) => obj is WhileStatement statement && EqualityComparer<IExpression>.Default.Equals(Condition, statement.Condition) && EqualityComparer<IStatement>.Default.Equals(Statement, statement.Statement) && IsDoLoop == statement.IsDoLoop && EqualityComparer<FileLocation>.Default.Equals(Location, statement.Location);

        public override int GetHashCode()
        {
            var hashCode = 1725153732;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IExpression>.Default.GetHashCode(Condition);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IStatement>.Default.GetHashCode(Statement);
            hashCode = (hashCode * -1521134295) + IsDoLoop.GetHashCode();
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(WhileStatement left, WhileStatement right) => left.Equals(right);

        public static bool operator !=(WhileStatement left, WhileStatement right) => !(left == right);
    }
}
