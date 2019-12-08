using System;

namespace TempleLang.Compiler
{

    public struct MethodInfo : IMemberInfo
    {
        public MemberType MemberType => MemberType.Method;

        public MemberFlags MemberFlags { get; }

        public string Name { get; }

        public ITypeInfo ContainingType { get; }

        public Procedure Method { get; }

        public MethodInfo(MemberFlags memberFlags, string name, ITypeInfo containingType, Procedure method)
        {
            MemberFlags = memberFlags;
            Name = name;
            ContainingType = containingType;
            Method = method;
        }
    }
}
