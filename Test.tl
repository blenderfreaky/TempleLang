namespace Program;

proc _start() : long
{
    print("Start\n", 6);
    arrTest();
}

proc arrTest() : long
{
    let size = 16;
    let arr = arrAlloc(size);
    
    for (let i = 0; i < size; ++i)
    {
        arrIndex(arr, i) <- 0;
    }

    arrIndex(arr, 0) <- 2;
    arrIndex(arr, 1) <- 3;
    
    print("\nAll", 4);
    for (let i = 0; i < size; ++i)
    {
        print("\n", 1);
        printNum((long)deref arrIndex(arr, i));
    }

    free(arr);

    return 0;
}


proc arrAlloc(size : long) : ptr => alloc(size * 8);

proc arrIndex(arr : ptr, index : long) : ptr => arr + 7 + 8*index;

proc alloc(size : long) : ptr
{
    let heapHandle = GetProcessHeap();
    return HeapAlloc(heapHandle, 0, size);
}

proc free(position : ptr) : bool
{
    let heapHandle = GetProcessHeap();
    return HeapFree(heapHandle, 0, position);
}
        
proc HeapAlloc(
    heapHandle : ptr,
    flags : long,
    size : long) : ptr
    using "kernel32.lib";
        
proc HeapFree(
    heapHandle : ptr,
    flags : long,
    pos : ptr) : bool
    using "kernel32.lib";
    
proc GetProcessHeap() : ptr
    using "kernel32.lib";


proc printNum(num : long) : long => printNumAny(num, 10);

proc printNumAny(num : long, base : long) : long
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

proc printDigit(digit : long) : long
{
    let digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    //  ^~~~~~ This is a pointer

    //           v This is pointer arithmetic
    print(digits + digit * 2, 1);
    // * 2 because strings are UTF-16
}

proc print(ptr:ptr, len:long) : long
{
    let stdOut = GetStdHandle(-11);
    let numberOfCharsWritten = 0;
    return WriteConsoleW(stdOut, ptr, len, ref numberOfCharsWritten, 0);
}

proc GetStdHandle(id : long) : ptr
    using "kernel32.lib";

proc WriteConsoleW(
    stdOut : ptr,
    message : ptr,
    messageLength : long,
    numberOfCharsWritten : ptr,
    zero : long) : long
    using "kernel32.lib";
