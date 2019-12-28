namespace TempleLang.Bound
{
    using Bound.Statements;
    using System.Collections.Generic;

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

        public override bool Equals(object? obj) => obj is ConstructorInfo info && MemberType == info.MemberType && MemberFlags == info.MemberFlags && Name == info.Name && EqualityComparer<ITypeInfo>.Default.Equals(ContainingType, info.ContainingType) && EqualityComparer<IStatement>.Default.Equals(EntryPoint, info.EntryPoint);

        public override int GetHashCode()
        {
            var hashCode = 552060675;
            hashCode = (hashCode * -1521134295) + MemberType.GetHashCode();
            hashCode = (hashCode * -1521134295) + MemberFlags.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ITypeInfo>.Default.GetHashCode(ContainingType);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IStatement>.Default.GetHashCode(EntryPoint);
            return hashCode;
        }

        public static bool operator ==(ConstructorInfo left, ConstructorInfo right) => left.Equals(right);

        public static bool operator !=(ConstructorInfo left, ConstructorInfo right) => !(left == right);
    }
}