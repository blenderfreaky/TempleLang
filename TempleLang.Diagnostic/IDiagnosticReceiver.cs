namespace TempleLang.Diagnostic
{
    using System.Collections.Concurrent;

    public interface IDiagnosticReceiver
    {
        void ReceiveDiagnostic(DiagnosticInfo diagnostic, bool isError);
    }
}
