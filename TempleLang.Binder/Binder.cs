namespace TempleLang.Binder
{
    using Bound;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Diagnostic;
    using S = TempleLang.Parser;

    public abstract class Binder : IDisposable, IDiagnosticReceiver
    {
        public Binder? Parent { get; }

        public bool HasErrors { get; private set; }
        private ConcurrentBag<DiagnosticInfo> DiagnosticsBag { get; }
        public IReadOnlyCollection<DiagnosticInfo> Diagnostics => DiagnosticsBag;

        protected Binder(Binder? parent = null)
        {
            Parent = parent;
            HasErrors = false;
            DiagnosticsBag = new ConcurrentBag<DiagnosticInfo>();
        }

        protected void Error(DiagnosticCode invalidType, FileLocation location)
        {
            ReceiveDiagnostic(new DiagnosticInfo(invalidType, location), true);
        }

        public void ReceiveDiagnostic(DiagnosticInfo info, bool error)
        {
            DiagnosticsBag.Add(info);
            HasErrors |= error;
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

                        foreach (var diagnostic in Diagnostics) Parent.DiagnosticsBag.Add(diagnostic);
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

        #endregion IDisposable Support
    }
}