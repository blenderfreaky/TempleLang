namespace TempleLang.Parser
{
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class LocalDeclarationStatement : Statement
    {
        public string Name { get; }
        public Expression? Assignment { get; }

        public LocalDeclarationStatement(string name, Expression? assignment = null)
        {
            Name = name;
            Assignment = assignment;
        }

        public static new readonly Parser<LocalDeclarationStatement, Token> Parser =
            (from name in Parse.Token(Token.Identifier)
             from _ in Parse.Token(Token.ComparisonEqual)
             from assignment in Expression.Parser
             from __ in Parse.Token(Token.StatementDelimiter)
             select new LocalDeclarationStatement(name.Text, assignment))
            .Or(from name in Parse.Token(Token.Identifier)
                from __ in Parse.Token(Token.StatementDelimiter)
                select new LocalDeclarationStatement(name.Text));
    }
}
