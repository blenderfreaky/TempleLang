namespace TempleLang.Parser.Abstractions
{
    public readonly struct SeparatedList<T>
    {
        public readonly T[] Elements;

        public SeparatedList(T[] elements)
        {
            Elements = elements;
        }

        public static SeparatedList<T> Parse()
        {

        }
    }
}