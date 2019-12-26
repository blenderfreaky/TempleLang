using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TempleLang.Compiler.Abstractions.Values;
using TempleLang.Intermediate.Expressions;
using TempleLang.Intermediate.Primitives;
using TempleLang.Intermediate.Statements;

namespace TempleLang.Compiler.Abstractions
{
    public partial class Transformer
    {
        private List<Constant> ConstantTable { get; }

        public Transformer()
        {
            ConstantTable = new List<Constant>();
        }
        private NameValue RequestLocal() =>
            new NameValue(Guid.NewGuid().ToString().Replace("-", "_"), true);

        private NameValue RequestUserLocal(Local local) =>
            new NameValue(local.Name, false);
    }
}
