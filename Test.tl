namespace Program;

let _start() : long
{
    let a = 1;
    if (a - 1)
    {
        print("a - 1 != 0", 10);
    }
    else
    {
        print("a - 1 == 0", 10);
    }
}

let print(ptr:long, len:long) : long
{
    let stdOut = GetStdHandle(-11);
    let numberOfCharsWritten = 0;
    return WriteConsoleW(stdOut, ptr, len, ref numberOfCharsWritten, 0);
}

let GetStdHandle(id : long) : long using "GetStdHandle"

let WriteConsoleW(stdOut : long, message : long, messageLength : long, numberOfCharsWritten : long, zero : long) : long using "WriteConsoleW"