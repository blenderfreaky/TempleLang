namespace TempleLang.Intermediate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Bound.Statements;

    public partial class Transformer
    {
        public IEnumerable<IInstruction> TransformStatment(IStatement statement) =>
            statement switch
            {
                BlockStatement stmt => TransformStatementCore(stmt),
                ExpressionStatement stmt => TransformStatementCore(stmt),
                _ => throw new ArgumentException(nameof(statement)),
            };

        public IEnumerable<IInstruction> TransformStatementCore(BlockStatement stmt) =>
            stmt.Statements.SelectMany(TransformStatment);

        public IEnumerable<IInstruction> TransformStatementCore(ExpressionStatement stmt)
        {
            return TransformExpression(stmt.Expression, new DiscardValue());
        }

        public IEnumerable<IInstruction> TransformStatementCore(IfStatement stmt)
        {
            var conditionResult = RequestLocal();

            foreach (var instruction in TransformExpression(stmt.Condition, conditionResult)) yield return instruction;

            yield return new Conditional(
                   conditionResult,
                   TransformStatment(stmt.TrueStatement).ToList(),
                   stmt.FalseStatement == null
                       ? (IReadOnlyList<IInstruction>)Array.Empty<IInstruction>()
                       : TransformStatment(stmt.FalseStatement).ToList());
        }

        public IEnumerable<IInstruction> TransformStatementCore(WhileStatement stmt)
        {
            var conditionResult = RequestLocal();

            yield return new ConditionalLoop(
                   conditionResult,
                   TransformExpression(stmt.Condition, conditionResult),
                   TransformStatment(stmt.Statement),
                   stmt.IsDoLoop);
        }
    }
}
