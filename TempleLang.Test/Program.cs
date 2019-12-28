namespace TempleLang.Test
{
    using Bound.Declarations;
    using Compiler;
    using System;
    using System.IO;
    using System.Linq;
    using TempleLang.Lexer;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var text = File.ReadAllText("../../../../Test.tl");

            var compiled = TempleLang.CompileDeclaration(text, new SourceFile("Console", null), out var parserError, out var diagnostics, out var constantTable);

            if (parserError != null) Console.WriteLine(parserError.ToString());

            foreach (var diagnostic in diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text.Split('\n')));

            if (compiled == null) return;

            Console.WriteLine("section .data");

            foreach (var constant in constantTable.CompileConstantTable()) Console.WriteLine(constant.ToNASM());

            Console.WriteLine("section .text");

            foreach (var declaration in compiled)
            {
                Console.WriteLine(declaration.Procedure.Name + ":");
                foreach(var instruction in declaration.CompileInstructions()) Console.WriteLine(instruction.ToNASM());
            }
        }
    }
}