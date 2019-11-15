using System;

namespace TempleLang.Parser
{
    public abstract class Literal : Expression
    {

    }

    public class IntLiteral : Literal
    {
        public readonly long Value;
        public readonly IntFlags Flags;
    }

    [Flags]
    public enum IntFlags
    {
        None = 0,

        Int8 = 1<<0,
        Int16 = 1<<1,
        Int32 = 1<<2,
        Int64 = 1<<3,

        Unsigned = 1<<4,
    }

    public class FloatLiteral : Literal
    {
        public readonly double Value;
        public readonly FloatFlags Flags;
    }

    [Flags]
    public enum FloatFlags
    {
        None = 0,
        Single = 1<<0,
        Double = 1<<1,
        Unsigned = 1<<2,
    }

    public class StringLiteral : Literal
    {
        public readonly string Value;
        public readonly StringFlags Flags;
    }

    [Flags]
    public enum StringFlags
    {
        None = 0,
    }
}