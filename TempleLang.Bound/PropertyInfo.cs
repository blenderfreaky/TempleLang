namespace TempleLang.Bound
{
    public struct PropertyInfo : IMemberInfo
    {
        public MemberType MemberType => MemberType.Property;

        public MemberFlags MemberFlags { get; }

        public string Name { get; }

        public ITypeInfo ContainingType { get; }

        public MethodInfo? GetMethod { get; }

        public MethodInfo? SetMethod { get; }

        public PropertyInfo(MemberFlags memberFlags, string name, ITypeInfo containingType, MethodInfo? getMethod, MethodInfo? setMethod)
        {
            MemberFlags = memberFlags;
            Name = name;
            ContainingType = containingType;
            GetMethod = getMethod;
            SetMethod = setMethod;
        }

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(PropertyInfo left, PropertyInfo right) => left.Equals(right);

        public static bool operator !=(PropertyInfo left, PropertyInfo right) => !(left == right);
    }
}