namespace TempleLang.Test
{
    using CommandLine;
    using Compiler;
    using System;
    using System.Diagnostics;
    using System.IO;
    using TempleLang.Lexer;

    public class CompilerOptions
    {
        [Option('f', "file", Required = true, HelpText = "File to compile")]
        public string? SourceCode { get; set; }

        [Option('t', "target", HelpText = "Path to place the .exe in")]
        public string? Target { get; set; }

        [Option('i', "printIL", HelpText = "Whether to output the intermediate language to the target directory")]
        public bool PrintIL { get; set; }

        [Option('a', "printASM", HelpText = "Whether to output the assembler to the target directory")]
        public bool PrintASM { get; set; }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CompilerOptions>(args).WithParsed(x =>
            {
                var targetPath = Path.GetDirectoryName(x.Target ?? x.SourceCode!) ?? throw new InvalidOperationException("Invalid sourcecode path");
                var tempPath = x.PrintASM ? Path.Combine(targetPath, "ASM") : Path.GetTempPath();
                Compile(x.SourceCode!, tempPath, x.Target, x.PrintIL);
            });
        }

        private static string? Compile(string path, string tempPath, string? execFile, bool printIL)
        {
            var stopwatch = Stopwatch.StartNew();
            var text = File.ReadAllText(path);

            Console.WriteLine("Starting Compilation of " + path);

            var compiled = TempleLangHelper.Compile(text, new SourceFile(Path.GetFileName(path), Path.GetFullPath(path)), out var parserError, out var diagnostics);

            if (parserError != null) Console.WriteLine(parserError.ToString());

            foreach (var diagnostic in diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text.Split('\n')));

            if (compiled == null) return null;

            execFile ??= Path.Combine(Path.GetDirectoryName(path) ?? throw new ArgumentException(nameof(path)), Path.GetFileNameWithoutExtension(path) + ".exe");
            var file = TempleLangHelper.GenerateExecutable(compiled,
                                                           Path.GetFileNameWithoutExtension(path + Guid.NewGuid().ToString()),
                                                           tempPath,
                                                           execFile,
                                                           printIL);
            stopwatch.Stop();

            Console.WriteLine("Compilation " + (file != null ? "succeeded" : "failed"));
            Console.WriteLine("Finished in " + stopwatch.Elapsed);

            return file;
        }
    }
}