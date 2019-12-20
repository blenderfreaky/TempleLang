using System;
using System.Collections.Generic;
using TempleLang.Compiler.Abstractions;
using TempleLang.Intermediate.Expressions;
using TempleLang.Intermediate.Primitives;

namespace TempleLang.Compiler.NASM
{
    public static class ExpressionCompiler
    {
        public static IEnumerable<Instruction> Compile(IExpression expression, int offset)
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

        private static readonly Dictionary<(BinaryOperatorType, PrimitiveType), string> BinaryOperators = new Dictionary<(BinaryOperatorType, PrimitiveType), string>
        {
            [(BinaryOperatorType.Add, PrimitiveType.Long)] = "add",
        };

        public static IEnumerable<Instruction> Compile(BinaryExpression expr, int offset)
        {
            int lhsSize = expr.Lhs.ReturnType.Size;
            int rhsSize = expr.Rhs.ReturnType.Size;

            foreach (var instruction in Compile(expr.Lhs, offset)) yield return instruction;
            foreach (var instruction in Compile(expr.Rhs, offset + lhsSize)) yield return instruction;

            var tempRegister = new RegisterMemory(nameof(IntRegisters.R0), rhsSize, expr.Rhs.ToString(), MemoryValueType.Local);
            var lhsStackMemory = new StackMemory(offset, lhsSize, expr.Lhs.ToString(), MemoryValueType.Local);
            var rhsStackMemory = new StackMemory(offset + lhsSize, rhsSize, expr.Rhs.ToString(), MemoryValueType.Local);

            yield return new Instruction("mov",
                new AddressingParameter(tempRegister),
                new AddressingParameter(rhsStackMemory));

            yield return new Instruction(BinaryOperators[(expr.Operator, (expr.ReturnType as PrimitiveType)!)],
                new AddressingParameter(lhsStackMemory),
                new AddressingParameter(tempRegister));
        }

        private static readonly Dictionary<(UnaryOperatorType, PrimitiveType), string> UnaryOperators = new Dictionary<(UnaryOperatorType, PrimitiveType), string>
        {
            [(UnaryOperatorType.LogicalNot, PrimitiveType.Long)] = "add",
        };

        public static IEnumerable<Instruction> Compile(UnaryExpression expr, int offset)
        {
            int valueSize = expr.Value.ReturnType.Size;

            foreach (var instruction in Compile(expr.Value, offset)) yield return instruction;

            yield return new Instruction(UnaryOperators[(expr.Operator, (expr.ReturnType as PrimitiveType)!)],
                new AddressingParameter(new StackMemory(offset, valueSize, expr.Value.ToString(), MemoryValueType.Local)));
        }

        public static IEnumerable<Instruction> Compile(TernaryExpression expr, int offset)
        {
            var conditionSize = expr.Condition.ReturnType.Size;

            foreach (var instruction in Compile(expr.Condition, offset)) yield return instruction;

            string labelId = Guid.NewGuid().ToString().Replace('-', '_');

            // TODO: Abstract LiteralParameters away

            yield return new Instruction("cmp",
                new AddressingParameter(new StackMemory(offset, conditionSize, expr.Condition.ToString(), MemoryValueType.Local)),
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

        public static IEnumerable<Instruction> Compile(IValue value, int offset)
        {
            switch (value)
            {
                case Constant<long> val: return Compile(new Constant(val.Value.ToString(), 8, "long " + val.Value), offset);
                case Local val: return Compile(val, offset);
                default: throw new ArgumentException(nameof(value));
            }
        }

        public static IEnumerable<Instruction> Compile(Constant val, int offset)
        {
            yield return new Instruction("mov",
                new AddressingParameter(new StackMemory(offset, val.Size, val.DebugName, MemoryValueType.Local)),
                new AddressingParameter(val));
        }

        public static IEnumerable<Instruction> Compile(Local val, int offset)
        {
            yield return new Instruction("mov",
                new AddressingParameter(new RegisterMemory(nameof(IntRegisters.R0), val.ReturnType.Size, val.ToString(), MemoryValueType.Local)),
                new AddressingParameter(new StackMemory(val.))
        }
    }
}