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

            if (result.RemainingLexemes.Length > 1) return ParserResult.Error<T, Token>("Error matching " + result.RemainingLexemes[result.RemainingLexemes.Length - 1], result.RemainingLexemes);

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

        public static string GenerateExecutable(
            Compilation compilation,
            string name,
            string tempPath,
            string execPath)
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
            execPath = Path.GetFullPath(execPath);
            string exeFile = Path.Combine(execPath, name + ".exe");

            Directory.CreateDirectory(tempPath);
            File.WriteAllText(asmFile, code);

            Process.Start("nasm", $"-f win64 -o \"{objFile}\" \"{asmFile}\"").WaitForExit();

            Directory.CreateDirectory(execPath);

            string libraries = @"""C:\Program Files (x86)\Windows Kits\10\Lib\10.0.18362.0\um\x64\kernel32.lib"" ""C:\Program Files (x86)\Windows Kits\10\Lib\10.0.18362.0\um\x64\user32.lib""";
            string arguments = $"/entry:_start /subsystem:console /out:\"{exeFile}\" \"{objFile}\" {libraries}";
            Process.Start("link", arguments).WaitForExit();

            return exeFile;
        }
    }
}