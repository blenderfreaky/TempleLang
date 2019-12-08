using System.Collections;
using System.Collections.Generic;

namespace TempleLang.Compiler
{
    public sealed class Procedure
    {
        public IEnumerable<IStatement> Instructions { get; }

        public ITypeInfo ReturnType { get; }

        public IList<ParameterInfo> Parameters { get; }

        public IScope Scope { get; }


    }

    public interface IScope
    {
        bool TryFindValue(string name, out IValueReference valueReference);
    }

    public struct Local : IValueReference
    {
        public bool IsAssignable { get; }

        public Procedure Assign { get; }
    }

    public sealed class LocalScope : IScope
    {
        private Dictionary<string, Local> Locals { get; }

        public bool FindValue(string name, out IValueReference valueReference)
        {
            var retVal = Locals.TryGetValue(name, out var local);
            // Out parameters do not like implicit casts
            valueReference = local;
            return retVal;
        }
    }

    public struct ParameterInfo
    {
        public ITypeInfo TypeInfo { get; }
        public string Name { get; }
    }

    public interface IStatement
    {
    }

    public struct ScopeStatement : IStatement
    {
        public Procedure Procedure { get; }
    }

    public struct ReturnStatement : IStatement
    {
        public IValueReference
    }

    public struct BinaryStatement : IStatement
    {
        public IValueReference Lhs { get; }
        public IValueReference Rhs { get; }
    }

    public interface IValueReference
    {
        bool IsAssignable { get; }

        Procedure Assign { get; }
    }

    public struct Constant : IValueReference
    {
        public bool IsAssignable => false;


    }
}
