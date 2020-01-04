namespace TempleLang.Bound.Expressions
{
    using System.Collections.Generic;
    using TempleLang.Diagnostic;

    public struct CallableValue : IValue
    {
        public ICallable Callable { get; }
        ITypeInfo IExpression.ReturnType => Callable;

        public ValueFlags Flags { get; }
        public FileLocation Location { get; }

        public CallableValue(ICallable callable, ValueFlags flags, FileLocation location)
        {
            Callable = callable;
            Flags = flags;
            Location = location;
        }

        public override bool Equals(object? obj) => obj is CallableValue value && EqualityComparer<ICallable>.Default.Equals(Callable, value.Callable) && Flags == value.Flags && EqualityComparer<FileLocation>.Default.Equals(Location, value.Location);

        public override int GetHashCode()
        {
            var hashCode = -532820301;
            hashCode = (hashCode * -1521134295) + EqualityComparer<ICallable>.Default.GetHashCode(Callable);
            hashCode = (hashCode * -1521134295) + Flags.GetHashCode();
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(CallableValue left, CallableValue right) => left.Equals(right);

        public static bool operator !=(CallableValue left, CallableValue right) => !(left == right);
    }
}