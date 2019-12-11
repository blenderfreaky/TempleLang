namespace TempleLang.Binder
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using TempleLang.Diagnostic;
    using TempleLang.Intermediate.Expressions;

    public partial class Binder
    {
        public Binder? Parent { get; }

        public bool HasErrors { get; private set; }
        public ConcurrentBag<DiagnosticInfo> Diagnostics { get; }

        public Dictionary<string, IValue> Symbols { get; }

        public Binder(Binder? parent = null)
        {
            Parent = parent;
            HasErrors = false;
            Diagnostics = new ConcurrentBag<DiagnosticInfo>();
            Symbols = new Dictionary<string, IValue>();
        }

        private void Error(DiagnosticCode invalidType, FileLocation location)
        {
            Diagnostics.Add(new DiagnosticInfo(invalidType, location));
            HasErrors = true;
        }

    }
}
