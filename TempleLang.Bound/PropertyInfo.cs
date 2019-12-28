namespace TempleLang.Bound
{
    using System.Collections.Generic;

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

        public override bool Equals(object? obj) => obj is PropertyInfo info && MemberType == info.MemberType && MemberFlags == info.MemberFlags && Name == info.Name && EqualityComparer<ITypeInfo>.Default.Equals(ContainingType, info.ContainingType) && EqualityComparer<MethodInfo?>.Default.Equals(GetMethod, info.GetMethod) && EqualityComparer<MethodInfo?>.Default.Equals(SetMethod, info.SetMethod);

        public override int GetHashCode()
        {
            var hashCode = 93506792;
            hashCode = (hashCode * -1521134295) + MemberType.GetHashCode();
            hashCode = (hashCode * -1521134295) + MemberFlags.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ITypeInfo>.Default.GetHashCode(ContainingType);
            hashCode = (hashCode * -1521134295) + GetMethod.GetHashCode();
            hashCode = (hashCode * -1521134295) + SetMethod.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(PropertyInfo left, PropertyInfo right) => left.Equals(right);

        public static bool operator !=(PropertyInfo left, PropertyInfo right) => !(left == right);
    }
}