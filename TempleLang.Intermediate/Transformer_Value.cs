namespace TempleLang.Intermediate
{
    using System;
    using System.Collections.Generic;
    using TempleLang.Bound.Expressions;
    using TempleLang.Bound.Primitives;

    public partial class Transformer
    {
        public IEnumerable<IInstruction> TransformValue(IValue value, IAssignableValue target) =>
            value switch
            {
                Constant<long> val => TransformValueCore(val, target),
                Local val => TransformValueCore(val, target),
                _ => throw new ArgumentException(nameof(value)),
            };

        private IEnumerable<IInstruction> TransformValueCore(Constant<long> val, IAssignableValue target)
        {
            var constant = new Constant(val.Value.ToString(), PrimitiveType.Long, "const long " + val.Value);

            ConstantTable.Add(constant);

            yield return DirectAssignment(target, constant, PrimitiveType.Long);
        }

        private IEnumerable<IInstruction> TransformValueCore(Local val, IAssignableValue target)
        {
            if (!(val.ReturnType is PrimitiveType type)) throw new InvalidOperationException("Internal Failure: Binder failed binding high-level locals to primitives");

            yield return DirectAssignment(target, RequestUserLocal(val), type);
        }
    }
}
