namespace TempleLang.Compiler.NASM
{
    using Intermediate.Statements;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using TempleLang.Intermediate.Declarations;

    public class ProcedureCompiler
    {
        public Procedure Procedure { get; }

        public int StackSize { get; }

        public ProcedureCompiler(Procedure procedure)
        {
            Procedure = procedure;
            StackSize = procedure.StackSize;
        }

        public ProcedureCompiler() { }

        public Dictionary<IntRegisters>

        public IEnumerable<Instruction> Compile(IStatement statement)
        {
            switch (statement)
            {
                case BlockStatement stmt: return Compile(stmt);
                case ExpressionStatement stmt: return Compile(stmt);
                case ReturnStatement stmt: return Compile(stmt);
                default: throw new InvalidOperationException();
            }
        }

        public IEnumerable<Instruction> Compile(BlockStatement stmt) => stmt.Statements.SelectMany(Compile);

        public IEnumerable<Instruction> Compile(ExpressionStatement stmt) => ExpressionCompiler.Compile(stmt.Expression, 0);

        public IEnumerable<Instruction> Compile(ReturnStatement stmt)
        {
            ExpressionCompiler.Compile(stmt.Expression, 0);

            yield return new Instruction(name: "ret");
        }
    }

    public struct AssemblerProcedure
    {

    }
}
