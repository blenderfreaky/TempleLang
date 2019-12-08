using System;

namespace TempleLang.Compiler
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
    }
}
