namespace TempleLang.Intermediate.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Diagnostic;

    public struct BlockStatement : IStatement
    {
        public IReadOnlyList<IStatement> Statements { get; }
        public FileLocation Location { get; }

        public BlockStatement(IReadOnlyList<IStatement> statements, FileLocation location)
        {
            Statements = statements;
            Location = location;
        }

        public override string ToString() => $"\n{{\n{string.Join(Environment.NewLine, Statements.Select(x => "    " + x.ToString()))}\n}}";
    }
}
