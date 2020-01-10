namespace Program;

func _start() : long
{
    Sys.print("Start\n", 6);
    //printNum(fac(8));
    Sys.printNum(1);
}

func yeet() : long {}

func pow(base : long, exp : long) : long
{
    let acc = 1;

    for ; exp > 0; --exp
    {
        acc *= base;
    }

    return acc;
}

func fac(num : long) : long
{
    let acc = 1;

    for let i = 1; i <= num; ++i
    {
        acc *= i;
    }

    return acc;
}

func fib(num : long) : long
{
    if num == 0 return 0;

    let m1 = 0;
    let m2 = 1;

    for let i = 0; i < num-1; ++i
    {
        let temp = m1;
        m1 = m2;
        m2 = temp + m2;
    }

    return m2;
}

namespace Sys
{
    func printNum(num : long) : long => printNumAny(num, 10);

    func printNumAny(num : long, base : long) : long
    {
        if (num < 0)
        {
            print("-", 1);
            printNum(-num);
            return;
        }
    
        let digit = num % base;
        let rest = num / base;
        if (rest > 0) printNumAny(rest, base);
        printDigit(digit);
    }

    func printDigit(digit : long) : long
    {
        let digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        // * 2 because strings are UTF-16
        print(digits + digit * 2, 1);
    }

    func print(ptr:ptr, len:long) : long
    {
        let stdOut = GetStdHandle(-11);
        let numberOfCharsWritten = 0;
        return WriteConsoleW(stdOut, ptr, len, ref numberOfCharsWritten, 0);
    }

    func GetStdHandle(id : long) : ptr
        using "kernel32.lib";

    func WriteConsoleW(
        stdOut : ptr,
        message : ptr,
        messageLength : long,
        numberOfCharsWritten : ptr,
        zero : long) : long
        using "kernel32.lib";
}