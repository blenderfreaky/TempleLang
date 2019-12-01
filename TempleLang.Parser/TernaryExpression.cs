namespace TempleLang.Parser
{
    public class TernaryExpression : Expression
    {
        public readonly Expression Condition;
        public readonly Expression TrueValue;
        public readonly Expression FalseValue;

        public TernaryExpression(Expression condition, Expression trueValue, Expression falseValue)
        {
            Condition = condition;
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public override string ToString() => $"({Condition}) ? ({TrueValue}) : ({FalseValue})";
    }
}