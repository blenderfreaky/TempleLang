namespace TempleLang.Parser
{
    public class TernaryExpression
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
    }
}