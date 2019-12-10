namespace TempleLang.Compiler
{
    using System;

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
    }
}
