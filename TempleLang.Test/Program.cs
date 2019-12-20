namespace TempleLang.Test
{
    using Intermediate.Declarations;
    using Intermediate.Primitives;
    using System;
    using System.IO;
    using TempleLang.Binder;
    using TempleLang.Compiler.NASM;
    using TempleLang.Lexer;
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

                var parser = Declaration.Parser;

                var eofParser =
                    (from r in parser
                     from _ in Parse.Token(Token.EoF)
                     select r);

                var parserResult = eofParser(lexemes);

                Console.WriteLine(parserResult);
                Console.WriteLine();

                if (!parserResult.IsSuccessful) continue;

                using DeclarationBinder binder = new DeclarationBinder(PrimitiveType.Types);

                var bound = binder.BindDeclaration(parserResult.Result);

                Console.WriteLine(bound);
                Console.WriteLine();

                foreach (var diagnostic in binder.Diagnostics) Console.WriteLine(diagnostic.ToStringFancy(text));

                Console.WriteLine();

                var boundProc = bound as Procedure;

                var compiler = new ProcedureCompiler(boundProc);

                var instructions = compiler.Compile(boundProc.EntryPoint);

                foreach (var instruction in instructions) Console.WriteLine(instruction.ToNASM());
            }
        }
    }
}
