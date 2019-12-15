﻿namespace TempleLang.Compiler.NASM
{
    using System;
    using System.Collections.Generic;
    using TempleLang.Compiler.Abstractions;
    using TempleLang.Intermediate.Declarations;
    using TempleLang.Intermediate.Expressions;

        public class ProcedureCompiler
        {
            public Procedure Procedure { get; }

            public int StackSize { get; }

            public ProcedureCompiler(Procedure procedure)
            {
                Procedure = procedure;
                StackSize = procedure.StackSize;
            }

            public IEnumerable<IInstruction> Compile()
            {

            }

            public static IEnumerable<IInstruction> ThreeAddressCode(IExpression expr, IWriteableMemory destination, IWriteableMemory buffer1, IWriteableMemory buffer2)
            {

            }

            public static IEnumerable<IInstruction> ThreeAddressCode(BinaryExpression expr, IWriteableMemory destination, IWriteableMemory buffer1, IWriteableMemory buffer2)
            {
                foreach (var instruction in ThreeAddressCode(expr.Lhs, buffer1, destination, buffer2)) yield return instruction;
                foreach (var instruction in ThreeAddressCode(expr.Rhs, buffer2, buffer1, destination)) yield return instruction;

                yield return new AssemblerInstruction(a,)
            }
        }

        public struct AssemblerProcedure
        {
        }

        public interface IInstruction
        {
        }

        public interface ILabel : IInstruction
        {
            string Name { get; }
            bool IsLocal { get; }
        }

        public struct Label : ILabel
        {
            public string Name { get; }
            public bool IsLocal { get; }
        }

        public struct AnonymousLabel : ILabel
        {
            public string Name => "@@";
            public bool IsLocal => false;
        }

        public enum Architecture
        {
            FASMx64,
        }

        public enum WordSize
        {

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