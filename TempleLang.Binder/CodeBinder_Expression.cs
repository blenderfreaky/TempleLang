﻿namespace TempleLang.Binder
{
    using Bound;
    using Bound.Primitives;
    using System;
    using System.Linq;
    using TempleLang.Bound.Expressions;
    using TempleLang.Diagnostic;
    using TempleLang.Lexer;
    using TempleLang.Parser;

    using IE = Bound.Expressions;
    using S = Parser;

    public partial class CodeBinder : Binder
    {
        public IExpression BindExpression(Expression syntaxExpression) => syntaxExpression switch
        {
            PrefixExpression expr => BindExpression(expr),
            PostfixExpression expr => BindExpression(expr),
            S.BinaryExpression expr => BindExpression(expr),
            S.TernaryExpression expr => BindExpression(expr),
            S.AccessExpression expr => BindExpression(expr),
            S.CallExpression expr => BindExpression(expr),
            Identifier expr => BindExpression(expr),
            Literal expr => BindLiteral(expr),
            _ => throw new ArgumentException(nameof(syntaxExpression)),
        };

        public UnaryExpression BindExpression(PrefixExpression expr)
        {
            var val = BindExpression(expr.Value);

            UnaryOperatorType op = expr.Operator.Value switch
            {
                Token.Increment => UnaryOperatorType.PreIncrement,
                Token.Decrement => UnaryOperatorType.PreDecrement,
                Token.Subtract => UnaryOperatorType.Negation,
                Token.BitwiseNot => UnaryOperatorType.BitwiseNot,
                Token.LogicalNot => UnaryOperatorType.LogicalNot,
                Token.Dereference => UnaryOperatorType.Dereference,
                Token.Reference => UnaryOperatorType.Reference,

                _ => UnaryOperatorType.ERROR,
            };

            if (op == UnaryOperatorType.ERROR)
            {
                Error(DiagnosticCode.InvalidOperator, expr.Location);
            }

            var returnType = val.ReturnType;

            return new UnaryExpression(val, op, returnType, expr.Location);
        }

        public UnaryExpression BindExpression(PostfixExpression expr)
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

            return new UnaryExpression(val, op, returnType, expr.Location);
        }

        public IE.BinaryExpression BindExpression(S.BinaryExpression expr)
        {
            var lhs = BindExpression(expr.Lhs);
            var rhs = BindExpression(expr.Rhs);

            if ((lhs == null && rhs == null) || lhs?.ReturnType != rhs?.ReturnType)
            {
                Error(DiagnosticCode.InvalidType, expr.Location);
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

                Token.AddCompoundAssign => BinaryOperatorType.Add,
                Token.SubtractCompoundAssign => BinaryOperatorType.Subtract,
                Token.MultiplyCompoundAssign => BinaryOperatorType.Multiply,
                Token.DivideCompoundAssign => BinaryOperatorType.Divide,
                Token.RemainderCompoundAssign => BinaryOperatorType.Remainder,
                Token.LogicalOrCompoundAssign => BinaryOperatorType.LogicalOr,
                Token.BitwiseOrCompoundAssign => BinaryOperatorType.BitwiseOr,
                Token.LogicalAndCompoundAssign => BinaryOperatorType.LogicalAnd,
                Token.BitwiseAndCompoundAssign => BinaryOperatorType.BitwiseAnd,
                Token.BitwiseXorCompoundAssign => BinaryOperatorType.BitwiseXor,
                Token.BitshiftLeftCompoundAssign => BinaryOperatorType.BitshiftLeft,
                Token.BitshiftRightCompoundAssign => BinaryOperatorType.BitshiftRight,

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

            var returnType = lhs?.ReturnType ?? rhs?.ReturnType;

            var computationExpression = new IE.BinaryExpression(lhs!, rhs!, op, returnType!, expr.Location);

            switch (expr.Operator.Token)
            {
                case Token.AddCompoundAssign:
                case Token.SubtractCompoundAssign:
                case Token.MultiplyCompoundAssign:
                case Token.DivideCompoundAssign:
                case Token.RemainderCompoundAssign:
                case Token.LogicalOrCompoundAssign:
                case Token.BitwiseOrCompoundAssign:
                case Token.LogicalAndCompoundAssign:
                case Token.BitwiseAndCompoundAssign:
                case Token.BitwiseXorCompoundAssign:
                case Token.BitshiftLeftCompoundAssign:
                case Token.BitshiftRightCompoundAssign:
                    return new IE.BinaryExpression(lhs!, computationExpression, BinaryOperatorType.Assign, returnType!, expr.Location);

                default:
                    return computationExpression;
            }
        }

        public IE.TernaryExpression BindExpression(S.TernaryExpression expr)
        {
            var trueVal = BindExpression(expr.TrueValue);
            var falseVal = BindExpression(expr.FalseValue);

            if (trueVal.ReturnType != falseVal.ReturnType)
            {
                Error(DiagnosticCode.InvalidType, expr.Location);
            }

            var condition = BindExpression(expr.Condition);

            if (condition.ReturnType.FullyQualifiedName != "bool") //TODO
            {
                Error(DiagnosticCode.InvalidConditionalType, expr.Location);
            }

            var returnType = trueVal.ReturnType;

            return new IE.TernaryExpression(condition, trueVal, falseVal, returnType, expr.Location);
        }

        public IE.AccessExpression BindExpression(S.AccessExpression expr)
        {
            var val = BindExpression(expr.Accessee);

            AccessOperationType op = expr.AccessOperator.Value switch
            {
                Token.Accessor => AccessOperationType.Regular,

                _ => AccessOperationType.ERROR,
            };

            //TODO
            return new IE.AccessExpression(val, op, expr.Accessor.PositionedText, ValueFlags.Readable, PrimitiveType.Bool, expr.Location);
        }

        public IE.CallExpression BindExpression(S.CallExpression expr)
        {
            var callee = BindExpression(expr.Callee);
            var parameters = expr.Parameters.Select(BindExpression).ToList();

            var calledType = callee.ReturnType;

            if (calledType is ICallable callable)
            {
                return callable.BindOverload(callee, parameters, expr.Location, this);
            }

            Error(DiagnosticCode.CallingUncallable, expr.Location);
            return new IE.CallExpression(null!, null!, PrimitiveType.Unknown, expr.Location);
        }

        public IValue BindExpression(Identifier expr) => FindValue(expr)!;
    }
}