namespace TempleLang.Intermediate
{
    using System.Collections.Generic;

    public struct DirectAssignment : IAssignment
    {
        public IAssignableValue Target { get; }
        public IReadableValue Source { get; }

        public DirectAssignment(IAssignableValue target, IReadableValue source)
        {
            Target = target;
            Source = source;
        }

        public override bool Equals(object? obj) => obj is DirectAssignment assignment && EqualityComparer<IAssignableValue>.Default.Equals(Target, assignment.Target) && EqualityComparer<IReadableValue>.Default.Equals(Source, assignment.Source);

        public override int GetHashCode()
        {
            var hashCode = 1748489632;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IAssignableValue>.Default.GetHashCode(Target);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadableValue>.Default.GetHashCode(Source);
            return hashCode;
        }

        public static bool operator ==(DirectAssignment left, DirectAssignment right) => left.Equals(right);

        public static bool operator !=(DirectAssignment left, DirectAssignment right) => !(left == right);
    }
}
