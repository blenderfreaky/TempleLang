namespace Program;

func _start() : long
{
    print("\n", 1);
    //printNum(fib(3));
    print("\n", 1);

    for let i = 0; i < 10; i++
    {
        printNum(i);
        print("\n", 1);
    }

    print("\n", 1);
}

func fib(num : long)
{
    let m1 = 0;
    let m2 = 1;

    for let i = 0; i < num; ++i
    {
        let temp = m1;
        m1 = m2;
        m2 = temp + m2;
    }

    return m1;
}

func printNum(num : long) : long
{
    if (num < 0)
    {
        print("-", 1);
        printNum(-num);
        return;
    }
    
    let digit = num % 10;
    let rest = num / 10;
    if (rest > 0) printNum(rest);
    printDigit(digit);
}

func printDigit(digit : long) : long
{
    let digits = "0123456789";
    print(digits + digit * 2, 1);
}

func print(ptr:long, len:long) : long
{
    let stdOut = GetStdHandle(-11);
    let numberOfCharsWritten = 0;
    return WriteConsoleW(stdOut, ptr, len, ref numberOfCharsWritten, 0);
}

func GetStdHandle(id : long) : long
    using "GetStdHandle"

func WriteConsoleW(
    stdOut : long, message : long, messageLength : long,
    numberOfCharsWritten : long, zero : long) : long
    using "WriteConsoleW"