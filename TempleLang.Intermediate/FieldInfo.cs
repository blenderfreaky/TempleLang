namespace TempleLang.Intermediate
{
    public struct FieldInfo : IMemberInfo
    {
        public MemberType MemberType => MemberType.Field;

        public MemberFlags MemberFlags { get; }

        public string Name { get; }

        public ITypeInfo ContainingType { get; }

        public FieldInfo(MemberFlags memberFlags, string name, ITypeInfo containingType)
        {
            MemberFlags = memberFlags;
            Name = name;
            ContainingType = containingType;
        }

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(FieldInfo left, FieldInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FieldInfo left, FieldInfo right)
        {
            return !(left == right);
        }
    }
}
