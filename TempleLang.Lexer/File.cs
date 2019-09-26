namespace TempleLang.Lexer
{
    using TempleLang.Lexer.Abstractions;

    public class File : IFile
    {
        public string Name { get; }
        public string Path { get; }
    }
}
