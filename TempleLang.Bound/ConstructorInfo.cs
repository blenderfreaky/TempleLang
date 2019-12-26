namespace TempleLang.Bound
{
    using Bound.Statements;

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

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(ConstructorInfo left, ConstructorInfo right) => left.Equals(right);

        public static bool operator !=(ConstructorInfo left, ConstructorInfo right) => !(left == right);
    }
}
