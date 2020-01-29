namespace TempleLang.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
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
            var result = parser(lexemes);

            if (!result.IsSuccessful) return result;

            if (result.RemainingLexemes.Length > 1)
            {
                var remaining = result.RemainingLexemes[0] + (result.RemainingLexemes.Length > 1 ? " " + result.RemainingLexemes[1] : "");
                return ParserResult.Error<T, Token>("Error matching " + remaining + " found " + result.Result, result.RemainingLexemes);
            }

            return result;
        }

        public static Compilation? Compile(
            string text,
            SourceFile sourceFile,
            out IParserResult<Parser.NamespaceDeclaration, Token>? parserError,
            out IEnumerable<DiagnosticInfo> diagnostics)
        {
            using var stringReader = new StringReader(text);

            var lexemes = Lex(stringReader, sourceFile);
            var parserResult = ParseEoF(Parser.NamespaceDeclaration.FileParser, lexemes);

            if (!parserResult.IsSuccessful)
            {
                diagnostics = Array.Empty<DiagnosticInfo>();
                parserError = parserResult;

                return null;
            }

            parserError = null;

            var compiler = new DeclarationCompiler();

            var procedureCompilations = compiler.Compile(parserResult.Result, out diagnostics);
            if (procedureCompilations == null) return null;
            return new Compilation(procedureCompilations, compiler.Externs, compiler.Imports, compiler.ConstantTable);
        }

        public static string? GenerateExecutable(
            Compilation compilation,
            string name,
            string tempPath,
            string execFile,
            bool writeIL = false)
        {
            var builder = new StringBuilder();

            builder.AppendLine("section .data");

            foreach (var instruction in compilation.WriteConstantTable()) builder.AppendLine(instruction.ToNASM());

            builder.AppendLine("section .text");

            foreach (var instruction in compilation.WriteExterns()) builder.AppendLine(instruction.ToNASM());

            foreach (var region in compilation.WriteProcedures()) builder.AppendLine(region.ToNASM());

            var code = builder.ToString();

            tempPath = Path.GetFullPath(tempPath);
            string asmFile = Path.Combine(tempPath, name + ".asm");
            string objFile = Path.Combine(tempPath, name + ".obj");

            string ilFile = Path.Combine(Path.GetDirectoryName(execFile), name + ".tlil");

            Directory.CreateDirectory(tempPath);
            File.WriteAllText(asmFile, code);

            if (writeIL)
            {
                File.WriteAllText(ilFile, string.Join(Environment.NewLine, compilation.WriteIntermediate()));
            }

            string nasmArguments = $"-f win64 -o \"{objFile}\" \"{asmFile}\"";
            Console.WriteLine("> nasm " + nasmArguments);
            Process.Start("nasm", nasmArguments).WaitForExit();

            if (!File.Exists(objFile)) return null;

            Directory.CreateDirectory(Path.GetDirectoryName(execFile));

            //                                                                      Hack: LINK.EXE doesn't properly find kernel32.lib otherwise
            string linkLibraries = string.Join(" ", compilation.Imports.Select(x => $@"""C:\Program Files (x86)\Windows Kits\10\Lib\10.0.18362.0\um\x64\{x}"""));
            string linkArguments = $"/entry:_start /debug /subsystem:console /out:\"{execFile}\" \"{objFile}\" {linkLibraries}";

            Console.WriteLine("");
            Console.WriteLine("> link " + linkArguments);
            Process.Start("link", linkArguments).WaitForExit();

            return File.Exists(execFile) ? execFile : null;
        }
    }
}