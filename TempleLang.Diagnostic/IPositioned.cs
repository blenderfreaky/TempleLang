using System;
using System.Collections.Generic;
using System.Text;

namespace TempleLang.Diagnostic
{
    public interface IPositioned
    {
        FileLocation Location { get; }
    }
}
