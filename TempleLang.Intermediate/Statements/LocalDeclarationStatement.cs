using TempleLang.Diagnostic;
using TempleLang.Intermediate.Expressions;

namespace TempleLang.Intermediate.Statements
{
    public struct LocalDeclarationStatement : IStatement
    {
        public IValue Local { get; }

        public IExpression? AssignedValue { get; }

        public FileLocation Location { get; }

        public LocalDeclarationStatement(IValue local, IExpression? assignedValue, FileLocation location)
        {
            Local = local;
            AssignedValue = assignedValue;
            Location = location;
        }
    }
}