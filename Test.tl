namespace Program;

func _start() : long
{
    let digits = "0123456789";
    
    print(digits + 0, 1);

    print("\n", 2);

    printNum(42);
}

func printNum(num : long) : long
{
    if (num < 0)
    {
        print("-", 1);
        num *= -1;
    }

    if (num == 0)
    {
        print("0", 1);
        return 0;
    }

    let digits = "0123456789";

    while (num > 0)
    {
        let remainder = num % 10;
        num /= 10;
        printNum(num);
        print(digits + remainder * 2, 1);
    }
}

func print(ptr:long, len:long) : long
{
    let stdOut = GetStdHandle(-11);
    let numberOfCharsWritten = 0;
    return WriteConsoleW(stdOut, ptr, len, ref numberOfCharsWritten, 0);
}

func GetStdHandle(id : long) : long using "GetStdHandle"

func WriteConsoleW(stdOut : long, message : long, messageLength : long, numberOfCharsWritten : long, zero : long) : long using "WriteConsoleW"