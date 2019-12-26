using System;
using System.Collections.Generic;
using System.Text;
using TempleLang.Intermediate.Expressions;
using TempleLang.Intermediate.Primitives;

namespace TempleLang.Compiler.Abstractions
{
    public partial class Transformer
    {
        public IEnumerable<IInstruction> TransformValue(IValue value, IAssignableValue target) =>
            value switch
            {
                Constant<long> val => TransformValueCore(val, target),
                Local val => TransformValueCore(val, target),
                _ => throw new ArgumentException(nameof(value)),
            };

        public IEnumerable<IInstruction> TransformValueCore(Constant<long> val, IAssignableValue target)
        {
            var constant = new Constant(val.Value.ToString(), PrimitiveType.Long, "const long " + val.Value);

            ConstantTable.Add(constant);

            yield return new DirectAssignment(target, constant);
        }

        public IEnumerable<IInstruction> TransformValueCore(Local val, IAssignableValue target)
        {
            yield return new DirectAssignment(target, RequestUserLocal(val));
        }
    }
}
