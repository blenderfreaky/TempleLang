namespace TempleLang.Bound.Declarations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TempleLang.Diagnostic;

    public struct NamespaceDeclaration : IDeclaration
    {
        public Positioned<string> Name { get; }
        public IReadOnlyList<IDeclaration> Declarations { get; }
        public FileLocation Location { get; }

        public NamespaceDeclaration(Positioned<string> name, IReadOnlyList<IDeclaration> declarations, FileLocation location)
        {
            Name = name;
            Declarations = declarations;
            Location = location;
        }

        public override string ToString() =>
            "namespace " + Name + Environment.NewLine
            + "{" + Environment.NewLine
            + string.Join(
                  Environment.NewLine,
                  Declarations.Select(x => string.Concat(x.ToString().Split('\n').Select(x => "    " + x + Environment.NewLine))))
            + "}";

        public override bool Equals(object? obj) => obj is NamespaceDeclaration declaration && Name == declaration.Name && EqualityComparer<IReadOnlyList<IDeclaration>>.Default.Equals(Declarations, declaration.Declarations) && EqualityComparer<FileLocation>.Default.Equals(Location, declaration.Location);

        public override int GetHashCode()
        {
            var hashCode = 100594572;
            hashCode = (hashCode * -1521134295) + EqualityComparer<Positioned<string>>.Default.GetHashCode(Name);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IReadOnlyList<IDeclaration>>.Default.GetHashCode(Declarations);
            hashCode = (hashCode * -1521134295) + Location.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(NamespaceDeclaration left, NamespaceDeclaration right) => left.Equals(right);

        public static bool operator !=(NamespaceDeclaration left, NamespaceDeclaration right) => !(left == right);
    }
}
