namespace TempleLang.Binder
{
    using Intermediate;
    using Intermediate.Primitives;
    using System;
    using System.Linq;
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;
    using TempleLang.Lexer;
    using TempleLang.Parser;
    using IE = TempleLang.Intermediate.Expressions;
    using S = TempleLang.Parser;

    public partial class CodeBinder : Binder
    {
        public IExpression BindExpression(Expression syntaxExpression) => syntaxExpression switch
        {
            S.PrefixExpression expr => BindExpression(expr),
            S.PostfixExpression expr => BindExpression(expr),
            S.BinaryExpression expr => BindExpression(expr),
            S.TernaryExpression expr => BindExpression(expr),
            S.AccessExpression expr => BindExpression(expr),
            S.CallExpression expr => BindExpression(expr),
            S.Identifier expr => BindExpression(expr),
            S.Literal expr => BindLiteral(expr),
            _ => throw new ArgumentException(nameof(syntaxExpression)),
        };

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

            var returnType = lhs?.ReturnType ?? rhs?.ReturnType;

            return new IE.BinaryExpression(lhs!, rhs!, op, returnType!, expr.Location);
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
                if (callable.Parameters.Count != parameters.Count)
                {
                    Error(DiagnosticCode.InvalidParamCount, expr.Location);
                    return new IE.CallExpression(null!, null!, PrimitiveType.Unknown, expr.Location);
                }

                for (int i = 0; i < parameters.Count; i++)
                {
                    if (callable.Parameters[i].ReturnType == parameters[i].ReturnType) continue;

                    Error(DiagnosticCode.InvalidParamType, expr.Location);
                    return new IE.CallExpression(null!, null!, PrimitiveType.Unknown, expr.Location);
                }

                return new IE.CallExpression(callee, parameters, callable.ReturnType, expr.Location);
            }

            return new IE.CallExpression(null!, null!, PrimitiveType.Unknown, expr.Location);
        }

        public IE.IValue BindExpression(S.Identifier expr) => FindValue(expr)!;
    }
}
