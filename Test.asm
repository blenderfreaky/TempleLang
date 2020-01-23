section .data
FALSE: equ     0
TRUE: equ     1
ptrC0: dq      __utf16__(`Start\n`)
longC1: equ     6
longC2: equ     16
longC3: equ     2
longC4: equ     3
ptrC5: dq      __utf16__(`\nAll`)
longC6: equ     4
ptrC7: dq      __utf16__(`\n`)
longC8: equ     8
longC9: equ     7
longC10: equ     10
ptrC11: dq      __utf16__(`-`)
ptrC12: dq      __utf16__(`0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ`)
longC13: equ     11
section .text
    global  _start
    extern  HeapAlloc
    extern  HeapFree
    extern  GetProcessHeap
    extern  GetStdHandle
    extern  WriteConsoleW
_start: ; _start() : long
        sub     qword rsp, 48 ; Allocate stack
    
      ; _ = Call print(Start\n, 6)
        mov     qword rcx, qword ptrC0 ; Pass parameter #0
        mov     qword rdx, qword longC1 ; Pass parameter #1
        call    print
      ; /
    
      ; _ = Call arrTest()
        call    arrTest
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 48 ; Return stack
        ret     
    
arrTest: ; arrTest() : long
        sub     qword rsp, 80 ; Allocate stack
    
      ; _ = size Assign 16
        mov     qword r15, qword longC2
      ; /
    
      ; <>T1 = Call arrAlloc(size)
        mov     qword [rsp + 64], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r15 ; Pass parameter #0
        call    arrAlloc
        mov     qword r14, qword rax ; Assign return value to <>T1
        mov     qword r15, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
      ; _ = arr Assign <>T1
        mov     qword r13, qword r14
      ; /
    
      ; _ = i Assign 0
        mov     qword r14, qword FALSE
      ; /
    
      ; Jump .T3
        jmp     .T3
      ; /
    
      ; .T2:
    .T2:         
      ; /
    
      ; <>T5 = PreIncrement i
        inc     qword r14
      ; /
    
      ; .T3:
    .T3:         
      ; /
    
      ; <>T6 = i ComparisonLessThan size
        cmp     qword r14, qword r15 ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If !<>T6 Jump .T4
        test    qword r12, qword r12 ; Set condition codes according to condition
        jz      .T4 ; Jump if condition is false/zero
      ; /
    
      ; <>T7 = Call arrIndex(arr, i)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        mov     qword rdx, qword r14 ; Pass parameter #1
        call    arrIndex
        mov     qword r12, qword rax ; Assign return value to <>T7
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; _ = <>T7 ReferenceAssign 0
        mov     qword [r12], qword FALSE
      ; /
    
      ; Jump .T2
        jmp     .T2
      ; /
    
      ; .T4:
    .T4:         
      ; /
    
      ; <>T8 = Call arrIndex(arr, 0)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        call    arrIndex
        mov     qword r12, qword rax ; Assign return value to <>T8
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; _ = <>T8 ReferenceAssign 2
        mov     qword [r12], qword longC3
      ; /
    
      ; <>T9 = Call arrIndex(arr, 1)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    arrIndex
        mov     qword r12, qword rax ; Assign return value to <>T9
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; _ = <>T9 ReferenceAssign 3
        mov     qword [r12], qword longC4
      ; /
    
      ; _ = Call print(\nAll, 4)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword ptrC5 ; Pass parameter #0
        mov     qword rdx, qword longC6 ; Pass parameter #1
        call    print
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; _ = i Assign 0
        mov     qword r14, qword FALSE
      ; /
    
      ; Jump .T11
        jmp     .T11
      ; /
    
      ; .T10:
    .T10:         
      ; /
    
      ; <>T13 = PreIncrement i
        inc     qword r14
      ; /
    
      ; .T11:
    .T11:         
      ; /
    
      ; <>T14 = i ComparisonLessThan size
        cmp     qword r14, qword r15 ; Set condition codes according to operands
        jl      .CG2 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; /
    
      ; If !<>T14 Jump .T12
        test    qword r12, qword r12 ; Set condition codes according to condition
        jz      .T12 ; Jump if condition is false/zero
      ; /
    
      ; _ = Call print(\n, 1)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword ptrC7 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; <>T16 = Call arrIndex(arr, i)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        mov     qword rdx, qword r14 ; Pass parameter #1
        call    arrIndex
        mov     qword r12, qword rax ; Assign return value to <>T16
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; <>T15 = Dereference <>T16
        mov     qword r11, [r12] ; Dereference <>T16
      ; /
    
      ; _ = Call printNum(<>T15)
        mov     qword [rsp + 64], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 40], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r11 ; Pass parameter #0
        call    printNum
        mov     qword r11, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 48] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; Jump .T10
        jmp     .T10
      ; /
    
      ; .T12:
    .T12:         
      ; /
    
      ; _ = Call free(arr)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        call    free
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
      ; Return 0
        mov     qword rax, qword FALSE ; Return 0
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
arrAlloc: ; arrAlloc(size : long) : ptr
        sub     qword rsp, 64 ; Allocate stack
    
      ; size = param 0
        mov     qword r15, qword rcx ; Read parameter #0
      ; /
    
      ; <>T18 = size Multiply 8
        mov     qword r14, qword r15 ; Assign LHS to target memory
        imul    qword r14, qword longC8
      ; /
    
      ; <>T17 = Call alloc(<>T18)
        mov     qword [rsp + 48], qword r14 ; Store live variable onto stack
        mov     qword rcx, qword r14 ; Pass parameter #0
        call    alloc
        mov     qword r15, qword rax ; Assign return value to <>T17
        mov     qword r14, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; Return <>T17
        mov     qword rax, qword r15 ; Return <>T17
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
arrIndex: ; arrIndex(arr : ptr, index : long) : ptr
        sub     qword rsp, 48 ; Allocate stack
    
      ; arr = param 0
        mov     qword r15, qword rcx ; Read parameter #0
      ; /
    
      ; index = param 1
        mov     qword r14, qword rdx ; Read parameter #1
      ; /
    
      ; <>T20 = arr Add 7
        mov     qword r13, qword r15 ; Assign LHS to target memory
        add     qword r13, qword longC9
      ; /
    
      ; <>T21 = 8 Multiply index
        mov     qword r15, qword longC8 ; Assign LHS to target memory
        imul    qword r15, qword r14
      ; /
    
      ; <>T19 = <>T20 Add <>T21
        mov     qword r14, qword r13 ; Assign LHS to target memory
        add     qword r14, qword r15
      ; /
    
      ; Return <>T19
        mov     qword rax, qword r14 ; Return <>T19
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 48 ; Return stack
        ret     
    
alloc: ; alloc(size : long) : ptr
        sub     qword rsp, 64 ; Allocate stack
    
      ; size = param 0
        mov     qword r15, qword rcx ; Read parameter #0
      ; /
    
      ; <>T22 = Call GetProcessHeap()
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        call    GetProcessHeap
        mov     qword r14, qword rax ; Assign return value to <>T22
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; _ = heapHandle Assign <>T22
        mov     qword r13, qword r14
      ; /
    
      ; <>T23 = Call HeapAlloc(heapHandle, 0, size)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 40], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword r15 ; Pass parameter #2
        call    HeapAlloc
        mov     qword r14, qword rax ; Assign return value to <>T23
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; Return <>T23
        mov     qword rax, qword r14 ; Return <>T23
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
free: ; free(position : ptr) : bool
        sub     qword rsp, 64 ; Allocate stack
    
      ; position = param 0
        mov     qword r15, qword rcx ; Read parameter #0
      ; /
    
      ; <>T24 = Call GetProcessHeap()
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        call    GetProcessHeap
        mov     qword r14, qword rax ; Assign return value to <>T24
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; _ = heapHandle Assign <>T24
        mov     qword r13, qword r14
      ; /
    
      ; <>T25 = Call HeapFree(heapHandle, 0, position)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 40], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword r15 ; Pass parameter #2
        call    HeapFree
        mov     qword r14, qword rax ; Assign return value to <>T25
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; Return <>T25
        mov     qword rax, qword r14 ; Return <>T25
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; num = param 0
        mov     qword r15, qword rcx ; Read parameter #0
      ; /
    
      ; <>T26 = Call printNumAny(num, 10)
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r15 ; Pass parameter #0
        mov     qword rdx, qword longC10 ; Pass parameter #1
        call    printNumAny
        mov     qword r14, qword rax ; Assign return value to <>T26
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; Return <>T26
        mov     qword rax, qword r14 ; Return <>T26
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printNumAny: ; printNumAny(num : long, base : long) : long
        sub     qword rsp, 80 ; Allocate stack
    
      ; num = param 0
        mov     qword r15, qword rcx ; Read parameter #0
      ; /
    
      ; base = param 1
        mov     qword r14, qword rdx ; Read parameter #1
      ; /
    
      ; <>T27 = num ComparisonLessThan 0
        cmp     qword r15, qword FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r13, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r13, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If <>T27 Jump .T28
        test    qword r13, qword r13 ; Set condition codes according to condition
        jnz     .T28 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T29
        jmp     .T29
      ; /
    
      ; .T28:
    .T28:         
      ; /
    
      ; _ = Call print(-, 1)
        mov     qword [rsp + 64], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword rcx, qword ptrC11 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword r15, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; <>T30 = ArithmeticNegation num
        mov     qword r13, qword r15 ; Assign operand to target
        neg     qword r13
      ; /
    
      ; _ = Call printNum(<>T30)
        mov     qword [rsp + 64], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        call    printNum
        mov     qword r15, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; Return 
        jmp     .__exit
      ; /
    
      ; .T29:
    .T29:         
      ; /
    
      ; <>T31 = num Remainder base
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword r15 ; Assign LHS to dividend
        idiv    qword r14 ; Assign remainder to RDX, quotient to RAX
        mov     qword r13, qword rdx ; Assign result to target memory
      ; /
    
      ; _ = digit Assign <>T31
        mov     qword r12, qword r13
      ; /
    
      ; <>T32 = num Divide base
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword r15 ; Assign LHS to dividend
        idiv    qword r14 ; Assign remainder to RDX, quotient to RAX
        mov     qword r13, qword rax ; Assign result to target memory
      ; /
    
      ; _ = rest Assign <>T32
        mov     qword r15, qword r13
      ; /
    
      ; <>T33 = rest ComparisonGreaterThan 0
        cmp     qword r15, qword FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     qword r13, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r13, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; /
    
      ; If <>T33 Jump .T34
        test    qword r13, qword r13 ; Set condition codes according to condition
        jnz     .T34 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T35
        jmp     .T35
      ; /
    
      ; .T34:
    .T34:         
      ; /
    
      ; _ = Call printNumAny(rest, base)
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r14 ; Store live variable onto stack
        mov     qword rcx, qword r15 ; Pass parameter #0
        mov     qword rdx, qword r14 ; Pass parameter #1
        call    printNumAny
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; .T35:
    .T35:         
      ; /
    
      ; _ = Call printDigit(digit)
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack
        mov     qword rcx, qword r12 ; Pass parameter #0
        call    printDigit
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
printDigit: ; printDigit(digit : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; digit = param 0
        mov     qword r15, qword rcx ; Read parameter #0
      ; /
    
      ; _ = digits Assign 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
        mov     qword r14, qword ptrC12
      ; /
    
      ; <>T37 = digit Multiply 2
        mov     qword r13, qword r15 ; Assign LHS to target memory
        imul    qword r13, qword longC3
      ; /
    
      ; <>T36 = digits Add <>T37
        mov     qword r15, qword r14 ; Assign LHS to target memory
        add     qword r15, qword r13
      ; /
    
      ; _ = Call print(<>T36, 1)
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword r15 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
print: ; print(ptr : ptr, len : long) : long
        sub     qword rsp, 96 ; Allocate stack
    
      ; ptr = param 0
        mov     qword r15, qword rcx ; Read parameter #0
      ; /
    
      ; len = param 1
        mov     qword r14, qword rdx ; Read parameter #1
      ; /
    
      ; <>T39 = ArithmeticNegation 11
        mov     qword r13, qword longC13 ; Assign operand to target
        neg     qword r13
      ; /
    
      ; <>T38 = Call GetStdHandle(<>T39)
        mov     qword [rsp + 80], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 72], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 64], qword r14 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        call    GetStdHandle
        mov     qword r12, qword rax ; Assign return value to <>T38
        mov     qword r13, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 72] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
      ; _ = stdOut Assign <>T38
        mov     qword r13, qword r12
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
        mov     qword r12, qword FALSE
      ; /
    
      ; <>T41 = Reference numberOfCharsWritten
        lea     qword r11, [r12] ; Create reference to numberOfCharsWritten
      ; /
    
      ; <>T40 = Call WriteConsoleW(stdOut, ptr, len, <>T41, 0)
        mov     qword [rsp + 80], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 72], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 64], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r11 ; Store live variable onto stack
        mov     qword rcx, qword r13 ; Pass parameter #0
        mov     qword rdx, qword r15 ; Pass parameter #1
        mov     qword r8, qword r14 ; Pass parameter #2
        mov     qword r9, qword r11 ; Pass parameter #3
        mov     qword [rsp + 64], qword FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     qword r12, qword rax ; Assign return value to <>T40
        mov     qword r13, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 72] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r11, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; Return <>T40
        mov     qword rax, qword r12 ; Return <>T40
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 96 ; Return stack
        ret     
    
