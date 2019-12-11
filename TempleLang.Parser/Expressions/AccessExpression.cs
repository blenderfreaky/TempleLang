namespace TempleLang.Parser
{
    using TempleLang.Diagnostic;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class AccessExpression : Expression
    {
        public Expression Accessee { get; }
        public Positioned<Token> AccessOperator { get; }
        public Identifier Accessor { get; }

        public AccessExpression(Expression accessee, Identifier accessor, Positioned<Token> accessOperator) : base(accessee, accessor, accessOperator)
        {
            Accessee = accessee;
            AccessOperator = accessOperator;
            Accessor = accessor;
        }

        public static new readonly Parser<Expression, Token> Parser = Atomic;
            //Parse.LeftRecursive(
            //    Atomic,
            //    from elem in Identifier.Parser
            //    from op in Parse.Token(Token.Accessor)
            //    select (elem, op),
            //    (a, x) => new AccessExpression(a, x.elem, x.op));
    }
}
