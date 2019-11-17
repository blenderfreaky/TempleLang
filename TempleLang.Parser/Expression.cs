namespace TempleLang.Parser
{
    public abstract class Expression
    {
    }

    public class IdentifierExpression : Expression
    {
        public readonly string Name;

        public IdentifierExpression(string name) => Name = name;
    }
}