namespace TempleLang.Diagnostic
{
    using System;
    using System.Text;

    public struct DiagnosticInfo
    {
        public FileLocation? Location;
        public DiagnosticCode Code;

        public DiagnosticInfo(DiagnosticCode code, FileLocation? location)
        {
            Code = code;
            Location = location;
        }

        public override string ToString() => $"{Location}: {Code}";

        public string ToStringFancy(params string[] text)
        {
            if (Location == null) return ToString();

            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine(ToString());

            int pos = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (Location.Value.LastCharIndex < pos)
                {
                    break;
                }
                if (Location.Value.FirstCharIndex >= pos && Location.Value.FirstCharIndex < pos + text[i].Length)
                {
                    int start = Math.Max(0, Location.Value.FirstCharIndex - pos);
                    int end = Math.Min(Location.Value.LastCharIndex - pos, text[i].Length);

                    buffer.AppendLine(text[i]);
                    buffer.Append(new string(' ', start)).AppendLine(new string('^', end - start));
                }

                pos += text[i].Length;
            }

            return buffer.ToString();
        }
    }
}
