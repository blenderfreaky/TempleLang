using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TempleLang.Binder;
using TempleLang.Bound.Primitives;
using TempleLang.Bound.Statements;
using TempleLang.CodeGenerator.NASM;
using TempleLang.Diagnostic;
using TempleLang.Intermediate;
using TempleLang.Lexer;
using TempleLang.Lexer.Abstractions;
using TempleLang.Parser;
using TempleLang.Parser.Abstractions;

namespace TempleLang.Compiler
{
    public static class TempleLang
    {
        public static LexemeString<Token> Lex(StringReader source, SourceFile sourceFile) =>
            new Lexer.Lexer(
                source,
                sourceFile)
            .LexUntil(Token.EoF);

        private static IParserResult<T, Token> ParseEoF<T>(Parser<T, Token> parser, LexemeString<Token> lexemes)
        {
            var eofParser =
                (from r in parser
                     // Match EoF to ensure the entire input is matched
                 from _ in Parser.Abstractions.Parse.Token(Token.EoF)
                 select r);

            return eofParser(lexemes);
        }

        public static IStatement? BindStatement(Statement statement, out IEnumerable<DiagnosticInfo> diagnostics)
        {
            using DeclarationBinder binder = new DeclarationBinder(PrimitiveType.Types);

            IStatement? bound;

            using (CodeBinder codeBinder = new CodeBinder(binder))
            {
                bound = codeBinder.BindStatement(statement);
            }

            diagnostics = binder.Diagnostics;

            return binder.HasErrors ? null : bound;
        }

        public static CodeCompilation? CompileStatement(string text, SourceFile sourceFile, out IParserResult<Statement, Token>? parserError, out IEnumerable<DiagnosticInfo> diagnostics)
        {
            using var stringReader = new StringReader(text);

            var lexemes = Lex(stringReader, sourceFile);
            var parserResult = ParseEoF(Statement.Parser, lexemes);

            if (!parserResult.IsSuccessful)
            {
                diagnostics = Array.Empty<DiagnosticInfo>();
                parserError = parserResult;

                return null;
            }

            parserError = null;

            var bound = BindStatement(parserResult.Result, out var binderDiagnostics);
            diagnostics = binderDiagnostics;

            if (bound == null) return null;

            var transformer = new Transformer();

            var instructions = transformer.TransformStatement(bound).ToList();

            var allocation = RegisterAllocation.Generate(instructions);

            Console.WriteLine(string.Join("\n", allocation.AssignedLocations.Select(x => x.Key + " -> " + x.Value)));
            Console.WriteLine();
            Console.WriteLine(string.Join("\n", instructions));
            Console.WriteLine();

            return new CodeCompilation(instructions, transformer.ConstantTable, allocation.AssignedLocations);
        }
    }
}