namespace TempleLang.Parser
{
    using System.Collections.Generic;
    using TempleLang.Lexer;
    using TempleLang.Parser.Abstractions;
    using Lexeme = Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>;
    using LexemeString = Lexer.Abstractions.LexemeString<Lexer.Lexeme<Lexer.Token, Lexer.SourceFile>, Lexer.Token, Lexer.SourceFile>;
    using static Abstractions.ParserExtensions;
    using System.Net.Http.Headers;
    using System.Linq;
    using System.Collections;
    using System;

    public static partial class Parser
    {
        public static readonly NamedParser<Identifier, Lexeme, Token, SourceFile> Identifier =
            TransformToken(Token.Identifier, l => new Identifier(l.Text));

        public static readonly NamedParser<Literal, Lexeme, Token, SourceFile> Literal = Or(
            NullLiteral.Cast<NullLiteral, Literal, Lexeme, Token, SourceFile>(),
            BoolLiteral.Cast<BoolLiteral, Literal, Lexeme, Token, SourceFile>(),
            NumberLiteral.Cast<NumberLiteral, Literal, Lexeme, Token, SourceFile>(),
            StringLiteral.Cast<StringLiteral, Literal, Lexeme, Token, SourceFile>());
    }

    public class Atomic : Expression
    {
    }

    public class Identifier : Atomic
    {
        public readonly string Name;

        public Identifier(string name) => Name = name;
    }
}
