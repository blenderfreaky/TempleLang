namespace TempleLang.Intermediate.Expressions
{
    using System.Collections.Generic;
    using TempleLang.Diagnostic;

    public struct CallExpression : IExpression
    {
        public IExpression Callee { get; }

        public IReadOnlyList<IExpression> Parameters { get; }

        public ITypeInfo ReturnType { get; }

        public FileLocation Location { get; }

        public CallExpression(IExpression callee, IReadOnlyList<IExpression> parameters, ITypeInfo returnType, FileLocation location)
        {
            Callee = callee;
            Parameters = parameters;
            ReturnType = returnType;
            Location = location;
        }

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(CallExpression left, CallExpression right) => left.Equals(right);

        public static bool operator !=(CallExpression left, CallExpression right) => !(left == right);
    }
}
