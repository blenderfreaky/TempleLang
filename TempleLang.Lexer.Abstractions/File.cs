namespace TempleLang.Lexer
{
    using System.Collections.Generic;
    using TempleLang.Lexer.Abstractions;

    /// <inheritdoc/>
    public struct File : IFile
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Path { get; }

        public File(string name, string path)
        {
            Name = name;
            Path = path;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is File file && Name == file.Name && Path == file.Path;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 193482316;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Path);
            return hashCode;
        }

        public static bool operator ==(File left, File right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(File left, File right)
        {
            return !(left == right);
        }
    }
}
