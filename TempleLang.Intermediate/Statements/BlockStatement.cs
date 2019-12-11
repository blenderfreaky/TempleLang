using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Diagnostic;

namespace TempleLang.Intermediate.Statements
{
    public struct BlockStatement : IStatement
    {
        public IReadOnlyList<IStatement> Statements { get; }
        public FileLocation Location { get; }

        public BlockStatement(IReadOnlyList<IStatement> statements, FileLocation location)
        {
            Statements = statements;
            Location = location;
        }
    }
}
