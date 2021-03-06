﻿namespace TempleLang.Test
{
    using CommandLine;
    using Compiler;
    using System;
    using System.Diagnostics;
    using System.IO;
    using TempleLang.Lexer;

    public class CompilerOptions
    {
        [Option('f', "file", Required = true, HelpText = "File to compile.")]
        public string? SourceCode { get; set; }

        [Option('t', "target", HelpText = "Path to place the .exe in.")]
        public string? Target { get; set; }

        [Option('r', "run", HelpText = "Run the generated executable upon successful compilation.")]
        public bool RunResult { get; set; }

        [Option('i', "printIL", HelpText = "Output the intermediate language to the target directory.")]
        public bool PrintIL { get; set; }

        [Option('a', "printASM", HelpText = "Output the assembler to the target directory.")]
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
                var execFile = Compile(x.SourceCode!, tempPath, x.Target, x.PrintIL);

                if (execFile == null) return;

                Console.WriteLine($"Saved executable to {execFile}.");

                if (!x.RunResult) return;

                Console.WriteLine($"Running result.\n\n{new string('-', 20)}\n");
                Process.Start(execFile).WaitForExit();
            });
        }

        private static string? Compile(string path, string tempPath, string? execFile, bool printIL)
        {
            var stopwatch = Stopwatch.StartNew();
            var text = File.ReadAllText(path);

            string fullPath = Path.GetFullPath(path);
            Console.WriteLine("Starting Compilation of " + fullPath);

            var compiled = TempleLangHelper.Compile(text, new SourceFile(Path.GetFileName(path), fullPath), out var parserError, out var diagnostics);

            if (parserError != null) Console.WriteLine(parserError.ToString());

            foreach (var diagnostic in diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text.Split('\n')));

            if (compiled == null) return null;

            execFile ??= Path.Combine(
                Path.GetDirectoryName(path) ?? throw new ArgumentException("Invalid path", nameof(path)),
                Path.GetFileNameWithoutExtension(path) + ".exe");

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