namespace TempleLang.Intermediate
{
    using TempleLang.Intermediate.Statements;

    public struct MethodInfo : IMemberInfo
    {
        public MemberType MemberType => MemberType.Method;

        public MemberFlags MemberFlags { get; }

        public string Name { get; }

        public ITypeInfo ContainingType { get; }

        public IStatement EntryPoint { get; }

        public MethodInfo(MemberFlags memberFlags, string name, ITypeInfo containingType, IStatement entryPoint)
        {
            MemberFlags = memberFlags;
            Name = name;
            ContainingType = containingType;
            EntryPoint = entryPoint;
        }
    }
}
