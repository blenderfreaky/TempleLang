using System;
using System.Collections.Generic;
using System.Text;

namespace TempleLang.Bound.Expressions
{
    public struct NamespaceValue : IValue
    {
        public ValueFlags Flags => throw new NotImplementedException();

        public ITypeInfo ReturnType => throw new NotImplementedException();
    }
}
