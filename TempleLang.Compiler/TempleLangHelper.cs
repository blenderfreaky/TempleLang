namespace TempleLang.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using TempleLang.Diagnostic;
    using TempleLang.Lexer;
    using TempleLang.Lexer.Abstractions;
    using TempleLang.Parser.Abstractions;

    public static class TempleLangHelper
    {
        public static LexemeString<Token> Lex(StringReader source, SourceFile sourceFile) =>
            new Lexer(
                source,
                sourceFile)
            .LexUntil(Token.EoF);

        private static IParserResult<T, Token> ParseEoF<T>(Parser<T, Token> parser, LexemeString<Token> lexemes)
        {
            var eofParser =
                (from r in parser
                     // Match EoF to ensure the entire input is matched
                 from _ in Parse.Token(Token.EoF)
                 select r);

            return eofParser(lexemes);
        }

        public static Compilation? Compile(
            string text,
            SourceFile sourceFile,
            out IParserResult<Parser.NamespaceDeclaration, Token>? parserError,
            out IEnumerable<DiagnosticInfo> diagnostics)
        {
            using var stringReader = new StringReader(text);

            var lexemes = Lex(stringReader, sourceFile);
            var parserResult = ParseEoF(Parser.NamespaceDeclaration.GlobalNamespaceParser, lexemes);

            if (!parserResult.IsSuccessful)
            {
                diagnostics = Array.Empty<DiagnosticInfo>();
                parserError = parserResult;

                return null;
            }

            parserError = null;

            var compiler = new DeclarationCompiler();

            return new Compilation(compiler.Compile(parserResult.Result, out diagnostics), compiler.Externs, compiler.ConstantTable);
        }
    }
}