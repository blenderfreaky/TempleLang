namespace TempleLang.Bound
{
    using TempleLang.Bound.Statements;

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

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(MethodInfo left, MethodInfo right) => left.Equals(right);

        public static bool operator !=(MethodInfo left, MethodInfo right) => !(left == right);
    }
}
