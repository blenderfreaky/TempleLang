namespace TempleLang.Intermediate.Statements
{
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;

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

        public override string ToString() => $"let {Local} = {AssignedValue?.ToString() ?? "unassigned"}";
    }
}