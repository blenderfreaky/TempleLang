namespace TempleLang.Test
{
    using Compiler;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using TempleLang.Lexer;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            var text = File.ReadAllText("../../../../Test.tl");

            var compiled = TempleLangHelper.Compile(text, new SourceFile("Console", null), out var parserError, out var diagnostics);

            if (parserError != null) Console.WriteLine(parserError.ToString());

            foreach (var diagnostic in diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text.Split('\n')));

            if (compiled == null) return;

            var file = TempleLangHelper.GenerateExecutable(compiled, "Test", "temp", "../../../../");
            stopwatch.Stop();

            Console.WriteLine("Finished in " + stopwatch.Elapsed);

            Process.Start(file);
        }
    }
}