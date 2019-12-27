namespace TempleLang.Test
{
    using TempleLang.Lexer;
    using TempleLang.Bound.Primitives;
    using TempleLang.CodeGenerator.NASM;
    using TempleLang.Intermediate;
    using System;
    using System.IO;
    using System.Linq;
    using TempleLang.Binder;
    using TempleLang.Lexer.Abstractions;
    using TempleLang.Parser;
    using TempleLang.Parser.Abstractions;

    public static class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                var text = Console.ReadLine();
                using var stringReader = new StringReader(text);

                var lexemes = new Lexer(
                    stringReader,
                    new SourceFile("Console", null))
                    .LexUntil(Token.EoF);

                Console.WriteLine(string.Join('\n', lexemes));
                Console.WriteLine();

                var parser = Statement.Parser;

                var eofParser =
                    (from r in parser
                     from _ in Parse.Token(Token.EoF) // Match EoF to ensure the entire input is matched
                     select r);

                var parserResult = eofParser(lexemes);

                Console.WriteLine(parserResult);
                Console.WriteLine();

                if (!parserResult.IsSuccessful) continue;

                using DeclarationBinder binder = new DeclarationBinder(PrimitiveType.Types);
                using CodeBinder codeBinder = new CodeBinder(binder);

                var bound = codeBinder.BindStatement(parserResult.Result);

                Console.WriteLine(bound);
                Console.WriteLine();

                foreach (var diagnostic in binder.Diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text));

                Console.WriteLine();

                var transformer = new Transformer();

                var instructions = transformer.TransformStatement(bound).ToList();

                foreach (var constant in transformer.ConstantTable) Console.WriteLine("Const " + constant.DebugName + " = " + constant);

                Console.WriteLine();

                foreach (var instruction in instructions) Console.WriteLine(instruction);

                Console.WriteLine();

                var allocation = RegisterAllocation.Generate(instructions);

                foreach (var assignment in allocation.AssignedLocations) Console.WriteLine(assignment.Key + " -> " + assignment.Value);

                Console.WriteLine();

                var compiler = new CodeCompiler(instructions, transformer.ConstantTable, allocation.AssignedLocations);

                Console.WriteLine("section .data");

                foreach (var constant in compiler.CompileConstantTable()) Console.WriteLine(constant.ToNASM());

                Console.WriteLine("section .text");

                foreach (var constant in compiler.CompileInstructions()) Console.WriteLine(constant.ToNASM());
            }
        }
    }
}
