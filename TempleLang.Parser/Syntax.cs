namespace TempleLang.Parser
{
    using TempleLang.Diagnostic;

    public abstract class Syntax : IPositioned
    {
        public FileLocation Location { get; protected set; }

        protected Syntax(IPositioned location) => Location = location.Location;

        protected Syntax(IPositioned first, IPositioned second) => Location = FileLocation.Concat(first, second);

        protected Syntax(params IPositioned[] locations) => Location = FileLocation.Concat(locations);
    }
}