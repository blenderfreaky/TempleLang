namespace TempleLang.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Diagnostic;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;

    public class NamespaceDeclaration : Declaration
    {
        public Positioned<string> Name { get; }
        public List<Declaration> Declarations { get; }

        public NamespaceDeclaration(Positioned<string> name, List<Declaration> declarations, FileLocation location) : base(location)
        {
            Name = name;
            Declarations = declarations;
        }

        public override string ToString() => $"\n{{\n{string.Join(Environment.NewLine, Declarations.Select(x => "    " + x.ToString()))}\n}}";

        public static readonly Parser<NamespaceDeclaration, Token> ParenthesizedParser =
            from declarator in Parse.Token(Token.Namespace)
            from name in Parse.Token(Token.Identifier)
            from ldelim in Parse.Token(Token.LeftCodeDelimiter)
            from declarations in Declaration.Parser.Many()
            from rdelim in Parse.Token(Token.RightCodeDelimiter)
            select new NamespaceDeclaration(name, declarations, FileLocation.Concat(declarator, rdelim));

        public static readonly Parser<NamespaceDeclaration, Token> FileWideParser =
            from declarator in Parse.Token(Token.Namespace)
            from name in Parse.Token(Token.Identifier)
            from _ in Parse.Token(Token.Semicolon)
            from declarations in Declaration.Parser.Many()
            from terminator in Parse.Lookahead(Parse.Token(Token.EoF))
            select new NamespaceDeclaration(name, declarations, FileLocation.Concat(declarator, terminator));

        public static new readonly Parser<NamespaceDeclaration, Token> Parser =
            ParenthesizedParser
            .Or(FileWideParser);

        public static readonly Parser<NamespaceDeclaration, Token> GlobalNamespaceParser =
            from declarations in Declaration.Parser.Many()
            select new NamespaceDeclaration(
                FileLocation.Null.WithValue("global"),
                declarations,
                declarations.Count > 0
                ? FileLocation.Concat(declarations[0], declarations[declarations.Count-1])
                : FileLocation.Null);
    }
}
