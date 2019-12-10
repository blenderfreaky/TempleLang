namespace TempleLang.AbstractASM
{
    using System.Collections;
    using System.Collections.Generic;

    public struct CodeBlock : IStackScope
    {
        public IStackScope ParentScope { get; }

        private Dictionary<string, IWriteableMemory> _locals { get; }
        public IReadOnlyDictionary<string, IWriteableMemory> Locals => _locals;

        public int StackSize { get; private set; }

        public CodeBlock(IStackScope parentScope)
        {
            ParentScope = parentScope;
            _locals = new Dictionary<string, IWriteableMemory>();
            StackSize = 0;
        }

        public void AddValue(IWriteableMemory local)
        {
            _locals.Add(local.DebugName, local);
            StackSize++;
        }

        public bool TryFindValue(string name, out IWriteableMemory valueReference)
        {
            var success = Locals.TryGetValue(name, out valueReference);
            return success || ParentScope.TryFindValue(name, out valueReference);
        }
    }
}
