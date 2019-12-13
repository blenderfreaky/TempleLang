namespace TempleLang.Diagnostic
{
    public struct Positioned<T> : IPositioned
    {
        public T Value { get; }
        public FileLocation Location { get; }

        public Positioned(T value, FileLocation location)
        {
            Value = value;
            Location = location;
        }

        public override string ToString() => $"{Location}: {Value}";

        public static implicit operator T(Positioned<T> positioned) => positioned.Value;
    }
}
