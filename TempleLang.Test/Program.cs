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

            var compiled = TempleLangHelper.Compile(text, new SourceFile("Console", null), out var parserError, out var diagnostics);

            if (parserError != null) Console.WriteLine(parserError.ToString());

            foreach (var diagnostic in diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text.Split('\n')));

            if (compiled == null) return;

            Console.WriteLine("section .data");

            foreach (var instruction in compiled.WriteConstantTable()) Console.WriteLine(instruction.ToNASM());

            Console.WriteLine("section .text");

            Console.WriteLine("    global main");

            foreach (var instruction in compiled.WriteExterns()) Console.WriteLine(instruction.ToNASM());

            foreach (var region in compiled.WriteProcedures()) Console.WriteLine(region.ToNASM());
        }
    }
}