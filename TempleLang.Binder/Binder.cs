using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TempleLang.Diagnostic;
using TempleLang.Intermediate.Expressions;
using TempleLang.Intermediate.Primitives;
using TempleLang.Intermediate.Statements;
using TempleLang.Lexer;
using TempleLang.Parser;
using IE = TempleLang.Intermediate.Expressions;
using IS = TempleLang.Intermediate.Statements;
using S = TempleLang.Parser;

namespace TempleLang.Binder
{
    public class Binder
    {
        public Binder? Parent { get; }

        public bool HasErrors { get; }
        public ConcurrentBag<DiagnosticInfo> Diagnostics { get; }

        public Dictionary<string, IValue> Symbols { get; }

        public Binder(Binder? parent = null)
        {
            Parent = parent;
            HasErrors = false;
            Diagnostics = new ConcurrentBag<DiagnosticInfo>();
            Symbols = new Dictionary<string, IValue>();
        }

        public IStatement BindStatement(Statement syntaxStatement)
        {
            switch (syntaxStatement)
            {
                case S.ExpressionStatement stmt: return BindStatement(stmt);
                case S.LocalDeclarationStatement stmt: return BindStatement(stmt);
                case S.BlockStatement stmt: return BindStatement(stmt);
                default: throw new ArgumentException(nameof(syntaxStatement));
            }
        }

        public IS.ExpressionStatement BindStatement(S.ExpressionStatement stmt)
        {
            return new IS.ExpressionStatement(BindExpression(stmt.Expression), stmt.Location);
        }

        public IS.LocalDeclarationStatement BindStatement(S.LocalDeclarationStatement stmt)
        {
            var local = new Local(stmt.Name, ValueFlags.Assignable | ValueFlags.Readable, PrimitiveType.Double, stmt.Location);

            Symbols[local.Name] = local;

            var assignedValue = stmt.Assignment == null ? null : BindExpression(stmt.Assignment);

            return new IS.LocalDeclarationStatement(local, assignedValue, stmt.Location);
        }

        public IS.BlockStatement BindStatement(S.BlockStatement stmt)
        {
            Binder binder = new Binder(this);
            var result = new IS.BlockStatement(stmt.Statements.Select(BindStatement).ToList(), stmt.Location);
            foreach (var diagnostic in binder.Diagnostics) Diagnostics.Add(diagnostic);
            return result;
        }

        public IExpression BindExpression(Expression syntaxExpression)
        {
            switch (syntaxExpression)
            {
                case S.PrefixExpression expr: return BindExpression(expr);
                case S.PostfixExpression expr: return BindExpression(expr);
                case S.BinaryExpression expr: return BindExpression(expr);
                case S.TernaryExpression expr: return BindExpression(expr);
                case S.Identifier expr: return BindExpression(expr);
                case S.Literal expr: return BindLiteral(expr);
                default: throw new ArgumentException(nameof(syntaxExpression));
            }
        }

        private void Error(DiagnosticCode invalidType, FileLocation location)
        {

        }

        public IE.UnaryExpression BindExpression(S.PrefixExpression expr)
        {
            var val = BindExpression(expr.Value);
            UnaryOperatorType op = expr.Operator.Value switch
            {
                Token.Increment => UnaryOperatorType.PreIncrement,
                Token.Decrement => UnaryOperatorType.PreDecrement,
                Token.BitwiseNot => UnaryOperatorType.BitwiseNot,
                Token.LogicalNot => UnaryOperatorType.LogicalNot,

                _ => UnaryOperatorType.ERROR,
            };

            if (op == UnaryOperatorType.ERROR)
            {
                Error(DiagnosticCode.InvalidOperator, expr.Location);
            }

            var returnType = val.ReturnType;

            return new IE.UnaryExpression(val, op, returnType, expr.Location);
        }

        public IE.UnaryExpression BindExpression(S.PostfixExpression expr)
        {
            var val = BindExpression(expr.Value);
            UnaryOperatorType op = expr.Operator.Value switch
            {
                Token.Increment => UnaryOperatorType.PostIncrement,
                Token.Decrement => UnaryOperatorType.PostDecrement,

                _ => UnaryOperatorType.ERROR,
            };

            if (op == UnaryOperatorType.ERROR)
            {
                Error(DiagnosticCode.InvalidOperator, expr.Location);
            }

            var returnType = val.ReturnType;

            return new IE.UnaryExpression(val, op, returnType, expr.Location);
        }

        public IE.BinaryExpression BindExpression(S.BinaryExpression expr)
        {
            var lhs = BindExpression(expr.Lhs);
            var rhs = BindExpression(expr.Rhs);

            if (lhs.ReturnType != rhs.ReturnType)
            {
                Error(DiagnosticCode.InvalidConditionalType, expr.Location);
            }

            var op = expr.Operator.Token switch
            {
                Token.Add => BinaryOperatorType.Add,
                Token.Subtract => BinaryOperatorType.Subtract,
                Token.Multiply => BinaryOperatorType.Multiply,
                Token.Divide => BinaryOperatorType.Divide,
                Token.Remainder => BinaryOperatorType.Remainder,
                Token.LogicalOr => BinaryOperatorType.LogicalOr,
                Token.BitwiseOr => BinaryOperatorType.BitwiseOr,
                Token.LogicalAnd => BinaryOperatorType.LogicalAnd,
                Token.BitwiseAnd => BinaryOperatorType.BitwiseAnd,
                Token.BitwiseXor => BinaryOperatorType.BitwiseXor,
                Token.BitshiftLeft => BinaryOperatorType.BitshiftLeft,
                Token.BitshiftRight => BinaryOperatorType.BitshiftRight,

                Token.Assign => BinaryOperatorType.Assign,

                Token.AddCompoundAssign => BinaryOperatorType.AddCompoundAssign,
                Token.SubtractCompoundAssign => BinaryOperatorType.SubtractCompoundAssign,
                Token.MultiplyCompoundAssign => BinaryOperatorType.MultiplyCompoundAssign,
                Token.DivideCompoundAssign => BinaryOperatorType.DivideCompoundAssign,
                Token.RemainderCompoundAssign => BinaryOperatorType.RemainderCompoundAssign,
                Token.OrCompoundAssign => BinaryOperatorType.OrCompoundAssign,
                Token.BitwiseOrCompoundAssign => BinaryOperatorType.BitwiseOrCompoundAssign,
                Token.AndCompoundAssign => BinaryOperatorType.AndCompoundAssign,
                Token.BitwiseAndCompoundAssign => BinaryOperatorType.BitwiseAndCompoundAssign,
                Token.BitwiseXorCompoundAssign => BinaryOperatorType.BitwiseXorCompoundAssign,
                Token.BitshiftLeftCompoundAssign => BinaryOperatorType.BitshiftLeftCompoundAssign,
                Token.BitshiftRightCompoundAssign => BinaryOperatorType.BitshiftRightCompoundAssign,

                Token.ComparisonGreaterThan => BinaryOperatorType.ComparisonGreaterThan,
                Token.ComparisonGreaterThanOrEqual => BinaryOperatorType.ComparisonGreaterThanOrEqual,
                Token.ComparisonLessThan => BinaryOperatorType.ComparisonLessThan,
                Token.ComparisonLessThanOrEqual => BinaryOperatorType.ComparisonLessThanOrEqual,
                Token.ComparisonEqual => BinaryOperatorType.ComparisonEqual,
                Token.ComparisonNotEqual => BinaryOperatorType.ComparisonNotEqual,

                _ => BinaryOperatorType.ERROR,
            };

            if (op == BinaryOperatorType.ERROR)
            {
                Error(DiagnosticCode.InvalidOperator, expr.Location);
            }

            var returnType = lhs.ReturnType;

            return new IE.BinaryExpression(lhs, rhs, op, returnType, expr.Location);
        }

        public IE.TernaryExpression BindExpression(S.TernaryExpression expr)
        {
            var trueVal = BindExpression(expr.TrueValue);
            var falseVal = BindExpression(expr.FalseValue);

            if (trueVal.ReturnType != falseVal.ReturnType)
            {
                Error(DiagnosticCode.InvalidConditionalType, expr.Location);
            }

            var condition = BindExpression(expr.Condition);

            if (condition.ReturnType.FullyQualifiedName != "bool") //TODO
            {
                Error(DiagnosticCode.InvalidConditionalType, expr.Location);
            }

            var returnType = trueVal.ReturnType;

            return new IE.TernaryExpression(condition, trueVal, falseVal, returnType, expr.Location);
        }

        public IE.IValue BindExpression(S.Identifier expr)
        {
            if (Symbols.TryGetValue(expr.Name, out var value))
            {
                return value;
            }

            if (Parent != null)
            {
                return Parent.BindExpression(expr);
            }

            //TODO
            Error(DiagnosticCode.UnknownValue, expr.Location);

            return default!;
        }

        public IE.IValue BindLiteral(S.Literal syntaxLiteral)
        {
            switch (syntaxLiteral)
            {
                case S.BoolLiteral expr: return BindLiteral(expr);
                case S.NumberLiteral expr: return BindLiteral(expr);
                default: throw new ArgumentException(nameof(syntaxLiteral));
            }
        }

        public IE.IValue BindLiteral(S.BoolLiteral expr)
        {
            return new Constant<bool>(expr.Value, PrimitiveType.Bool, expr.Location);
        }

        public IE.IValue BindLiteral(S.NumberLiteral expr)
        {
            if (expr.Value.Contains(".") || (expr.Flags & (NumberFlags.SuffixDouble)) == 0)
            {
                if (!double.TryParse(expr.Value, out var result))
                {
                    Error(DiagnosticCode.InvalidNumeric, expr.Location);
                }

                return new Constant<double>(result, PrimitiveType.Double, expr.Location);
            }
            else
            {
                if (!long.TryParse(expr.Value, out var result))
                {
                    Error(DiagnosticCode.InvalidNumeric, expr.Location);
                }

                return new Constant<long>(result, PrimitiveType.Long, expr.Location);
            }
        }
    }
}
