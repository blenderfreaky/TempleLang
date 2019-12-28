namespace TempleLang.Compiler
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bound.Declarations;
    using global::TempleLang.Binder;
    using global::TempleLang.Bound.Primitives;
    using global::TempleLang.Bound.Statements;
    using global::TempleLang.CodeGenerator.NASM;
    using global::TempleLang.Diagnostic;
    using global::TempleLang.Intermediate;
    using global::TempleLang.Lexer;
    using global::TempleLang.Lexer.Abstractions;
    using global::TempleLang.Parser;
    using global::TempleLang.Parser.Abstractions;

    public static class TempleLang
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
                 //from _ in Parse.Token(Token.EoF)
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

        public static ProcedureCompilation? CompileStatement(string text, SourceFile sourceFile, out IParserResult<Statement, Token>? parserError, out IEnumerable<DiagnosticInfo> diagnostics)
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

            return new ProcedureCompilation(
                null,
                instructions,
                transformer.ConstantTable.ToDictionary(x => x, x => new DataLocation(x.DebugName.Replace(" ", "_"), x.Type.Size)),
                allocation.AssignedLocations);
        }

        public static List<ProcedureCompilation>? CompileDeclaration(string text, SourceFile sourceFile, out IParserResult<List<Declaration>, Token>? parserError, out IEnumerable<DiagnosticInfo> diagnostics, out Dictionary<Constant, DataLocation>? constantTable)
        {
            using var stringReader = new StringReader(text);

            var lexemes = Lex(stringReader, sourceFile);
            var parserResult = ParseEoF(Declaration.Parser.Many(), lexemes);

            if (!parserResult.IsSuccessful)
            {
                diagnostics = Array.Empty<DiagnosticInfo>();
                constantTable = null;
                parserError = parserResult;

                return null;
            }

            parserError = null;

            var compiler = new DeclarationCompiler();

            var result = compiler.Compile(parserResult.Result);
            diagnostics = compiler.DeclarationBinder.Diagnostics;
            constantTable = compiler.Transformer.ConstantTable.ToDictionary(x => x, x => new DataLocation(x.DebugName.Replace(" ", "_"), x.Type.Size));

            return result;
        }

        public static IEnumerable<NasmInstruction> CompileConstantTable(this Dictionary<Constant, DataLocation> constantTable)
        {
            foreach (var constant in constantTable)
            {
                var isString = constant.Key.Type == PrimitiveType.String;

                yield return new NasmInstruction(
                    label: constant.Value.LabelName,
                    name: isString ? "db" : "equ",
                    new LiteralParameter(isString ? $"__utf16__({constant.Key.ValueText})" : constant.Key.ValueString));
            }
        }
    }
}