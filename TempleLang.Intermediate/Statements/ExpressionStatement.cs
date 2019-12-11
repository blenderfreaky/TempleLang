namespace TempleLang.Intermediate.Statements
{
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;

    public struct ExpressionStatement : IStatement
    {
        public IExpression Expression { get; }

        public FileLocation Location { get; }

        public ExpressionStatement(IExpression expression, FileLocation location)
        {
            Expression = expression;
            Location = location;
        }

        public override string ToString() => $"{Expression}";
    }
}
