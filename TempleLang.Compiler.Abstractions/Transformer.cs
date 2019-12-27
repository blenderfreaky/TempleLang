namespace TempleLang.Intermediate
{
    using Bound.Primitives;
    using System;
    using System.Collections.Generic;
    using TempleLang.Bound.Expressions;

    public partial class Transformer
    {
        public List<Constant> ConstantTable { get; }

        public Transformer()
        {
            ConstantTable = new List<Constant>();
        }

        private string RequestName() => Guid.NewGuid().ToString().Replace("-", "_");

        private Variable RequestLocal() =>
            new Variable(RequestName(), true);

        private LabelInstruction RequestLabel() =>
            new LabelInstruction(RequestName());

        private Variable RequestUserLocal(Local local) =>
            new Variable(local.Name, false);

        private IInstruction DirectAssignment(IAssignableValue target, IReadableValue source, PrimitiveType type) =>
            new BinaryComputationAssignment(target, target, source, BinaryOperatorType.Assign, type);
    }
}
