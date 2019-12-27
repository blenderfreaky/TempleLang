namespace TempleLang.Intermediate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Bound.Statements;

    public partial class Transformer
    {
        public IEnumerable<IInstruction> TransformStatement(IStatement statement) =>
            statement switch
            {
                BlockStatement stmt => TransformStatementCore(stmt),
                ExpressionStatement stmt => TransformStatementCore(stmt),
                IfStatement stmt => TransformStatementCore(stmt),
                WhileStatement stmt => TransformStatementCore(stmt),
                _ => throw new ArgumentException(nameof(statement)),
            };

        private IEnumerable<IInstruction> TransformStatementCore(BlockStatement stmt) =>
            stmt.Statements.SelectMany(TransformStatement);

        private IEnumerable<IInstruction> TransformStatementCore(ExpressionStatement stmt)
        {
            return TransformExpression(stmt.Expression, new DiscardValue());
        }

        private IEnumerable<IInstruction> TransformStatementCore(IfStatement stmt)
        {
            IReadableValue conditionResult;

            foreach (var instruction in GetValue(stmt.Condition, out conditionResult)) yield return instruction;

            var trueLabel = RequestLabel();
            var exitLabel = RequestLabel();

            yield return new ConditionalJump(trueLabel, conditionResult);

            if (stmt.FalseStatement != null)
            {
                foreach (var instruction in TransformStatement(stmt.FalseStatement)) yield return instruction;
            }

            yield return new UnconditionalJump(exitLabel);

            yield return trueLabel;
            foreach (var instruction in TransformStatement(stmt.TrueStatement)) yield return instruction;

            yield return exitLabel;
        }

        private IEnumerable<IInstruction> TransformStatementCore(WhileStatement stmt)
        {
            IReadableValue conditionResult;
            var conditionInstructions = GetValue(stmt.Condition, out conditionResult);
            var statementInstructions = TransformStatement(stmt.Statement);

            if (stmt.IsDoLoop)
            {
                var entryLabel = RequestLabel();

                yield return entryLabel;

                foreach (var instruction in statementInstructions) yield return instruction;
                foreach (var instruction in conditionInstructions) yield return instruction;

                yield return new ConditionalJump(entryLabel, conditionResult);
            }
            else
            {
                var entryLabel = RequestLabel();
                var exitLabel = RequestLabel();

                yield return entryLabel;
                foreach (var instruction in conditionInstructions) yield return instruction;

                yield return new ConditionalJump(exitLabel, conditionResult, true);

                foreach (var instruction in statementInstructions) yield return instruction;
                yield return new UnconditionalJump(entryLabel);

                yield return exitLabel;
            }
        }
    }
}
