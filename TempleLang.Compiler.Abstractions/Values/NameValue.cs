using System;
using System.Collections.Generic;
using System.Text;

namespace TempleLang.Compiler.Abstractions.Values
{
    public struct NameValue : IReadableValue, IAssignableValue
    {
        public string Name { get; }
        public bool IsCompilerOwned { get; }

        public string DisplayName => IsCompilerOwned ? "<>" + Name : Name;

        public NameValue(string name, bool isCompilerOwned)
        {
            Name = name;
            IsCompilerOwned = isCompilerOwned;
        }

        public override bool Equals(object? obj) =>
            obj is NameValue value
            && Name == value.Name
            && IsCompilerOwned == value.IsCompilerOwned
            && DisplayName == value.DisplayName;

        public override int GetHashCode()
        {
            var hashCode = -1229624999;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + IsCompilerOwned.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            return hashCode;
        }

        public static bool operator ==(NameValue left, NameValue right) => left.Equals(right);

        public static bool operator !=(NameValue left, NameValue right) => !(left == right);
    }
}
