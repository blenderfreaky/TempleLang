namespace TempleLang.Intermediate
{
    using System;
    using System.Collections.Generic;
    using TempleLang.Bound.Expressions;

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
