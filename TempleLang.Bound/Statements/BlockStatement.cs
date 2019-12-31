namespace TempleLang.Bound.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Bound.Expressions;
    using TempleLang.Diagnostic;

    public struct BlockStatement : IStatement
    {
        public IReadOnlyCollection<Local> Locals { get; }
        public IReadOnlyList<IStatement> Statements { get; }
        public FileLocation Location { get; }

        public BlockStatement(IReadOnlyCollection<Local> locals, IReadOnlyList<IStatement> statements, FileLocation location)
        {
            Locals = locals;
            Statements = statements;
            Location = location;
        }

        public override string ToString() =>
            Environment.NewLine
            + "{"
            + Environment.NewLine
            + string.Concat(
                  Locals.Select(x => $"    let {x.Name} : {x.ReturnType}" + Environment.NewLine))
            + string.Join(
                  Environment.NewLine,
                  Statements.Select(x => string.Concat(x.ToString().Split('\n').Select(x => "    " + x + Environment.NewLine))))
            + "}";

        public override bool Equals(object? obj) => obj is BlockStatement statement && EqualityComparer<IReadOnlyCollection<Local>>.Default.Equals(Locals, statement.Locals) && EqualityComparer<IReadOnlyList<IStatement>>.Default.Equals(Statements, statement.Statements) && EqualityComparer<FileLocation>.Default.Equals(Location, statement.Location);

        public override int GetHashCode()
        {
            var hashCode = 1166748500;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyCollection<Local>>.Default.GetHashCode(Locals);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyList<IStatement>>.Default.GetHashCode(Statements);
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(BlockStatement left, BlockStatement right) => left.Equals(right);

        public static bool operator !=(BlockStatement left, BlockStatement right) => !(left == right);
    }
}