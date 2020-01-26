section .data
FALSE: equ     0
TRUE: equ     1
ptrC0: dq      __utf16__(`Start\n`)
longC1: equ     6
longC2: equ     10
longC3: equ     14214
longC4: equ     2
ptrC5: dq      __utf16__(`\n`)
longC6: equ     94813
longC7: equ     42133
ptrC8: dq      __utf16__(`, `)
longC9: equ     8
longC10: equ     7
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
        sub     qword rsp, 80 ; Allocate stack
    
      ; _ = Call print(Start\n, 6)
        mov     qword rcx, qword ptrC0 ; Pass parameter #0
        mov     qword rdx, qword longC1 ; Pass parameter #1
        call    print
      ; /
    
      ; _ = Call printNum(1)
        mov     qword rcx, qword TRUE ; Pass parameter #0
        call    printNum
      ; /
    
      ; _ = size Assign 10
        mov     qword r15, qword longC2
      ; /
    
      ; <>T1 = Call arrInit(size)
        mov     qword [rsp + 64], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrInit
        mov     qword r14, qword rax ; Assign return value to <>T1
        mov     qword r15, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
      ; _ = arr Assign <>T1
        mov     qword r13, qword r14
      ; /
    
      ; _ = seed Assign 14214
        mov     qword r14, qword longC3
      ; /
    
      ; _ = i Assign 0
        mov     qword r12, qword FALSE
      ; /
    
      ; Jump .T3
        jmp     .T3
      ; /
    
      ; .T2:
    .T2:         
      ; /
    
      ; <>T5 = PreIncrement i
        inc     qword r12
      ; /
    
      ; .T3:
    .T3:         
      ; /
    
      ; <>T6 = i ComparisonLessThan size
        cmp     qword r12, qword r15 ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r11, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r11, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If !<>T6 Jump .T4
        test    qword r11, qword r11 ; Set condition codes according to condition
        jz      .T4 ; Jump if condition is false/zero
      ; /
    
      ; <>T7 = Call pseudoRandom(seed)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 40], qword r12 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 56] ; Pass parameter #0
        call    pseudoRandom
        mov     qword r11, qword rax ; Assign return value to <>T7
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
        mov     qword r12, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; _ = seed Assign <>T7
        mov     qword r14, qword r11
      ; /
    
      ; <>T8 = Call arrIndex(arr, i)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 40], qword r12 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 40] ; Pass parameter #1
        call    arrIndex
        mov     qword r11, qword rax ; Assign return value to <>T8
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
        mov     qword r12, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; <>T10 = size Multiply 2
        mov     qword r10, qword r15 ; Assign LHS to target memory
        imul    qword r10, qword longC4
      ; /
    
      ; <>T9 = seed Remainder <>T10
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword r14 ; Assign LHS to dividend
        idiv    qword r10 ; Assign remainder to RDX, quotient to RAX
        mov     qword r9, qword rdx ; Assign result to target memory
      ; /
    
      ; _ = <>T8 ReferenceAssign <>T9
        mov     qword [r11], qword r9
      ; /
    
      ; Jump .T2
        jmp     .T2
      ; /
    
      ; .T4:
    .T4:         
      ; /
    
      ; _ = Call printArr(arr, size)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    printArr
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; _ = Call print(\n, 1)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword ptrC5 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; _ = Call quickSort(arr, size)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    quickSort
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; _ = Call printArr(arr, size)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    printArr
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; _ = Call arrFree(arr)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrFree
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
pseudoRandom: ; pseudoRandom(seed : long) : long
        sub     qword rsp, 48 ; Allocate stack
    
      ; <>T12 = seed Multiply 94813
        mov     qword r15, qword rcx ; Assign LHS to target memory
        imul    qword r15, qword longC6
      ; /
    
      ; <>T11 = <>T12 Remainder 42133
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword r15 ; Assign LHS to dividend
        mov     qword rbx, qword longC7 ; Move divisor into RBX, as a register is required for idiv
        idiv    qword rbx ; Assign remainder to RDX, quotient to RAX
        mov     qword rcx, qword rdx ; Assign result to target memory
      ; /
    
      ; Return <>T11
        mov     qword rax, qword rcx ; Return <>T11
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 48 ; Return stack
        ret     
    
quickSort: ; quickSort(arr : ptr, size : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T14 = size Subtract 1
        mov     qword r14, qword rdx ; Assign LHS to target memory
        sub     qword r14, qword TRUE
      ; /
    
      ; <>T13 = Call quickSortCore(arr, 0, <>T14)
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 40], qword r14 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    quickSortCore
        mov     qword rdx, qword rax ; Assign return value to <>T13
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; Return <>T13
        mov     qword rax, qword rdx ; Return <>T13
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
quickSortCore: ; quickSortCore(arr : ptr, lo : long, hi : long) : long
        sub     qword rsp, 96 ; Allocate stack
    
        mov     qword r8, qword r8
      ; <>T15 = lo ComparisonGreaterThanOrEqual hi
        cmp     qword rdx, qword r8 ; Set condition codes according to operands
        jge     .CG0 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If <>T15 Jump .T16
        test    qword r12, qword r12 ; Set condition codes according to condition
        jnz     .T16 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T17
        jmp     .T17
      ; /
    
      ; .T16:
    .T16:         
      ; /
    
      ; Return 0
        mov     qword rax, qword FALSE ; Return 0
        jmp     .__exit
      ; /
    
      ; .T17:
    .T17:         
      ; /
    
      ; <>T18 = Call partition(arr, lo, hi)
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 72], qword r8 ; Store live variable onto stack
        mov     qword [rsp + 64], qword rdx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 64] ; Pass parameter #1
        mov     qword r8, qword [rsp + 72] ; Pass parameter #2
        call    partition
        mov     qword r12, qword rax ; Assign return value to <>T18
        mov     qword rcx, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 72] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
      ; _ = partition Assign <>T18
        mov     qword r11, qword r12
      ; /
    
      ; <>T19 = partition Subtract 1
        mov     qword r12, qword r11 ; Assign LHS to target memory
        sub     qword r12, qword TRUE
      ; /
    
      ; _ = Call quickSortCore(arr, lo, <>T19)
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 72], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack
        mov     qword [rsp + 56], qword rdx ; Store live variable onto stack
        mov     qword [rsp + 48], qword r12 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        mov     qword r8, qword [rsp + 48] ; Pass parameter #2
        call    quickSortCore
        mov     qword rcx, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r11, qword [rsp + 72] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r12, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; <>T20 = partition Add 1
        mov     qword r12, qword r11 ; Assign LHS to target memory
        add     qword r12, qword TRUE
      ; /
    
      ; _ = Call quickSortCore(arr, <>T20, hi)
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 72], qword r12 ; Store live variable onto stack
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        mov     qword r8, qword [rsp + 64] ; Pass parameter #2
        call    quickSortCore
        mov     qword rcx, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r12, qword [rsp + 72] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 96 ; Return stack
        ret     
    
partition: ; partition(arr : ptr, lo : long, hi : long) : long
        sub     qword rsp, 112 ; Allocate stack
    
        mov     qword r8, qword r8
      ; <>T22 = Call arrIndex(arr, hi)
        mov     qword [rsp + 96], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 88], qword rdx ; Store live variable onto stack
        mov     qword [rsp + 80], qword r8 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 96] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 80] ; Pass parameter #1
        call    arrIndex
        mov     qword r13, qword rax ; Assign return value to <>T22
        mov     qword rcx, qword [rsp + 96] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 88] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 80] ; Restore live variable from stack
      ; /
    
      ; <>T21 = Dereference <>T22
        mov     qword r11, [r13] ; Dereference <>T22
      ; /
    
      ; _ = pivot Assign <>T21
        mov     qword r13, qword r11
      ; /
    
      ; _ = i Assign lo
        mov     qword r11, qword rdx
      ; /
    
      ; _ = j Assign lo
        mov     qword r10, qword rdx
      ; /
    
      ; Jump .T24
        jmp     .T24
      ; /
    
      ; .T23:
    .T23:         
      ; /
    
      ; <>T26 = PreIncrement j
        inc     qword r10
      ; /
    
      ; .T24:
    .T24:         
      ; /
    
      ; <>T27 = j ComparisonLessThanOrEqual hi
        cmp     qword r10, qword r8 ; Set condition codes according to operands
        jle     .CG0 ; Jump to True if the comparison is true
        mov     qword rdx, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword rdx, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If !<>T27 Jump .T25
        test    qword rdx, qword rdx ; Set condition codes according to condition
        jz      .T25 ; Jump if condition is false/zero
      ; /
    
      ; <>T29 = Call arrIndex(arr, j)
        mov     qword [rsp + 96], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 80], qword r10 ; Store live variable onto stack
        mov     qword [rsp + 72], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 80] ; Pass parameter #1
        call    arrIndex
        mov     qword rdx, qword rax ; Assign return value to <>T29
        mov     qword r11, qword [rsp + 96] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack
        mov     qword r10, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 72] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
      ; <>T28 = Dereference <>T29
        mov     qword r9, [rdx] ; Dereference <>T29
      ; /
    
      ; _ = elem Assign <>T28
        mov     qword rdx, qword r9
      ; /
    
      ; <>T30 = elem ComparisonLessThan pivot
        cmp     qword rdx, qword r13 ; Set condition codes according to operands
        jl      .CG2 ; Jump to True if the comparison is true
        mov     qword r9, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r9, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; /
    
      ; If <>T30 Jump .T31
        test    qword r9, qword r9 ; Set condition codes according to condition
        jnz     .T31 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T32
        jmp     .T32
      ; /
    
      ; .T31:
    .T31:         
      ; /
    
      ; <>T33 = Call arrIndex(arr, j)
        mov     qword [rsp + 96], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 80], qword rdx ; Store live variable onto stack
        mov     qword [rsp + 72], qword r10 ; Store live variable onto stack
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r8 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        call    arrIndex
        mov     qword r9, qword rax ; Assign return value to <>T33
        mov     qword r11, qword [rsp + 96] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r10, qword [rsp + 72] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; <>T35 = Call arrIndex(arr, i)
        mov     qword [rsp + 96], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 80], qword rdx ; Store live variable onto stack
        mov     qword [rsp + 72], qword r9 ; Store live variable onto stack
        mov     qword [rsp + 64], qword r10 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 48], qword r8 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r8, qword rax ; Assign return value to <>T35
        mov     qword r11, qword [rsp + 96] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r9, qword [rsp + 72] ; Restore live variable from stack
        mov     qword r10, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; <>T34 = Dereference <>T35
        mov     qword rcx, [r8] ; Dereference <>T35
      ; /
    
      ; _ = <>T33 ReferenceAssign <>T34
        mov     qword [r9], qword rcx
      ; /
    
      ; <>T36 = Call arrIndex(arr, i)
        mov     qword [rsp + 96], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 80], qword rdx ; Store live variable onto stack
        mov     qword [rsp + 72], qword r10 ; Store live variable onto stack
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r8 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword rcx, qword rax ; Assign return value to <>T36
        mov     qword r11, qword [rsp + 96] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r10, qword [rsp + 72] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; _ = <>T36 ReferenceAssign elem
        mov     qword [rcx], qword rdx
      ; /
    
      ; <>T37 = i Add 1
        mov     qword rcx, qword r11 ; Assign LHS to target memory
        add     qword rcx, qword TRUE
      ; /
    
      ; _ = i Assign <>T37
        mov     qword r11, qword rcx
      ; /
    
      ; .T32:
    .T32:         
      ; /
    
      ; Jump .T23
        jmp     .T23
      ; /
    
      ; .T25:
    .T25:         
      ; /
    
      ; <>T38 = Call arrIndex(arr, hi)
        mov     qword [rsp + 96], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 80], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 72], qword r8 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        call    arrIndex
        mov     qword rcx, qword rax ; Assign return value to <>T38
        mov     qword r11, qword [rsp + 96] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 80] ; Restore live variable from stack
        mov     qword r8, qword [rsp + 72] ; Restore live variable from stack
      ; /
    
      ; <>T40 = Call arrIndex(arr, i)
        mov     qword [rsp + 96], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 80], qword r13 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r8, qword rax ; Assign return value to <>T40
        mov     qword r11, qword [rsp + 96] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 80] ; Restore live variable from stack
      ; /
    
      ; <>T39 = Dereference <>T40
        mov     qword r10, [r8] ; Dereference <>T40
      ; /
    
      ; _ = <>T38 ReferenceAssign <>T39
        mov     qword [rcx], qword r10
      ; /
    
      ; <>T41 = Call arrIndex(arr, i)
        mov     qword [rsp + 96], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 80], qword r13 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r10, qword rax ; Assign return value to <>T41
        mov     qword r11, qword [rsp + 96] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 80] ; Restore live variable from stack
      ; /
    
      ; _ = <>T41 ReferenceAssign pivot
        mov     qword [r10], qword r13
      ; /
    
      ; Return i
        mov     qword rax, qword r11 ; Return i
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 112 ; Return stack
        ret     
    
arrInit: ; arrInit(size : long) : ptr
        sub     qword rsp, 80 ; Allocate stack
    
      ; <>T42 = Call arrAlloc(size)
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrAlloc
        mov     qword r15, qword rax ; Assign return value to <>T42
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
      ; _ = arr Assign <>T42
        mov     qword r13, qword r15
      ; /
    
      ; _ = i Assign 0
        mov     qword r15, qword FALSE
      ; /
    
      ; Jump .T44
        jmp     .T44
      ; /
    
      ; .T43:
    .T43:         
      ; /
    
      ; <>T46 = PreIncrement i
        inc     qword r15
      ; /
    
      ; .T44:
    .T44:         
      ; /
    
      ; <>T47 = i ComparisonLessThan size
        cmp     qword r15, qword rcx ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If !<>T47 Jump .T45
        test    qword r12, qword r12 ; Set condition codes according to condition
        jz      .T45 ; Jump if condition is false/zero
      ; /
    
      ; <>T48 = Call arrIndex(arr, i)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    arrIndex
        mov     qword r12, qword rax ; Assign return value to <>T48
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; _ = <>T48 ReferenceAssign 0
        mov     qword [r12], qword FALSE
      ; /
    
      ; Jump .T43
        jmp     .T43
      ; /
    
      ; .T45:
    .T45:         
      ; /
    
      ; Return arr
        mov     qword rax, qword r13 ; Return arr
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
arrFree: ; arrFree(arr : ptr) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T49 = Call free(arr)
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        call    free
        mov     qword r15, qword rax ; Assign return value to <>T49
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; Return <>T49
        mov     qword rax, qword r15 ; Return <>T49
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printArr: ; printArr(arr : ptr, size : long) : long
        sub     qword rsp, 80 ; Allocate stack
    
      ; _ = i Assign 0
        mov     qword r14, qword FALSE
      ; /
    
      ; Jump .T51
        jmp     .T51
      ; /
    
      ; .T50:
    .T50:         
      ; /
    
      ; <>T53 = PreIncrement i
        inc     qword r14
      ; /
    
      ; .T51:
    .T51:         
      ; /
    
      ; <>T54 = i ComparisonLessThan size
        cmp     qword r14, qword rdx ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If !<>T54 Jump .T52
        test    qword r12, qword r12 ; Set condition codes according to condition
        jz      .T52 ; Jump if condition is false/zero
      ; /
    
      ; <>T55 = i ComparisonGreaterThan 0
        cmp     qword r14, qword FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; /
    
      ; If <>T55 Jump .T56
        test    qword r12, qword r12 ; Set condition codes according to condition
        jnz     .T56 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T57
        jmp     .T57
      ; /
    
      ; .T56:
    .T56:         
      ; /
    
      ; _ = Call print(, , 2)
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 48], qword rdx ; Store live variable onto stack
        mov     qword rcx, qword ptrC8 ; Pass parameter #0
        mov     qword rdx, qword longC4 ; Pass parameter #1
        call    print
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; .T57:
    .T57:         
      ; /
    
      ; <>T59 = Call arrIndex(arr, i)
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 48], qword rdx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    arrIndex
        mov     qword r12, qword rax ; Assign return value to <>T59
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; <>T58 = Dereference <>T59
        mov     qword r11, [r12] ; Dereference <>T59
      ; /
    
      ; _ = Call printNum(<>T58)
        mov     qword [rsp + 64], qword r11 ; Store live variable onto stack
        mov     qword [rsp + 56], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 48], qword r14 ; Store live variable onto stack
        mov     qword [rsp + 40], qword rdx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    printNum
        mov     qword r11, qword [rsp + 64] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 48] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; Jump .T50
        jmp     .T50
      ; /
    
      ; .T52:
    .T52:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
arrSize: ; arrSize(arr : ptr) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T62 = ArithmeticNegation 1
        mov     qword r14, qword TRUE ; Assign operand to target
        neg     qword r14
      ; /
    
      ; <>T61 = Call arrIndex(arr, <>T62)
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 40], qword r14 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 40] ; Pass parameter #1
        call    arrIndex
        mov     qword r13, qword rax ; Assign return value to <>T61
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack
        mov     qword r14, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; <>T60 = Dereference <>T61
        mov     qword r14, [r13] ; Dereference <>T61
      ; /
    
      ; Return <>T60
        mov     qword rax, qword r14 ; Return <>T60
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
arrAlloc: ; arrAlloc(size : long) : ptr
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T64 = size Multiply 8
        mov     qword r15, qword rcx ; Assign LHS to target memory
        imul    qword r15, qword longC9
      ; /
    
      ; <>T63 = Call alloc(<>T64)
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        call    alloc
        mov     qword rcx, qword rax ; Assign return value to <>T63
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; Return <>T63
        mov     qword rax, qword rcx ; Return <>T63
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
arrIndex: ; arrIndex(arr : ptr, index : long) : ptr
        sub     qword rsp, 48 ; Allocate stack
    
      ; <>T66 = arr Add 7
        mov     qword r15, qword rcx ; Assign LHS to target memory
        add     qword r15, qword longC10
      ; /
    
      ; <>T67 = 8 Multiply index
        mov     qword rcx, qword longC9 ; Assign LHS to target memory
        imul    qword rcx, qword rdx
      ; /
    
      ; <>T65 = <>T66 Add <>T67
        mov     qword rdx, qword r15 ; Assign LHS to target memory
        add     qword rdx, qword rcx
      ; /
    
      ; Return <>T65
        mov     qword rax, qword rdx ; Return <>T65
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 48 ; Return stack
        ret     
    
alloc: ; alloc(size : long) : ptr
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T68 = Call GetProcessHeap()
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack
        call    GetProcessHeap
        mov     qword r15, qword rax ; Assign return value to <>T68
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; _ = heapHandle Assign <>T68
        mov     qword r13, qword r15
      ; /
    
      ; <>T69 = Call HeapAlloc(heapHandle, 0, size)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 40], qword rcx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    HeapAlloc
        mov     qword r15, qword rax ; Assign return value to <>T69
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; Return <>T69
        mov     qword rax, qword r15 ; Return <>T69
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
free: ; free(position : ptr) : bool
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T70 = Call GetProcessHeap()
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack
        call    GetProcessHeap
        mov     qword r15, qword rax ; Assign return value to <>T70
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; _ = heapHandle Assign <>T70
        mov     qword r13, qword r15
      ; /
    
      ; <>T71 = Call HeapFree(heapHandle, 0, position)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack
        mov     qword [rsp + 40], qword rcx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    HeapFree
        mov     qword r15, qword rax ; Assign return value to <>T71
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 40] ; Restore live variable from stack
      ; /
    
      ; Return <>T71
        mov     qword rax, qword r15 ; Return <>T71
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T72 = Call printNumAny(num, 10)
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword longC2 ; Pass parameter #1
        call    printNumAny
        mov     qword r15, qword rax ; Assign return value to <>T72
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; Return <>T72
        mov     qword rax, qword r15 ; Return <>T72
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printNumAny: ; printNumAny(num : long, base : long) : long
        sub     qword rsp, 80 ; Allocate stack
    
      ; <>T73 = num ComparisonLessThan 0
        cmp     qword rcx, qword FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r13, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r13, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If <>T73 Jump .T74
        test    qword r13, qword r13 ; Set condition codes according to condition
        jnz     .T74 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T75
        jmp     .T75
      ; /
    
      ; .T74:
    .T74:         
      ; /
    
      ; _ = Call print(-, 1)
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 56], qword rdx ; Store live variable onto stack
        mov     qword rcx, qword ptrC11 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; <>T76 = ArithmeticNegation num
        mov     qword r13, qword rcx ; Assign operand to target
        neg     qword r13
      ; /
    
      ; _ = Call printNum(<>T76)
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 56], qword rdx ; Store live variable onto stack
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        call    printNum
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 56] ; Restore live variable from stack
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; Return 
        jmp     .__exit
      ; /
    
      ; .T75:
    .T75:         
      ; /
    
      ; <>T77 = num Remainder base
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword rcx ; Assign LHS to dividend
        idiv    qword rdx ; Assign remainder to RDX, quotient to RAX
        mov     qword r13, qword rdx ; Assign result to target memory
      ; /
    
      ; _ = digit Assign <>T77
        mov     qword r12, qword r13
      ; /
    
      ; <>T78 = num Divide base
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword rcx ; Assign LHS to dividend
        idiv    qword rdx ; Assign remainder to RDX, quotient to RAX
        mov     qword r13, qword rax ; Assign result to target memory
      ; /
    
      ; _ = rest Assign <>T78
        mov     qword rcx, qword r13
      ; /
    
      ; <>T79 = rest ComparisonGreaterThan 0
        cmp     qword rcx, qword FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     qword r13, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r13, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; /
    
      ; If <>T79 Jump .T80
        test    qword r13, qword r13 ; Set condition codes according to condition
        jnz     .T80 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T81
        jmp     .T81
      ; /
    
      ; .T80:
    .T80:         
      ; /
    
      ; _ = Call printNumAny(rest, base)
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack
        mov     qword [rsp + 56], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 48], qword rdx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 56] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 48] ; Pass parameter #1
        call    printNumAny
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 56] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
      ; .T81:
    .T81:         
      ; /
    
      ; _ = Call printDigit(digit)
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    printDigit
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
printDigit: ; printDigit(digit : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; _ = digits Assign 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
        mov     qword r15, qword ptrC12
      ; /
    
      ; <>T83 = digit Multiply 2
        mov     qword r13, qword rcx ; Assign LHS to target memory
        imul    qword r13, qword longC4
      ; /
    
      ; <>T82 = digits Add <>T83
        mov     qword rcx, qword r15 ; Assign LHS to target memory
        add     qword rcx, qword r13
      ; /
    
      ; _ = Call print(<>T82, 1)
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
print: ; print(ptr : ptr, len : long) : long
        sub     qword rsp, 96 ; Allocate stack
    
      ; <>T85 = ArithmeticNegation 11
        mov     qword r15, qword longC13 ; Assign operand to target
        neg     qword r15
      ; /
    
      ; <>T84 = Call GetStdHandle(<>T85)
        mov     qword [rsp + 80], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 72], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 64], qword rdx ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        call    GetStdHandle
        mov     qword r12, qword rax ; Assign return value to <>T84
        mov     qword r15, qword [rsp + 80] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 72] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 64] ; Restore live variable from stack
      ; /
    
      ; _ = stdOut Assign <>T84
        mov     qword r15, qword r12
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
        mov     qword r12, qword FALSE
      ; /
    
      ; <>T87 = Reference numberOfCharsWritten
        lea     qword r11, [r12] ; Create reference to numberOfCharsWritten
      ; /
    
      ; <>T86 = Call WriteConsoleW(stdOut, ptr, len, <>T87, 0)
        mov     qword [rsp + 80], qword r15 ; Store live variable onto stack
        mov     qword [rsp + 72], qword rcx ; Store live variable onto stack
        mov     qword [rsp + 64], qword rdx ; Store live variable onto stack
        mov     qword [rsp + 56], qword r11 ; Store live variable onto stack
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        mov     qword r8, qword [rsp + 64] ; Pass parameter #2
        mov     qword r9, qword [rsp + 56] ; Pass parameter #3
        mov     qword [rsp + 64], qword FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     qword r12, qword rax ; Assign return value to <>T86
        mov     qword r15, qword [rsp + 80] ; Restore live variable from stack
        mov     qword rcx, qword [rsp + 72] ; Restore live variable from stack
        mov     qword rdx, qword [rsp + 64] ; Restore live variable from stack
        mov     qword r11, qword [rsp + 56] ; Restore live variable from stack
      ; /
    
      ; Return <>T86
        mov     qword rax, qword r12 ; Return <>T86
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 96 ; Return stack
        ret     
    
