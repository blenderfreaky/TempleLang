namespace TempleLang.Bound.Statements
{
    using System.Collections.Generic;
    using TempleLang.Diagnostic;
    using TempleLang.Bound.Expressions;

    public struct IfStatement : IStatement
    {
        public IExpression Condition { get; }

        public IStatement TrueStatement { get; }
        public IStatement? FalseStatement { get; }

        public FileLocation Location { get; }

        public IfStatement(IExpression condition, IStatement trueStatement, IStatement? falseStatement, FileLocation location)
        {
            Condition = condition;
            TrueStatement = trueStatement;
            FalseStatement = falseStatement;
            Location = location;
        }

        public override bool Equals(object? obj) => obj is IfStatement statement && EqualityComparer<IExpression>.Default.Equals(Condition, statement.Condition) && EqualityComparer<IStatement>.Default.Equals(TrueStatement, statement.TrueStatement) && EqualityComparer<IStatement?>.Default.Equals(FalseStatement, statement.FalseStatement) && EqualityComparer<FileLocation>.Default.Equals(Location, statement.Location);

        public override int GetHashCode()
        {
            var hashCode = 800281975;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IExpression>.Default.GetHashCode(Condition);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IStatement>.Default.GetHashCode(TrueStatement);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IStatement?>.Default.GetHashCode(FalseStatement);
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(IfStatement left, IfStatement right) => left.Equals(right);

        public static bool operator !=(IfStatement left, IfStatement right) => !(left == right);
    }
}
