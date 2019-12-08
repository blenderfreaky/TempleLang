using System;

namespace TempleLang.Compiler
{

    public struct ConstructorInfo : IMemberInfo
    {
        public MemberType MemberType => MemberType.Constructor;

        public MemberFlags MemberFlags { get; }

        public string Name { get; }

        public ITypeInfo ContainingType { get; }

        public Procedure Method { get; }

        public ConstructorInfo(MemberFlags memberFlags, string name, ITypeInfo containingType, Procedure method)
        {
            MemberFlags = memberFlags;
            Name = name;
            ContainingType = containingType;
            Method = method;
        }
    }
}
