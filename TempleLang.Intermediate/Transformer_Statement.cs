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
            var conditionResult = RequestLocal();

            foreach (var instruction in TransformExpression(stmt.Condition, conditionResult)) yield return instruction;

            var trueLabel = new LabelInstruction();
            var exitLabel = new LabelInstruction();

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
            var conditionResult = RequestLocal();
            var conditionInstructions = TransformExpression(stmt.Condition, conditionResult);
            var statementInstructions = TransformStatement(stmt.Statement);

            if (stmt.IsDoLoop)
            {
                var entryLabel = new LabelInstruction();

                yield return entryLabel;

                foreach (var instruction in statementInstructions) yield return instruction;
                foreach (var instruction in conditionInstructions) yield return instruction;

                yield return new ConditionalJump(entryLabel, conditionResult);
            }
            else
            {
                var entryLabel = new LabelInstruction();
                var exitLabel = new LabelInstruction();

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
