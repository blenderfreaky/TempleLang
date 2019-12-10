using System;

namespace TempleLang.Diagnostic
{
    public struct DiagnosticInfo
    {
        public FileLocation? Location;
        public DiagnosticCode Code;
    }
}
