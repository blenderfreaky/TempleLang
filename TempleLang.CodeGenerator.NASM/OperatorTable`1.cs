namespace TempleLang.CodeGenerator.NASM
{
    using Bound.Primitives;
    using System.Collections.Generic;

    internal class OperatorTable<T>
    {
        private readonly Dictionary<(T, PrimitiveType?), string> _operators = new Dictionary<(T, PrimitiveType?), string>();

        public string this[T op]
        {
            get => GetOperator(op, null);
            set => _operators[(op, null)] = value;
        }

        public string this[T op, PrimitiveType type]
        {
            get => GetOperator(op, type);
            set => _operators[(op, type)] = value;
        }

        private string GetOperator(T op, PrimitiveType? type)
        {
            if (_operators.TryGetValue((op, type), out var result))
            {
                return result;
            }

            // If the key doesn't exist, let it throw
            return _operators[(op, null)];
        }
    }
}