namespace TempleLang.Parser
{
    using TempleLang.Diagnostic;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class LocalDeclarationStatement : Statement
    {
        public string Name { get; }
        public Expression? Assignment { get; }

        public LocalDeclarationStatement(Positioned<string> name) : base(name)
        {
            Name = name;
            Assignment = null;
        }

        public LocalDeclarationStatement(Positioned<string> name, Expression assignment) : base(name, assignment)
        {
            Name = name;
            Assignment = assignment;
        }

        public override string ToString() => $"let {Name}{(Assignment == null ? string.Empty : " = " + Assignment.ToString())};";

        public static new readonly Parser<LocalDeclarationStatement, Token> Parser =
            (from _ in Parse.Token(Token.Declarator)
             from name in Parse.Token(Token.Identifier)
             from __ in Parse.Token(Token.Assign)
             from assignment in Expression.Parser
             from ___ in Parse.Token(Token.StatementDelimiter)
             select new LocalDeclarationStatement(name.PositionedText, assignment))
            .Or(from _ in Parse.Token(Token.Declarator)
                from name in Parse.Token(Token.Identifier)
                from __ in Parse.Token(Token.StatementDelimiter)
                select new LocalDeclarationStatement(name.PositionedText));
    }
}
