namespace TempleLang.Intermediate.Statements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;

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

        public int StackSize => Locals.Sum(x => x.ReturnType.Size) + Statements.Max(x => x is BlockStatement block ? block.StackSize : 0);

        public override string ToString() => string.Concat(
            Environment.NewLine,
            "{",
            Environment.NewLine,
            string.Concat(
                Locals.Select(x => $"    let {x.Name} : {x.ReturnType}" + Environment.NewLine)),
            string.Join(
                Environment.NewLine,
                Statements.Select(x => string.Concat(x.ToString().Split('\n').Select(x => "    " + x + Environment.NewLine)))),
            "}");

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(BlockStatement left, BlockStatement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BlockStatement left, BlockStatement right)
        {
            return !(left == right);
        }
    }
}
