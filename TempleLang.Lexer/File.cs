namespace TempleLang.Lexer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using TempleLang.Lexer.Abstractions;

    public class File : IFile
    {
        public string Name { get; }
        public string Path { get; }
    }
}
