using System;

namespace TempleLang.Compiler.FASM
{
    public class ProcedureCompiler
    {
        public Procedure Procedure { get; }

        public int StackSize { get; }

        public ProcedureCompiler(Procedure procedure)
        {
            Procedure = procedure;
            StackSize = procedure.StackSize;
        }

        public IEnumerable<IAssemblerInstruction> Compile()
        {

        }

        public static IEnumerable<IAssemblerInstruction> ThreeAddressCode(IExpression expr, IWriteableMemory destination, IWriteableMemory buffer1, IWriteableMemory buffer2)
        {

        }

        public static IEnumerable<IAssemblerInstruction> ThreeAddressCode(BinaryExpression expr, IWriteableMemory destination, IWriteableMemory buffer1, IWriteableMemory buffer2)
        {
            foreach (var instruction in ThreeAddressCode(expr.Lhs, buffer1, destination, buffer2)) yield return instruction;
            foreach (var instruction in ThreeAddressCode(expr.Rhs, buffer2, buffer1, destination)) yield return instruction;

            yield return new AssemblerInstruction(a,)
        }
    }

    public struct AssemblerProcedure
    {
    }

    public enum Architecture
    {
        FASMx64,
    }

    public enum AssemblerInstructionType
    {
        Move,
        AddInt,
        AddUInt,
        AddFloat,
        SubtractInt,
        SubtractUInt,
        SubtractFloat,
        MultiplyInt,
        MultiplyUInt,
        MultiplyFloat,
        DivideInt,
        DivideUInt,
        DivideFloat,
    }
}
