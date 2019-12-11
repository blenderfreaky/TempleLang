namespace TempleLang.Parser
{
    using TempleLang.Diagnostic;

    public abstract class Fragment : Syntax
    {
        protected Fragment(IPositioned location) : base(location) { }

        protected Fragment(IPositioned first, IPositioned second) : base(first, second) { }

        protected Fragment(params IPositioned[] locations) : base(locations) { }
    }
}
