namespace TempleLang.Bound
{
    using System.Collections.Generic;

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

        public override bool Equals(object? obj) => obj is FieldInfo info && MemberType == info.MemberType && MemberFlags == info.MemberFlags && Name == info.Name && EqualityComparer<ITypeInfo>.Default.Equals(ContainingType, info.ContainingType);

        public override int GetHashCode()
        {
            var hashCode = 1341479176;
            hashCode = (hashCode * -1521134295) + MemberType.GetHashCode();
            hashCode = (hashCode * -1521134295) + MemberFlags.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ITypeInfo>.Default.GetHashCode(ContainingType);
            return hashCode;
        }

        public static bool operator ==(FieldInfo left, FieldInfo right) => left.Equals(right);

        public static bool operator !=(FieldInfo left, FieldInfo right) => !(left == right);
    }
}