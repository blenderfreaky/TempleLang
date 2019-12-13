namespace TempleLang.Intermediate
{
    using Intermediate.Statements;

    public struct ConstructorInfo : IMemberInfo
    {
        public MemberType MemberType => MemberType.Constructor;

        public MemberFlags MemberFlags { get; }

        public string Name { get; }

        public ITypeInfo ContainingType { get; }

        public IStatement EntryPoint { get; }

        public ConstructorInfo(MemberFlags memberFlags, string name, ITypeInfo containingType, IStatement entryPoint)
        {
            MemberFlags = memberFlags;
            Name = name;
            ContainingType = containingType;
            EntryPoint = entryPoint;
        }
    }
}
