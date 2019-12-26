namespace TempleLang.Binder
{
    using Bound;
    using System;
    using System.Collections.Concurrent;
    using TempleLang.Diagnostic;
    using S = TempleLang.Parser;

    public abstract partial class Binder : IDisposable
    {
        public Binder? Parent { get; }

        public bool HasErrors { get; private set; }
        public ConcurrentBag<DiagnosticInfo> Diagnostics { get; }

        protected Binder(Binder? parent = null)
        {
            Parent = parent;
            HasErrors = false;
            Diagnostics = new ConcurrentBag<DiagnosticInfo>();
        }

        protected void Error(DiagnosticCode invalidType, FileLocation location)
        {
            Diagnostics.Add(new DiagnosticInfo(invalidType, location));
            HasErrors = true;
        }

        public abstract ITypeInfo FindType(S.Expression expression);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Parent != null)
                    {
                        Parent.HasErrors |= HasErrors;

                        foreach (var diagnostic in Diagnostics) Parent.Diagnostics.Add(diagnostic);
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
