using System;
using System.Collections.Generic;
using System.Linq;

namespace TempleLang.Compiler.NASM
{
    public enum RegisterName
    {
        // 64-bit integer
        R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11, R12, R13, R14, R15,
        RAX, RCX, RDX, RBX,
        RSP, RBP, RSI, RDI, RIP,

        // 32-bit integer
        R0D, R1D, R2D, R3D, R4D, R5D, R6D, R7D, R8D, R9D, R10D, R11D, R12D, R13D, R14D, R15D,
        EAX, ECX, EDX, EBX,
        ESP, EBP, ESI, EDI, EIP,

        // 16-bit integer
        R0W, R1W, R2W, R3W, R4W, R5W, R6W, R7W, R8W, R9W, R10W, R11W, R12W, R13W, R14W, R15W,
        AX, CX, DX, BX,
        SP, BP, SI, DI, IP,

        // 8-bit integer
        R0B, R1B, R2B, R3B, R4B, R5B, R6B, R7B, R8B, R9B, R10B, R11B, R12B, R13B, R14B, R15B,
        AL, AH, BL, BH, CL, CH, DL, DH,
        SPL, BPL, SIL, DIL
    }

    public readonly struct Register
    {
        public readonly RegisterName RegisterName;
        public readonly RegisterKind Kind;
        public readonly RegisterFlags Flags;
        public readonly RegisterSize Size;
        public readonly RegisterName? Containing;

        public readonly string Name => RegisterName.ToString();

        private static readonly Dictionary<RegisterName, Register> Registers = new[]
        {
            new Register(RegisterName.RAX, RegisterKind.Accumulator, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.EAX, RegisterKind.Accumulator, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.RAX),
            new Register(RegisterName.AX,  RegisterKind.Accumulator, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.EAX),
            new Register(RegisterName.AH,  RegisterKind.Accumulator, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.AX),
            new Register(RegisterName.AL,  RegisterKind.Accumulator, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.AX),

            new Register(RegisterName.RBX, RegisterKind.Base,        RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes8),
            new Register(RegisterName.EBX, RegisterKind.Base,        RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes4, RegisterName.RBX),
            new Register(RegisterName.BX,  RegisterKind.Base,        RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes2, RegisterName.EBX),
            new Register(RegisterName.BH,  RegisterKind.Base,        RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes1, RegisterName.BX),
            new Register(RegisterName.BL,  RegisterKind.Base,        RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes1, RegisterName.BX),

            new Register(RegisterName.RCX, RegisterKind.Counter,     RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.ECX, RegisterKind.Counter,     RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.RCX),
            new Register(RegisterName.CX,  RegisterKind.Counter,     RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.ECX),
            new Register(RegisterName.CH,  RegisterKind.Counter,     RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.CX),
            new Register(RegisterName.CL,  RegisterKind.Counter,     RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.CX),

            new Register(RegisterName.RDX, RegisterKind.Data,        RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.EDX, RegisterKind.Data,        RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.RDX),
            new Register(RegisterName.DX,  RegisterKind.Data,        RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.EDX),
            new Register(RegisterName.DH,  RegisterKind.Data,        RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.DX),
            new Register(RegisterName.DL,  RegisterKind.Data,        RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.DX),

            new Register(RegisterName.RSP, RegisterKind.StackPointer,     RegisterFlags.Preserved, RegisterSize.Bytes8),
            new Register(RegisterName.RSP, RegisterKind.StackBasePointer, RegisterFlags.Preserved, RegisterSize.Bytes8),

#region 64-Bit General Purpose Rn 
            new Register(RegisterName.R0,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R1,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R2,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R3,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R4,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R5,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R6,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R7,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R8,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R9,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R10, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R11, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes8),
            new Register(RegisterName.R12, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes8),
            new Register(RegisterName.R13, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes8),
            new Register(RegisterName.R14, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes8),
            new Register(RegisterName.R15, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes8),
#endregion

#region 32-Bit General Purpose RnD
            new Register(RegisterName.R0D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R0 ),
            new Register(RegisterName.R1D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R1 ),
            new Register(RegisterName.R2D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R2 ),
            new Register(RegisterName.R3D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R3 ),
            new Register(RegisterName.R4D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R4 ),
            new Register(RegisterName.R5D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R5 ),
            new Register(RegisterName.R6D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R6 ),
            new Register(RegisterName.R7D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R7 ),
            new Register(RegisterName.R8D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R8 ),
            new Register(RegisterName.R9D,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R9 ),
            new Register(RegisterName.R10D, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R10),
            new Register(RegisterName.R11D, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes4, RegisterName.R11),
            new Register(RegisterName.R12D, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes4, RegisterName.R12),
            new Register(RegisterName.R13D, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes4, RegisterName.R13),
            new Register(RegisterName.R14D, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes4, RegisterName.R14),
            new Register(RegisterName.R15D, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes4, RegisterName.R15),
#endregion                                            

#region 16-Bit General Purpose RnW/RnB 
            new Register(RegisterName.R0W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R0D),
            new Register(RegisterName.R1W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R1D),
            new Register(RegisterName.R2W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R2D),
            new Register(RegisterName.R3W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R3D),
            new Register(RegisterName.R4W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R4D),
            new Register(RegisterName.R5W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R5D),
            new Register(RegisterName.R6W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R6D),
            new Register(RegisterName.R7W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R7D),
            new Register(RegisterName.R8W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R8D),
            new Register(RegisterName.R9W,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R9D),
            new Register(RegisterName.R10W, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R10D),
            new Register(RegisterName.R11W, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes2, RegisterName.R11D),
            new Register(RegisterName.R12W, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes2, RegisterName.R12D),
            new Register(RegisterName.R13W, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes2, RegisterName.R13D),
            new Register(RegisterName.R14W, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes2, RegisterName.R14D),
            new Register(RegisterName.R15W, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes2, RegisterName.R15D),

            new Register(RegisterName.R0B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R0W),
            new Register(RegisterName.R1B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R1W),
            new Register(RegisterName.R2B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R2W),
            new Register(RegisterName.R3B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R3W),
            new Register(RegisterName.R4B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R4W),
            new Register(RegisterName.R5B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R5W),
            new Register(RegisterName.R6B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R6W),
            new Register(RegisterName.R7B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R7W),
            new Register(RegisterName.R8B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R8W),
            new Register(RegisterName.R9B,  RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R9W),
            new Register(RegisterName.R10B, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R10W),
            new Register(RegisterName.R11B, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose, RegisterSize.Bytes1, RegisterName.R11W),
            new Register(RegisterName.R12B, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes1, RegisterName.R12W),
            new Register(RegisterName.R13B, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes1, RegisterName.R13W),
            new Register(RegisterName.R14B, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes1, RegisterName.R14W),
            new Register(RegisterName.R15B, RegisterKind.StrictlyGeneralPurpose, RegisterFlags.GeneralPurpose | RegisterFlags.Preserved, RegisterSize.Bytes1, RegisterName.R15W),
#endregion
        }
        .ToDictionary(x => x.RegisterName, x => x);

        public static Register Get(RegisterName name) => Registers[name];

        private Register(RegisterName register, RegisterKind kind, RegisterFlags flags, RegisterSize size, RegisterName? containing = null)
        {
            RegisterName = register;
            Kind = kind;
            Flags = flags;
            Size = size;
            Containing = containing;
        }

        public override bool Equals(object? obj) => obj is Register register && RegisterName == register.RegisterName && Kind == register.Kind && Flags == register.Flags && Size == register.Size && Containing == register.Containing && Name == register.Name;

        public override int GetHashCode()
        {
            var hashCode = -1235707855;
            hashCode = (hashCode * -1521134295) + RegisterName.GetHashCode();
            hashCode = (hashCode * -1521134295) + Kind.GetHashCode();
            hashCode = (hashCode * -1521134295) + Flags.GetHashCode();
            hashCode = (hashCode * -1521134295) + Size.GetHashCode();
            hashCode = (hashCode * -1521134295) + Containing.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public static bool operator ==(Register left, Register right) => left.Equals(right);

        public static bool operator !=(Register left, Register right) => !(left == right);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1027:Mark enums with FlagsAttribute", Justification = "Not flags")]
    public enum RegisterSize
    {
        Bytes1 = 1,
        Bytes2 = 2,
        Bytes4 = 4,
        Bytes8 = 8,
    }

    public enum RegisterKind
    {
        StrictlyGeneralPurpose,
        Accumulator,
        Base,
        Counter,
        Data,
        StackPointer,
        StackBasePointer,
        SourceIndex,
        DestinationIndex,
        FloatingPoint,
        Flags,
        InstructionPointer,
        SIMD,
    }

    [Flags]
    public enum RegisterFlags
    {
        None = 0,
        GeneralPurpose = 1 << 1,
        Preserved = 1 << 2,
    }
}
