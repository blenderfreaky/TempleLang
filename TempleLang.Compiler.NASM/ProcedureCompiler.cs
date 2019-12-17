namespace TempleLang.Compiler.NASM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Compiler.Abstractions;
    using TempleLang.Intermediate.Declarations;
    using TempleLang.Intermediate.Expressions;
    using TempleLang.Intermediate.Primitives;
    using ValueType = Abstractions.ValueType;

    public class ProcedureCompiler
    {
        public Procedure Procedure { get; }

        public int StackSize { get; }

        public ProcedureCompiler(Procedure procedure)
        {
            Procedure = procedure;
            StackSize = procedure.StackSize;
        }

        public IEnumerable<Instruction> Compile()
        {
            return Compile(Procedure.EntryPoint, 0);
        }

        public IEnumerable<Instruction> Compile(IExpression expression, int offset)
        {
            switch (expression)
            {
                case BinaryExpression expr: return Compile(expr, offset);
                case UnaryExpression expr: return Compile(expr, offset);
                case TernaryExpression expr: return Compile(expr, offset);
                case IValue expr: return Compile(expr, offset);
                default: throw new ArgumentException(nameof(expression));
            }
        }

        private readonly Dictionary<(BinaryOperatorType, PrimitiveType), string> BinaryOperators = new Dictionary<(BinaryOperatorType, PrimitiveType), string>
        {
            [(BinaryOperatorType.Add, PrimitiveType.Long)] = "add",
        };

        public IEnumerable<Instruction> Compile(BinaryExpression expr, int offset)
        {
            int lhsSize = expr.Lhs.ReturnType.Size;
            int rhsSize = expr.Rhs.ReturnType.Size;

            foreach (var instruction in Compile(expr.Lhs, offset)) yield return instruction;
            foreach (var instruction in Compile(expr.Rhs, offset + lhsSize)) yield return instruction;

            yield return new Instruction(BinaryOperators[(expr.Operator, (expr.ReturnType as PrimitiveType)!)],
                new AddressingParameter(new StackMemory(offset, lhsSize, expr.Lhs.ToString(), ValueType.Local)),
                new AddressingParameter(new StackMemory(offset + lhsSize, rhsSize, expr.Lhs.ToString(), ValueType.Local)));
        }

        private readonly Dictionary<(UnaryOperatorType, PrimitiveType), string> UnaryOperators = new Dictionary<(UnaryOperatorType, PrimitiveType), string>
        {
            [(UnaryOperatorType.LogicalNot, PrimitiveType.Long)] = "add",
        };

        public IEnumerable<Instruction> Compile(UnaryExpression expr, int offset)
        {
            int valueSize = expr.Value.ReturnType.Size;

            foreach (var instruction in Compile(expr.Value, offset)) yield return instruction;

            yield return new Instruction(UnaryOperators[(expr.Operator, (expr.ReturnType as PrimitiveType)!)],
                new AddressingParameter(new StackMemory(offset, valueSize, expr.Value.ToString(), ValueType.Local)));
        }

        public IEnumerable<Instruction> Compile(TernaryExpression expr, int offset)
        {
            var conditionSize = expr.Condition.ReturnType.Size;

            foreach (var instruction in Compile(expr.Condition, offset)) yield return instruction;

            string labelId = Guid.NewGuid().ToString().Replace('-', '_');

            // TODO: Abstract LiteralParameters away

            yield return new Instruction("cmp",
                new AddressingParameter(new StackMemory(offset, conditionSize, expr.Condition.ToString(), ValueType.Local)),
                new LiteralParameter("0"));

            string falseLabel = "falseValue" + labelId;
            string exitLabel = "exit" + labelId;

            yield return new Instruction("je",
                new LiteralParameter(falseLabel));

            foreach (var instruction in Compile(expr.TrueValue, offset)) yield return instruction;
            yield return new Instruction("jmp", new LiteralParameter(exitLabel));

            yield return new Instruction(label: falseLabel);
            foreach (var instruction in Compile(expr.FalseValue, offset)) yield return instruction;

            yield return new Instruction(label: exitLabel);
        }

        public IEnumerable<Instruction> Compile(IValue value, int offset)
        {
            switch (value)
            {
                case Constant<long> val: return Compile(val, offset);
            }
        }

        public IEnumerable<Instruction> Compile(Constant val, int offset)
        {

        }
    }

    public struct AssemblerProcedure
    {
    }

    public struct Instruction
    {
        public string? Label { get; }
        public string? Name { get; }
        public IParameter[]? Parameters { get; }

        public Instruction(string label)
        {
            Label = label;
            Name = null;
            Parameters = null;
        }

        public Instruction(string name, params IParameter[] parameters)
        {
            Label = string.Empty;
            Name = name;
            Parameters = parameters;
        }

        public Instruction(string label, string name, params IParameter[] parameters)
        {
            Label = label;
            Name = name;
            Parameters = parameters;
        }

        public string ToNASM() => string.Concat(
            (Label == null ? string.Empty : Label + ':').PadRight(20),
            (Name ?? string.Empty).PadRight(6),
            Parameters == null ? string.Empty : string.Join(", ", Parameters.Select(x => x.ToNASM()))
            );
    }

    public interface IParameter
    {
        string ToNASM();
    }

    public struct AddressingParameter : IParameter
    {
        public IMemory Memory { get; }

        public AddressingParameter(IMemory memory) => Memory = memory;

        public string ToNASM() => Memory switch
        {
            RegisterMemory register => register.RegisterName,
            StackMemory stack => $"[rsi + {stack.StackOffset}]",
            Constant constant => "0x" + ByteArrayToString(constant.Value),
            _ => throw new InvalidOperationException(),
        };

        private static string ByteArrayToString(byte[] ba)
        {
            var hex = new System.Text.StringBuilder(ba.Length * 2);
            foreach (byte b in ba) hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }

    public struct LiteralParameter : IParameter
    {
        public string Text { get; }

        public LiteralParameter(string text) => Text = text;

        public string ToNASM() => Text;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1027:Mark enums with FlagsAttribute", Justification = "Not flags")]
    public enum WordSize
    {
        BYTE = 1,
        WORD = 2,
        DWORD = 4,
        QWORD = 8,
        TWORD = 10,
        OWORD = 16,
        YWORD = 32,
        ZWORD = 64,
    }

    public enum IntRegisters
    {
        // 64-bit integer
        R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12, R13, R14, R15, RAX, RCX, RDX, RBX, RSP, RBP, RSI, RDI,

        // 32-bit integer
        R0D, R1D, R2D, R3D, R4D, R5D, R6D, R7D, R8D, R9D, R10D, R11D, R12D, R13D, R14D, R15D, EAX, ECX, EDX, EBX, ESP, EBP, ESI, EDI,

        // 16-bit integer
        R0W, R1W, R2W, R3W, R4W, R5W, R6W, R7W, R8W, R9W, R10W, R11W, R12W, R13W, R14W, R15W, AX, CX, DX, BX, SP, BP, SI, DI,

        // 8-bit integer
        R0B, R1B, R2B, R3B, R4B, R5B, R6B, R7B, R8B, R9B, R10B, R11B, R12B, R13B, R14B, R15B, AL, CL, DL, BL, SPL, BPL, SIL, DIL
    }
}
