namespace TempleLang.Intermediate
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Bound.Expressions;
    using TempleLang.Bound.Statements;

    public partial class Transformer
    {
        public IEnumerable<IInstruction> TransformStatementWithParameters(IStatement statement, IReadOnlyList<Local> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                yield return new ParameterQueryAssignment(i, RequestUserLocal(parameters[i]));
            }

            var exitLabel = RequestLabelInstruction(".__exit");
            foreach (var instruction in TransformStatement(statement, null, exitLabel)) yield return instruction;
            yield return exitLabel;
        }

        public IEnumerable<IInstruction> TransformStatement(IStatement statement, LabelInstruction? entryLabel, LabelInstruction exitLabel) =>
            statement switch
            {
                BlockStatement stmt => TransformStatementCore(stmt),
                ExpressionStatement stmt => TransformStatementCore(stmt),
                IfStatement stmt => TransformStatementCore(stmt),
                WhileStatement stmt => TransformStatementCore(stmt),
                ForStatement stmt => TransformStatementCore(stmt, entryLabel, exitLabel),
                ReturnStatement stmt => TransformStatementCore(stmt),
                BreakStatement stmt => TransformStatementCore(stmt, exitLabel),
                ContinueStatement stmt => TransformStatementCore(stmt, entryLabel),
                _ => throw new ArgumentException(nameof(statement)),
            };

        private IEnumerable<IInstruction> TransformStatementCore(BlockStatement stmt)
        {
            var exitLabel = RequestLabelInstruction();
            foreach (var statement in stmt.Statements.SelectMany(x => TransformStatement(x, null, exitLabel))) yield return statement;
            yield return exitLabel;
        }

        private IEnumerable<IInstruction> TransformStatementCore(ExpressionStatement stmt)
        {
            return TransformExpression(stmt.Expression, new DiscardValue());
        }

        private IEnumerable<IInstruction> TransformStatementCore(IfStatement stmt)
        {
            IReadableValue conditionResult;

            foreach (var instruction in GetValue(stmt.Condition, out conditionResult)) yield return instruction;

            var trueLabel = RequestLabelInstruction();
            var exitLabel = RequestLabelInstruction();

            yield return new ConditionalJump(trueLabel, conditionResult);

            if (stmt.FalseStatement != null)
            {
                foreach (var instruction in TransformStatement(stmt.FalseStatement, null, exitLabel)) yield return instruction;
            }

            yield return new UnconditionalJump(exitLabel);

            yield return trueLabel;
            foreach (var instruction in TransformStatement(stmt.TrueStatement, null, exitLabel)) yield return instruction;

            yield return exitLabel;
        }

        private IEnumerable<IInstruction> TransformStatementCore(WhileStatement stmt)
        {
            var entryLabel = RequestLabelInstruction();
            var exitLabel = RequestLabelInstruction();

            var conditionInstructions = GetValue(stmt.Condition, out IReadableValue conditionResult);
            var statementInstructions = TransformStatement(stmt.Statement, entryLabel, exitLabel);

            if (stmt.IsDoLoop)
            {
                yield return entryLabel;

                foreach (var instruction in statementInstructions) yield return instruction;
                foreach (var instruction in conditionInstructions) yield return instruction;

                yield return new ConditionalJump(entryLabel, conditionResult);

                yield return exitLabel;
            }
            else
            {
                yield return entryLabel;
                foreach (var instruction in conditionInstructions) yield return instruction;

                yield return new ConditionalJump(exitLabel, conditionResult, true);

                foreach (var instruction in statementInstructions) yield return instruction;
                yield return new UnconditionalJump(entryLabel);

                yield return exitLabel;
            }
        }

        private IEnumerable<IInstruction> TransformStatementCore(ForStatement stmt, LabelInstruction? parentEntryLabel, LabelInstruction parentExitLabel)
        {
            if (stmt.Prefix != null) foreach (var instruction in TransformStatement(stmt.Prefix, parentEntryLabel, parentExitLabel)) yield return instruction;

            var stepLabel = RequestLabelInstruction();
            var entryLabel = RequestLabelInstruction();
            var exitLabel = RequestLabelInstruction();

            yield return new UnconditionalJump(entryLabel);

            yield return stepLabel;
            if (stmt.Step != null)
            {
                foreach (var instruction in GetValue(stmt.Step, out _)) yield return instruction;
            }

            yield return entryLabel;
            if (stmt.Condition != null)
            {
                IReadableValue conditionResult;
                foreach (var instruction in GetValue(stmt.Condition, out conditionResult)) yield return instruction;

                yield return new ConditionalJump(exitLabel, conditionResult, true);
            }

            foreach (var instruction in TransformStatement(stmt.Statement, stepLabel, exitLabel)) yield return instruction;
            yield return new UnconditionalJump(stepLabel);

            yield return exitLabel;
        }

        private IEnumerable<IInstruction> TransformStatementCore(ReturnStatement stmt)
        {
            IReadableValue? returnLocation = null;

            if (stmt.Expression != null)
            {
                foreach (var instruction in GetValue(stmt.Expression, out returnLocation)) yield return instruction;
            }

            yield return new ReturnInstruction(stmt.Expression == null ? null : returnLocation);
        }

        private IEnumerable<IInstruction> TransformStatementCore(BreakStatement stmt, LabelInstruction exitLabel)
        {
            yield return new UnconditionalJump(exitLabel);
        }

        private IEnumerable<IInstruction> TransformStatementCore(ContinueStatement stmt, LabelInstruction? entryLabel)
        {
            if (entryLabel == null) throw new InvalidOperationException("Invalid continue");
            yield return new UnconditionalJump(entryLabel.Value);
        }
    }
}