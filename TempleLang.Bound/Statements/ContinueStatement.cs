namespace TempleLang.Bound.Statements
{
    using System.Collections.Generic;
    using TempleLang.Diagnostic;

    public struct ContinueStatement : IStatement
    {
        public FileLocation Location { get; }

        public ContinueStatement(FileLocation location) => Location = location;

        public override bool Equals(object? obj) => obj is ContinueStatement statement && EqualityComparer<FileLocation>.Default.Equals(Location, statement.Location);

        public override int GetHashCode() => 1369928374 + Location.GetHashCode();

        public static bool operator ==(ContinueStatement left, ContinueStatement right) => left.Equals(right);

        public static bool operator !=(ContinueStatement left, ContinueStatement right) => !(left == right);
    }
}