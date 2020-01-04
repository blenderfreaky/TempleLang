namespace TempleLang.Bound.Statements
{
    using System.Collections.Generic;
    using TempleLang.Diagnostic;

    public struct BreakStatement : IStatement
    {
        public FileLocation Location { get; }

        public BreakStatement(FileLocation location) => Location = location;

        public override bool Equals(object? obj) => obj is BreakStatement statement && EqualityComparer<FileLocation>.Default.Equals(Location, statement.Location);

        public override int GetHashCode() => 1369928374 + Location.GetHashCode();

        public static bool operator ==(BreakStatement left, BreakStatement right) => left.Equals(right);

        public static bool operator !=(BreakStatement left, BreakStatement right) => !(left == right);
    }
}