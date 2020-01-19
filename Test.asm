section .data
FALSE: equ     0
TRUE: equ     1
ptrC0: dq      __utf16__(`Start\n`)
longC1: equ     6
longC2: equ     16
longC3: equ     8
longC4: equ     2
longC5: equ     3
ptrC6: dq      __utf16__(`\nAll`)
longC7: equ     4
ptrC8: dq      __utf16__(`\n`)
longC9: equ     7
longC10: equ     10
ptrC11: dq      __utf16__(`-`)
ptrC12: dq      __utf16__(`0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ`)
longC13: equ     11
section .text
    global  _start
    extern  GetStdHandle
    extern  WriteConsoleW
    extern  HeapAlloc
    extern  HeapFree
    extern  GetProcessHeap
_start: ; _start() : long
        sub     rsp, 40 ; Allocate stack
    
      ; _ = call print(Start\n, 6)
        mov     rcx, ptrC0 ; Pass parameter #0
        mov     rdx, longC1 ; Pass parameter #1
        call    print
      ; /
    
      ; _ = call arrTest()
        call    arrTest
      ; /
    
      ; _ = call arrTest()
        call    arrTest
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 40 ; Return stack
        ret     
    
arrTest: ; arrTest() : long
        sub     rsp, 216 ; Allocate stack
    
      ; _ = size Assign 16
        mov     qword [rsp + 48], longC2
      ; /
    
      ; <>T2 = size Multiply 8
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        imul    rbx, longC3
        mov     qword [rsp + 80], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T1 = call alloc(<>T2)
        mov     rcx, qword [rsp + 80] ; Pass parameter #0
        call    alloc
        mov     qword [rsp + 72], rax ; Assign return value to <>T1
      ; /
    
      ; _ = arr Assign <>T1
        mov     rbx, qword [rsp + 72] ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; _ = i Assign 0
        mov     qword [rsp + 32], FALSE
      ; /
    
      ; Jump .T4
        jmp     .T4
      ; /
    
      ; .T3:
    .T3:         
      ; /
    
      ; <>T6 = PreIncrement i
        inc     qword [rsp + 32]
      ; /
    
      ; .T4:
    .T4:         
      ; /
    
      ; <>T7 = i ComparisonLessThan size
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        cmp     rbx, qword [rsp + 48] ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG1:          ; Exit
        mov     qword [rsp + 88], rbx ; Assign result to actual target memory
      ; /
    
      ; If !<>T7 Jump .T5
        mov     rbx, qword [rsp + 88] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jz      .T5 ; Jump if condition is false/zero
      ; /
    
      ; <>T8 = call arrIndex(arr, i)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        mov     rdx, qword [rsp + 32] ; Pass parameter #1
        call    arrIndex
        mov     qword [rsp + 16], rax ; Assign return value to <>T8
      ; /
    
      ; _ = <>T8 ReferenceAssign 0
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        mov     qword [rbx], FALSE
        mov     qword [rsp + 16], rbx ; Assign result to actual target memory
      ; /
    
      ; Jump .T3
        jmp     .T3
      ; /
    
      ; .T5:
    .T5:         
      ; /
    
      ; <>T9 = call arrIndex(arr, 0)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        mov     rdx, FALSE ; Pass parameter #1
        call    arrIndex
        mov     qword [rsp + 64], rax ; Assign return value to <>T9
      ; /
    
      ; _ = <>T9 ReferenceAssign 2
        mov     rbx, qword [rsp + 64] ; Move RHS into register so operation is possible
        mov     qword [rbx], longC4
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T10 = call arrIndex(arr, 1)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    arrIndex
        mov     qword [rsp + 56], rax ; Assign return value to <>T10
      ; /
    
      ; _ = <>T10 ReferenceAssign 3
        mov     rbx, qword [rsp + 56] ; Move RHS into register so operation is possible
        mov     qword [rbx], longC5
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call print(\nAll, 4)
        mov     rcx, ptrC6 ; Pass parameter #0
        mov     rdx, longC7 ; Pass parameter #1
        call    print
      ; /
    
      ; _ = i Assign 0
        mov     qword [rsp + 32], FALSE
      ; /
    
      ; Jump .T12
        jmp     .T12
      ; /
    
      ; .T11:
    .T11:         
      ; /
    
      ; <>T14 = PreIncrement i
        inc     qword [rsp + 32]
      ; /
    
      ; .T12:
    .T12:         
      ; /
    
      ; <>T15 = i ComparisonLessThan size
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        cmp     rbx, qword [rsp + 48] ; Set condition codes according to operands
        jl      .CG2 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG3:          ; Exit
        mov     qword [rsp + 96], rbx ; Assign result to actual target memory
      ; /
    
      ; If !<>T15 Jump .T13
        mov     rbx, qword [rsp + 96] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jz      .T13 ; Jump if condition is false/zero
      ; /
    
      ; _ = call print(\n, 1)
        mov     rcx, ptrC8 ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; <>T17 = call arrIndex(arr, i)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        mov     rdx, qword [rsp + 32] ; Pass parameter #1
        call    arrIndex
        mov     qword [rsp + 40], rax ; Assign return value to <>T17
      ; /
    
      ; <>T16 = Dereference <>T17
        mov     rbx, qword [rsp + 40] ; Move RHS into register so operation is possible
        mov     rbx, [rbx] ; Dereference <>T17
        mov     qword [rsp + 104], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call printNum(<>T16)
        mov     rcx, qword [rsp + 104] ; Pass parameter #0
        call    printNum
      ; /
    
      ; Jump .T11
        jmp     .T11
      ; /
    
      ; .T13:
    .T13:         
      ; /
    
      ; _ = call free(arr)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        call    free
      ; /
    
      ; Return 0
        mov     rax, FALSE ; Return 0
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 216 ; Return stack
        ret     
    
arrIndex: ; arrIndex(arr : ptr, index : long) : ptr
        sub     rsp, 56 ; Allocate stack
    
      ; arr = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; index = param 1
        mov     qword [rsp + 24], rdx ; Read parameter #1
      ; /
    
      ; <>T18 = arr Add 7
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        add     rbx, longC9
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T19 = 8 Multiply index
        mov     rbx, longC3 ; Move RHS into register so operation is possible
        imul    rbx, qword [rsp + 24]
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = <>T18 Add <>T19
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 56 ; Return stack
        ret     
    
alloc: ; alloc(size : long) : ptr
        sub     rsp, 112 ; Allocate stack
    
      ; size = param 0
        mov     qword [rsp + 32], rcx ; Read parameter #0
      ; /
    
      ; <>T20 = call GetProcessHeap()
        call    GetProcessHeap
        mov     qword [rsp + 40], rax ; Assign return value to <>T20
      ; /
    
      ; _ = heapHandle Assign <>T20
        mov     rbx, qword [rsp + 40] ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; <>T21 = call HeapAlloc(heapHandle, 0, size)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        mov     rdx, FALSE ; Pass parameter #1
        mov     r8, qword [rsp + 32] ; Pass parameter #2
        call    HeapAlloc
        mov     qword [rsp + 16], rax ; Assign return value to <>T21
      ; /
    
      ; Return <>T21
        mov     rax, qword [rsp + 16] ; Return <>T21
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 112 ; Return stack
        ret     
    
free: ; free(position : ptr) : bool
        sub     rsp, 112 ; Allocate stack
    
      ; position = param 0
        mov     qword [rsp + 32], rcx ; Read parameter #0
      ; /
    
      ; <>T22 = call GetProcessHeap()
        call    GetProcessHeap
        mov     qword [rsp + 40], rax ; Assign return value to <>T22
      ; /
    
      ; _ = heapHandle Assign <>T22
        mov     rbx, qword [rsp + 40] ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; <>T23 = call HeapFree(heapHandle, 0, position)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        mov     rdx, FALSE ; Pass parameter #1
        mov     r8, qword [rsp + 32] ; Pass parameter #2
        call    HeapFree
        mov     qword [rsp + 16], rax ; Assign return value to <>T23
      ; /
    
      ; Return <>T23
        mov     rax, qword [rsp + 16] ; Return <>T23
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 112 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     rsp, 72 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; _ = call printNumAny(num, 10)
        mov     rcx, qword [rsp + 16] ; Pass parameter #0
        mov     rdx, longC10 ; Pass parameter #1
        call    printNumAny
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 72 ; Return stack
        ret     
    
printNumAny: ; printNumAny(num : long, base : long) : long
        sub     rsp, 168 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp + 32], rcx ; Read parameter #0
      ; /
    
      ; base = param 1
        mov     qword [rsp + 40], rdx ; Read parameter #1
      ; /
    
      ; <>T24 = num ComparisonLessThan 0
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG1:          ; Exit
        mov     qword [rsp + 48], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T24 Jump .T25
        mov     rbx, qword [rsp + 48] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T25 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T26
        jmp     .T26
      ; /
    
      ; .T25:
    .T25:         
      ; /
    
      ; _ = call print(-, 1)
        mov     rcx, ptrC11 ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; <>T27 = Negation num
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call printNum(<>T27)
        mov     rcx, qword [rsp + 56] ; Pass parameter #0
        call    printNum
      ; /
    
      ; Return 
        jmp     .__exit
      ; /
    
      ; .T26:
    .T26:         
      ; /
    
      ; <>T28 = num Remainder base
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp + 32] ; Assign LHS to dividend
        mov     rbx, qword [rsp + 40] ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rdx ; Assign result to target memory
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = digit Assign <>T28
        mov     rbx, qword [rsp + 64] ; Move RHS into register so operation is possible
        mov     qword [rsp + 16], rbx
      ; /
    
      ; <>T29 = num Divide base
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp + 32] ; Assign LHS to dividend
        mov     rbx, qword [rsp + 40] ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rax ; Assign result to target memory
        mov     qword [rsp + 72], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = rest Assign <>T29
        mov     rbx, qword [rsp + 72] ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; <>T30 = rest ComparisonGreaterThan 0
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG3:          ; Exit
        mov     qword [rsp + 80], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T30 Jump .T31
        mov     rbx, qword [rsp + 80] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T31 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T32
        jmp     .T32
      ; /
    
      ; .T31:
    .T31:         
      ; /
    
      ; _ = call printNumAny(rest, base)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        mov     rdx, qword [rsp + 40] ; Pass parameter #1
        call    printNumAny
      ; /
    
      ; .T32:
    .T32:         
      ; /
    
      ; _ = call printDigit(digit)
        mov     rcx, qword [rsp + 16] ; Pass parameter #0
        call    printDigit
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 168 ; Return stack
        ret     
    
printDigit: ; printDigit(digit : long) : long
        sub     rsp, 88 ; Allocate stack
    
      ; digit = param 0
        mov     qword [rsp + 24], rcx ; Read parameter #0
      ; /
    
      ; _ = digits Assign 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
        mov     rbx, ptrC12 ; Move RHS into register so operation is possible
        mov     qword [rsp + 16], rbx
      ; /
    
      ; <>T34 = digit Multiply 2
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        imul    rbx, longC4
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T33 = digits Add <>T34
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        add     rbx, qword [rsp + 32]
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call print(<>T33, 1)
        mov     rcx, qword [rsp + 40] ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 88 ; Return stack
        ret     
    
print: ; print(ptr : ptr, len : long) : long
        sub     rsp, 192 ; Allocate stack
    
      ; ptr = param 0
        mov     qword [rsp + 32], rcx ; Read parameter #0
      ; /
    
      ; len = param 1
        mov     qword [rsp + 56], rdx ; Read parameter #1
      ; /
    
      ; <>T36 = Negation 11
        mov     rbx, longC13 ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T35 = call GetStdHandle(<>T36)
        mov     rcx, qword [rsp + 64] ; Pass parameter #0
        call    GetStdHandle
        mov     qword [rsp + 48], rax ; Assign return value to <>T35
      ; /
    
      ; _ = stdOut Assign <>T35
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
        mov     qword [rsp + 40], FALSE
      ; /
    
      ; <>T38 = Reference numberOfCharsWritten
        mov     qword [rsp + 72], rsp ; Assign stackpointer
        add     qword [rsp + 72], 40 ; and subtract to get correct position
      ; /
    
      ; <>T37 = call WriteConsoleW(stdOut, ptr, len, <>T38, 0)
        mov     rcx, qword [rsp + 24] ; Pass parameter #0
        mov     rdx, qword [rsp + 32] ; Pass parameter #1
        mov     r8, qword [rsp + 56] ; Pass parameter #2
        mov     r9, qword [rsp + 72] ; Pass parameter #3
        mov     qword [rsp + 184], FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     qword [rsp + 16], rax ; Assign return value to <>T37
      ; /
    
      ; Return <>T37
        mov     rax, qword [rsp + 16] ; Return <>T37
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 192 ; Return stack
        ret     
    
