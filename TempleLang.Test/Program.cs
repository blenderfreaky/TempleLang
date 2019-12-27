namespace TempleLang.Test
{
    using Compiler;
    using System;
    using System.IO;
    using TempleLang.Lexer;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var text = File.ReadAllText("../../../../Test.tl");

            var compiled = TempleLang.CompileStatement(text, new SourceFile("Console", null), out var parserError, out var diagnostics);

            if (parserError != null) Console.WriteLine(parserError.ToString());

            foreach (var diagnostic in diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text.Split('\n')));

            if (compiled == null) return;

            Console.WriteLine("section .data");

            foreach (var constant in compiled.CompileConstantTable()) Console.WriteLine(constant.ToNASM());

            Console.WriteLine("section .text");

            foreach (var constant in compiled.CompileInstructions()) Console.WriteLine(constant.ToNASM());
        }
    }
}