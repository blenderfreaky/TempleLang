namespace TempleLang.Compiler.NASM
{
    public struct LiteralParameter : IParameter
    {
        public string Text { get; }

        public LiteralParameter(string text) => Text = text;

        public string ToNASM() => Text;

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(LiteralParameter left, LiteralParameter right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LiteralParameter left, LiteralParameter right)
        {
            return !(left == right);
        }
    }
}
