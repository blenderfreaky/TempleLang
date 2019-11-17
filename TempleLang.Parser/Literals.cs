using System;

namespace TempleLang.Parser
{
    public abstract class Literal : Expression
    {
    }

    public class NullLiteral : Literal
    {
    }

    public class BoolLiteral : Literal
    {
        public readonly bool Value;

        public BoolLiteral(bool value) => Value = value;
    }

    public class IntLiteral : Literal
    {
        public readonly long Value;
        public readonly IntFlags Flags;

        public IntLiteral(long value, IntFlags flags)
        {
            Value = value;
            Flags = flags;
        }
    }

    [Flags]
    public enum IntFlags
    {
        None = 0,

        Int8 = 1 << 0,
        Int16 = 1 << 1,
        Int32 = 1 << 2,
        Int64 = 1 << 3,

        Unsigned = 1 << 4,
    }

    public class FloatLiteral : Literal
    {
        public readonly double Value;
        public readonly FloatFlags Flags;

        public FloatLiteral(double value, FloatFlags flags)
        {
            Value = value;
            Flags = flags;
        }
    }

    [Flags]
    public enum FloatFlags
    {
        None = 0,
        Single = 1 << 0,
        Double = 1 << 1,
        Unsigned = 1 << 2,
    }

    public class StringLiteral : Literal
    {
        public readonly string Value;

        public StringLiteral(string value)
        {
            Value = value;
        }
    }
}