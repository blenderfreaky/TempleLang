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

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(ExpressionStatement left, ExpressionStatement right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ExpressionStatement left, ExpressionStatement right)
        {
            return !(left == right);
        }
    }
}
