namespace TempleLang.Intermediate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Bound.Expressions;
    using TempleLang.Bound.Primitives;

    public partial class Transformer
    {
        public IEnumerable<IInstruction> TransformExpression(IExpression expression, IAssignableValue target) =>
            expression switch
            {
                BinaryExpression expr => TransformExpressionCore(expr, target),
                UnaryExpression expr => TransformExpressionCore(expr, target),
                TernaryExpression expr => TransformExpressionCore(expr, target),
                IValue expr => TransformValue(expr, target),
                _ => throw new ArgumentException(nameof(expression)),
            };

        public IEnumerable<IInstruction> TransformExpressionCore(UnaryExpression expr, IAssignableValue target)
        {
            var operandResult = RequestLocal();

            foreach (var instruction in TransformExpression(expr.Operand, operandResult)) yield return instruction;

            if (!(expr.Operand.ReturnType is PrimitiveType type)) throw new InvalidOperationException("Internal Failure: Binder failed binding high-level operators to methods");

            yield return new UnaryComputationAssignment(target, operandResult, expr.Operator, type);
        }

        public IEnumerable<IInstruction> TransformExpressionCore(BinaryExpression expr, IAssignableValue target)
        {
            var lhsResult = RequestLocal();
            var rhsResult = RequestLocal();

            foreach (var instruction in TransformExpression(expr.Lhs, lhsResult)) yield return instruction;
            foreach (var instruction in TransformExpression(expr.Rhs, rhsResult)) yield return instruction;

            if (!(expr.Lhs.ReturnType is PrimitiveType type)) throw new InvalidOperationException("Internal Failure: Binder failed binding high-level operators to methods");

            yield return new BinaryComputationAssignment(target, lhsResult, rhsResult, expr.Operator, type);
        }

        public IEnumerable<IInstruction> TransformExpressionCore(TernaryExpression expr, IAssignableValue target)
        {
            var conditionResult = RequestLocal();

            foreach (var instruction in TransformExpression(expr, conditionResult)) yield return instruction;

            yield return new Conditional(
                conditionResult,
                TransformExpression(expr.TrueValue, target).ToList(),
                TransformExpression(expr.FalseValue, target).ToList());
        }
    }
}
