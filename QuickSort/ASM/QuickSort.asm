section .data
FALSE: equ     0
TRUE: equ     1
ptrC0: dq      __utf16__(`Start\n`)
longC1: equ     6
longC2: equ     10
longC3: equ     420
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
      ; In = {  }
        mov     qword rcx, qword ptrC0 ; Pass parameter #0
        mov     qword rdx, qword longC1 ; Pass parameter #1
        call    print
      ; Out = {  }
      ; /
    
      ; _ = size Assign 10
      ; In = {  }
        mov     qword r15, qword longC2
      ; Out = { size }
      ; /
    
      ; <>T1 = Call arrInit(size)
      ; In = { size }
        mov     qword [rsp + 64], qword r15 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrInit
        mov     qword r14, qword rax ; Assign return value to <>T1
        mov     qword r15, qword [rsp + 64] ; Restore live variable from stack (size)
      ; Out = { <>T1, size }
      ; /
    
      ; _ = arr Assign <>T1
      ; In = { <>T1, size }
        mov     qword r13, qword r14
      ; Out = { arr, size }
      ; /
    
      ; _ = seed Assign 420
      ; In = { arr, size }
        mov     qword r12, qword longC3
      ; Out = { arr, seed, size }
      ; /
    
      ; _ = i Assign 0
      ; In = { arr, seed, size }
        mov     qword r14, qword FALSE
      ; Out = { arr, seed, size, i }
      ; /
    
      ; Jump .T3
      ; In = { arr, seed, size, i }
        jmp     .T3
      ; Out = { arr, seed, size, i }
      ; /
    
      ; .T2:
      ; In = { arr, seed, size, i }
    .T2:         
      ; Out = { arr, seed, size, i }
      ; /
    
      ; <>T5 = PreIncrement i
      ; In = { arr, seed, size, i }
        inc     qword r14
      ; Out = { arr, seed, size, i }
      ; /
    
      ; .T3:
      ; In = { arr, seed, size, i }
    .T3:         
      ; Out = { arr, seed, size, i }
      ; /
    
      ; <>T6 = i ComparisonLessThan size
      ; In = { arr, seed, size, i }
        cmp     qword r14, qword r15 ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r11, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r11, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { arr, seed, size, i, <>T6 }
      ; /
    
      ; If !<>T6 Jump .T4
      ; In = { arr, seed, size, i, <>T6 }
        test    qword r11, qword r11 ; Set condition codes according to condition
        jz      .T4 ; Jump if condition is false/zero
      ; Out = { arr, seed, size, i }
      ; /
    
      ; <>T7 = Call pseudoRandom(seed)
      ; In = { arr, seed, size, i }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r12 ; Store live variable onto stack (seed)
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack (size)
        mov     qword [rsp + 40], qword r14 ; Store live variable onto stack (i)
        mov     qword rcx, qword [rsp + 56] ; Pass parameter #0
        call    pseudoRandom
        mov     qword r10, qword rax ; Assign return value to <>T7
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack (size)
        mov     qword r14, qword [rsp + 40] ; Restore live variable from stack (i)
      ; Out = { arr, <>T7, size, i }
      ; /
    
      ; _ = seed Assign <>T7
      ; In = { arr, <>T7, size, i }
        mov     qword r12, qword r10
      ; Out = { arr, seed, size, i }
      ; /
    
      ; <>T8 = Call arrIndex(arr, i)
      ; In = { arr, seed, size, i }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r12 ; Store live variable onto stack (seed)
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack (size)
        mov     qword [rsp + 40], qword r14 ; Store live variable onto stack (i)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 40] ; Pass parameter #1
        call    arrIndex
        mov     qword r11, qword rax ; Assign return value to <>T8
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r12, qword [rsp + 56] ; Restore live variable from stack (seed)
        mov     qword r15, qword [rsp + 48] ; Restore live variable from stack (size)
        mov     qword r14, qword [rsp + 40] ; Restore live variable from stack (i)
      ; Out = { <>T8, seed, size, arr, i }
      ; /
    
      ; <>T10 = size Multiply 2
      ; In = { <>T8, seed, size, arr, i }
        mov     qword r10, qword r15 ; Assign LHS to target memory
        imul    qword r10, qword longC4
      ; Out = { <>T8, seed, <>T10, arr, size, i }
      ; /
    
      ; <>T9 = seed Remainder <>T10
      ; In = { <>T8, seed, <>T10, arr, size, i }
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword r12 ; Assign LHS to dividend
        idiv    qword r10 ; Assign remainder to RDX, quotient to RAX
        mov     qword r9, qword rdx ; Assign result to target memory
      ; Out = { <>T8, <>T9, arr, seed, size, i }
      ; /
    
      ; _ = <>T8 ReferenceAssign <>T9
      ; In = { <>T8, <>T9, arr, seed, size, i }
        mov     qword [r11], qword r9
      ; Out = { arr, seed, size, i }
      ; /
    
      ; Jump .T2
      ; In = { arr, seed, size, i }
        jmp     .T2
      ; Out = { arr, seed, size, i }
      ; /
    
      ; .T4:
      ; In = { arr, size }
    .T4:         
      ; Out = { arr, size }
      ; /
    
      ; _ = Call printArr(arr, size)
      ; In = { arr, size }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    printArr
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack (size)
      ; Out = { arr, size }
      ; /
    
      ; _ = Call print(\n, 1)
      ; In = { arr, size }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack (size)
        mov     qword rcx, qword ptrC5 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack (size)
      ; Out = { arr, size }
      ; /
    
      ; _ = Call quickSort(arr, size)
      ; In = { arr, size }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    quickSort
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r15, qword [rsp + 56] ; Restore live variable from stack (size)
      ; Out = { arr, size }
      ; /
    
      ; _ = Call printArr(arr, size)
      ; In = { arr, size }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r15 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    printArr
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (arr)
      ; Out = { arr }
      ; /
    
      ; _ = Call arrFree(arr)
      ; In = { arr }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrFree
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
pseudoRandom: ; pseudoRandom(seed : long) : long
        sub     qword rsp, 48 ; Allocate stack
    
      ; <>T12 = seed Multiply 94813
      ; In = { seed }
        mov     qword r15, qword rcx ; Assign LHS to target memory
        imul    qword r15, qword longC6
      ; Out = { <>T12 }
      ; /
    
      ; <>T11 = <>T12 Remainder 42133
      ; In = { <>T12 }
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword r15 ; Assign LHS to dividend
        mov     qword rbx, qword longC7 ; Move divisor into RBX, as a register is required for idiv
        idiv    qword rbx ; Assign remainder to RDX, quotient to RAX
        mov     qword r13, qword rdx ; Assign result to target memory
      ; Out = { <>T11 }
      ; /
    
      ; Return <>T11
      ; In = { <>T11 }
        mov     qword rax, qword r13 ; Return <>T11
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 48 ; Return stack
        ret     
    
quickSort: ; quickSort(arr : ptr, size : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
        mov     qword r13, qword rdx
      ; <>T14 = size Subtract 1
      ; In = { arr, size }
        mov     qword r14, qword r13 ; Assign LHS to target memory
        sub     qword r14, qword TRUE
      ; Out = { arr, <>T14 }
      ; /
    
      ; <>T13 = Call quickSortCore(arr, 0, <>T14)
      ; In = { arr, <>T14 }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 40], qword r14 ; Store live variable onto stack (<>T14)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    quickSortCore
        mov     qword r12, qword rax ; Assign return value to <>T13
      ; Out = { <>T13 }
      ; /
    
      ; Return <>T13
      ; In = { <>T13 }
        mov     qword rax, qword r12 ; Return <>T13
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
quickSortCore: ; quickSortCore(arr : ptr, lo : long, hi : long) : long
        sub     qword rsp, 96 ; Allocate stack
    
        mov     qword r13, qword rdx
      ; <>T15 = lo ComparisonGreaterThanOrEqual hi
      ; In = { arr, hi, lo }
        cmp     qword r13, qword r8 ; Set condition codes according to operands
        jge     .CG0 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { arr, hi, lo, <>T15 }
      ; /
    
      ; If <>T15 Jump .T16
      ; In = { arr, hi, lo, <>T15 }
        test    qword r12, qword r12 ; Set condition codes according to condition
        jnz     .T16 ; Jump if condition is true/non-zero
      ; Out = { arr, hi, lo }
      ; /
    
      ; Jump .T17
      ; In = { arr, hi, lo }
        jmp     .T17
      ; Out = { arr, hi, lo }
      ; /
    
      ; .T16:
      ; In = { arr, hi, lo }
    .T16:         
      ; Out = { arr, hi, lo }
      ; /
    
      ; Return 0
      ; In = { arr, hi, lo }
        mov     qword rax, qword FALSE ; Return 0
        jmp     .__exit
      ; Out = { arr, hi, lo }
      ; /
    
      ; .T17:
      ; In = { arr, hi, lo }
    .T17:         
      ; Out = { arr, hi, lo }
      ; /
    
      ; <>T18 = Call partition(arr, lo, hi)
      ; In = { arr, hi, lo }
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 72], qword r8 ; Store live variable onto stack (hi)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (lo)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 64] ; Pass parameter #1
        mov     qword r8, qword [rsp + 72] ; Pass parameter #2
        call    partition
        mov     qword r12, qword rax ; Assign return value to <>T18
        mov     qword rcx, qword [rsp + 80] ; Restore live variable from stack (arr)
        mov     qword r8, qword [rsp + 72] ; Restore live variable from stack (hi)
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (lo)
      ; Out = { arr, <>T18, hi, lo }
      ; /
    
      ; _ = partition Assign <>T18
      ; In = { arr, <>T18, hi, lo }
        mov     qword r11, qword r12
      ; Out = { arr, partition, hi, lo }
      ; /
    
      ; <>T19 = partition Subtract 1
      ; In = { arr, partition, hi, lo }
        mov     qword r10, qword r11 ; Assign LHS to target memory
        sub     qword r10, qword TRUE
      ; Out = { arr, partition, hi, lo, <>T19 }
      ; /
    
      ; _ = Call quickSortCore(arr, lo, <>T19)
      ; In = { arr, partition, hi, lo, <>T19 }
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 72], qword r11 ; Store live variable onto stack (partition)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword [rsp + 56], qword r13 ; Store live variable onto stack (lo)
        mov     qword [rsp + 48], qword r10 ; Store live variable onto stack (<>T19)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        mov     qword r8, qword [rsp + 48] ; Pass parameter #2
        call    quickSortCore
        mov     qword rcx, qword [rsp + 80] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 72] ; Restore live variable from stack (partition)
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack (hi)
      ; Out = { arr, partition, hi }
      ; /
    
      ; <>T20 = partition Add 1
      ; In = { arr, partition, hi }
        mov     qword r12, qword r11 ; Assign LHS to target memory
        add     qword r12, qword TRUE
      ; Out = { arr, <>T20, hi }
      ; /
    
      ; _ = Call quickSortCore(arr, <>T20, hi)
      ; In = { arr, <>T20, hi }
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 72], qword r12 ; Store live variable onto stack (<>T20)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        mov     qword r8, qword [rsp + 64] ; Pass parameter #2
        call    quickSortCore
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 96 ; Return stack
        ret     
    
partition: ; partition(arr : ptr, lo : long, hi : long) : long
        sub     qword rsp, 128 ; Allocate stack
    
        mov     qword r14, qword rdx
      ; <>T22 = Call arrIndex(arr, hi)
      ; In = { arr, lo, hi }
        mov     qword [rsp + 96], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 88], qword r14 ; Store live variable onto stack (lo)
        mov     qword [rsp + 80], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 96] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 80] ; Pass parameter #1
        call    arrIndex
        mov     qword r13, qword rax ; Assign return value to <>T22
        mov     qword rcx, qword [rsp + 96] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 88] ; Restore live variable from stack (lo)
        mov     qword r8, qword [rsp + 80] ; Restore live variable from stack (hi)
      ; Out = { arr, lo, <>T22, hi }
      ; /
    
      ; <>T21 = Dereference <>T22
      ; In = { arr, lo, <>T22, hi }
        mov     qword r11, [r13] ; Dereference <>T22
      ; Out = { arr, lo, <>T21, hi }
      ; /
    
      ; _ = pivot Assign <>T21
      ; In = { arr, lo, <>T21, hi }
        mov     qword r10, qword r11
      ; Out = { arr, lo, pivot, hi }
      ; /
    
      ; _ = i Assign lo
      ; In = { arr, lo, pivot, hi }
        mov     qword r13, qword r14
      ; Out = { i, arr, lo, pivot, hi }
      ; /
    
      ; _ = j Assign lo
      ; In = { i, arr, lo, pivot, hi }
        mov     qword r11, qword r14
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; Jump .T24
      ; In = { i, arr, j, pivot, hi }
        jmp     .T24
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T23:
      ; In = { i, arr, j, pivot, hi }
    .T23:         
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; <>T26 = PreIncrement j
      ; In = { i, arr, j, pivot, hi }
        inc     qword r11
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T24:
      ; In = { i, arr, j, pivot, hi }
    .T24:         
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; <>T27 = j ComparisonLessThanOrEqual hi
      ; In = { i, arr, j, pivot, hi }
        cmp     qword r11, qword r8 ; Set condition codes according to operands
        jle     .CG0 ; Jump to True if the comparison is true
        mov     qword r14, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r14, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { i, arr, j, pivot, hi, <>T27 }
      ; /
    
      ; If !<>T27 Jump .T25
      ; In = { i, arr, j, pivot, hi, <>T27 }
        test    qword r14, qword r14 ; Set condition codes according to condition
        jz      .T25 ; Jump if condition is false/zero
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; <>T29 = Call arrIndex(arr, j)
      ; In = { i, arr, j, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r11 ; Store live variable onto stack (j)
        mov     qword [rsp + 72], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 80] ; Pass parameter #1
        call    arrIndex
        mov     qword r9, qword rax ; Assign return value to <>T29
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 80] ; Restore live variable from stack (j)
        mov     qword r10, qword [rsp + 72] ; Restore live variable from stack (pivot)
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack (hi)
      ; Out = { i, arr, <>T29, j, pivot, hi }
      ; /
    
      ; <>T28 = Dereference <>T29
      ; In = { i, arr, <>T29, j, pivot, hi }
        mov     qword r14, [r9] ; Dereference <>T29
      ; Out = { i, arr, <>T28, j, pivot, hi }
      ; /
    
      ; _ = elem Assign <>T28
      ; In = { i, arr, <>T28, j, pivot, hi }
        mov     qword [rsp + 120], qword r14
      ; Out = { i, arr, elem, j, pivot, hi }
      ; /
    
      ; <>T30 = elem ComparisonLessThan pivot
      ; In = { i, arr, elem, j, pivot, hi }
        cmp     qword [rsp + 120], qword r10 ; Set condition codes according to operands
        jl      .CG2 ; Jump to True if the comparison is true
        mov     qword r9, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r9, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; Out = { i, arr, elem, j, <>T30, pivot, hi }
      ; /
    
      ; If <>T30 Jump .T31
      ; In = { i, arr, elem, j, <>T30, pivot, hi }
        test    qword r9, qword r9 ; Set condition codes according to condition
        jnz     .T31 ; Jump if condition is true/non-zero
      ; Out = { i, arr, elem, j, pivot, hi }
      ; /
    
      ; Jump .T32
      ; In = { i, arr, j, pivot, hi }
        jmp     .T32
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T31:
      ; In = { i, arr, elem, j, pivot, hi }
    .T31:         
      ; Out = { i, arr, elem, j, pivot, hi }
      ; /
    
      ; <>T33 = Call arrIndex(arr, j)
      ; In = { i, arr, elem, j, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r11 ; Store live variable onto stack (j)
        mov     qword [rsp + 72], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 80] ; Pass parameter #1
        call    arrIndex
        mov     qword r9, qword rax ; Assign return value to <>T33
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 80] ; Restore live variable from stack (j)
        mov     qword r10, qword [rsp + 72] ; Restore live variable from stack (pivot)
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack (hi)
      ; Out = { i, arr, elem, <>T33, j, pivot, hi }
      ; /
    
      ; <>T35 = Call arrIndex(arr, i)
      ; In = { i, arr, elem, <>T33, j, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r9 ; Store live variable onto stack (<>T33)
        mov     qword [rsp + 72], qword r11 ; Store live variable onto stack (j)
        mov     qword [rsp + 64], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 56], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r14, qword rax ; Assign return value to <>T35
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r9, qword [rsp + 80] ; Restore live variable from stack (<>T33)
        mov     qword r11, qword [rsp + 72] ; Restore live variable from stack (j)
        mov     qword r10, qword [rsp + 64] ; Restore live variable from stack (pivot)
        mov     qword r8, qword [rsp + 56] ; Restore live variable from stack (hi)
      ; Out = { i, arr, elem, <>T33, <>T35, j, pivot, hi }
      ; /
    
      ; <>T34 = Dereference <>T35
      ; In = { i, arr, elem, <>T33, <>T35, j, pivot, hi }
        mov     qword rbx, qword r14 ; Move RHS into register so operation is possible
        mov     qword rbx, [rbx] ; Dereference <>T35
        mov     qword [rsp + 112], qword rbx ; Assign result to actual target memory
      ; Out = { i, arr, elem, <>T33, <>T34, j, pivot, hi }
      ; /
    
      ; _ = <>T33 ReferenceAssign <>T34
      ; In = { i, arr, elem, <>T33, <>T34, j, pivot, hi }
        mov     qword rbx, qword [rsp + 112]
        mov     qword [r9], qword rbx
      ; Out = { i, arr, elem, j, pivot, hi }
      ; /
    
      ; <>T36 = Call arrIndex(arr, i)
      ; In = { i, arr, elem, j, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r11 ; Store live variable onto stack (j)
        mov     qword [rsp + 72], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r14, qword rax ; Assign return value to <>T36
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 80] ; Restore live variable from stack (j)
        mov     qword r10, qword [rsp + 72] ; Restore live variable from stack (pivot)
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack (hi)
      ; Out = { i, <>T36, elem, arr, j, pivot, hi }
      ; /
    
      ; _ = <>T36 ReferenceAssign elem
      ; In = { i, <>T36, elem, arr, j, pivot, hi }
        mov     qword rbx, qword [rsp + 120]
        mov     qword [r14], qword rbx
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; <>T37 = i Add 1
      ; In = { i, arr, j, pivot, hi }
        mov     qword r9, qword r13 ; Assign LHS to target memory
        add     qword r9, qword TRUE
      ; Out = { <>T37, arr, j, pivot, hi }
      ; /
    
      ; _ = i Assign <>T37
      ; In = { <>T37, arr, j, pivot, hi }
        mov     qword r13, qword r9
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T32:
      ; In = { i, arr, j, pivot, hi }
    .T32:         
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; Jump .T23
      ; In = { i, arr, j, pivot, hi }
        jmp     .T23
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T25:
      ; In = { i, arr, pivot, hi }
    .T25:         
      ; Out = { i, arr, pivot, hi }
      ; /
    
      ; <>T38 = Call arrIndex(arr, hi)
      ; In = { i, arr, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 72], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        call    arrIndex
        mov     qword r9, qword rax ; Assign return value to <>T38
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r10, qword [rsp + 80] ; Restore live variable from stack (pivot)
      ; Out = { i, arr, pivot, <>T38 }
      ; /
    
      ; <>T40 = Call arrIndex(arr, i)
      ; In = { i, arr, pivot, <>T38 }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 72], qword r9 ; Store live variable onto stack (<>T38)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r14, qword rax ; Assign return value to <>T40
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r10, qword [rsp + 80] ; Restore live variable from stack (pivot)
        mov     qword r9, qword [rsp + 72] ; Restore live variable from stack (<>T38)
      ; Out = { i, arr, pivot, <>T38, <>T40 }
      ; /
    
      ; <>T39 = Dereference <>T40
      ; In = { i, arr, pivot, <>T38, <>T40 }
        mov     qword r8, [r14] ; Dereference <>T40
      ; Out = { i, arr, pivot, <>T38, <>T39 }
      ; /
    
      ; _ = <>T38 ReferenceAssign <>T39
      ; In = { i, arr, pivot, <>T38, <>T39 }
        mov     qword [r9], qword r8
      ; Out = { i, arr, pivot }
      ; /
    
      ; <>T41 = Call arrIndex(arr, i)
      ; In = { i, arr, pivot }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r10 ; Store live variable onto stack (pivot)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r14, qword rax ; Assign return value to <>T41
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword r10, qword [rsp + 80] ; Restore live variable from stack (pivot)
      ; Out = { i, <>T41, pivot }
      ; /
    
      ; _ = <>T41 ReferenceAssign pivot
      ; In = { i, <>T41, pivot }
        mov     qword [r14], qword r10
      ; Out = { i }
      ; /
    
      ; Return i
      ; In = { i }
        mov     qword rax, qword r13 ; Return i
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 128 ; Return stack
        ret     
    
arrInit: ; arrInit(size : long) : ptr
        sub     qword rsp, 80 ; Allocate stack
    
      ; <>T42 = Call arrAlloc(size)
      ; In = { size }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrAlloc
        mov     qword r15, qword rax ; Assign return value to <>T42
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (size)
      ; Out = { <>T42, size }
      ; /
    
      ; _ = arr Assign <>T42
      ; In = { <>T42, size }
        mov     qword r13, qword r15
      ; Out = { arr, size }
      ; /
    
      ; _ = i Assign 0
      ; In = { arr, size }
        mov     qword r12, qword FALSE
      ; Out = { arr, i, size }
      ; /
    
      ; Jump .T44
      ; In = { arr, i, size }
        jmp     .T44
      ; Out = { arr, i, size }
      ; /
    
      ; .T43:
      ; In = { arr, i, size }
    .T43:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T46 = PreIncrement i
      ; In = { arr, i, size }
        inc     qword r12
      ; Out = { arr, i, size }
      ; /
    
      ; .T44:
      ; In = { arr, i, size }
    .T44:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T47 = i ComparisonLessThan size
      ; In = { arr, i, size }
        cmp     qword r12, qword rcx ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r15, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r15, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { arr, i, <>T47, size }
      ; /
    
      ; If !<>T47 Jump .T45
      ; In = { arr, i, <>T47, size }
        test    qword r15, qword r15 ; Set condition codes according to condition
        jz      .T45 ; Jump if condition is false/zero
      ; Out = { arr, i, size }
      ; /
    
      ; <>T48 = Call arrIndex(arr, i)
      ; In = { arr, i, size }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r12 ; Store live variable onto stack (i)
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    arrIndex
        mov     qword r11, qword rax ; Assign return value to <>T48
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r12, qword [rsp + 56] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack (size)
      ; Out = { <>T48, arr, i, size }
      ; /
    
      ; _ = <>T48 ReferenceAssign 0
      ; In = { <>T48, arr, i, size }
        mov     qword [r11], qword FALSE
      ; Out = { arr, i, size }
      ; /
    
      ; Jump .T43
      ; In = { arr, i, size }
        jmp     .T43
      ; Out = { arr, i, size }
      ; /
    
      ; .T45:
      ; In = { arr }
    .T45:         
      ; Out = { arr }
      ; /
    
      ; Return arr
      ; In = { arr }
        mov     qword rax, qword r13 ; Return arr
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
arrFree: ; arrFree(arr : ptr) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T49 = Call free(arr)
      ; In = { arr }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (arr)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        call    free
        mov     qword r15, qword rax ; Assign return value to <>T49
      ; Out = { <>T49 }
      ; /
    
      ; Return <>T49
      ; In = { <>T49 }
        mov     qword rax, qword r15 ; Return <>T49
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printArr: ; printArr(arr : ptr, size : long) : long
        sub     qword rsp, 80 ; Allocate stack
    
        mov     qword r13, qword rdx
      ; _ = i Assign 0
      ; In = { arr, size }
        mov     qword r14, qword FALSE
      ; Out = { arr, i, size }
      ; /
    
      ; Jump .T51
      ; In = { arr, i, size }
        jmp     .T51
      ; Out = { arr, i, size }
      ; /
    
      ; .T50:
      ; In = { arr, i, size }
    .T50:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T53 = PreIncrement i
      ; In = { arr, i, size }
        inc     qword r14
      ; Out = { arr, i, size }
      ; /
    
      ; .T51:
      ; In = { arr, i, size }
    .T51:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T54 = i ComparisonLessThan size
      ; In = { arr, i, size }
        cmp     qword r14, qword r13 ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { arr, i, <>T54, size }
      ; /
    
      ; If !<>T54 Jump .T52
      ; In = { arr, i, <>T54, size }
        test    qword r12, qword r12 ; Set condition codes according to condition
        jz      .T52 ; Jump if condition is false/zero
      ; Out = { arr, i, size }
      ; /
    
      ; <>T55 = i ComparisonGreaterThan 0
      ; In = { arr, i, size }
        cmp     qword r14, qword FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     qword r11, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r11, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; Out = { arr, i, <>T55, size }
      ; /
    
      ; If <>T55 Jump .T56
      ; In = { arr, i, <>T55, size }
        test    qword r11, qword r11 ; Set condition codes according to condition
        jnz     .T56 ; Jump if condition is true/non-zero
      ; Out = { arr, i, size }
      ; /
    
      ; Jump .T57
      ; In = { arr, i, size }
        jmp     .T57
      ; Out = { arr, i, size }
      ; /
    
      ; .T56:
      ; In = { arr, i, size }
    .T56:         
      ; Out = { arr, i, size }
      ; /
    
      ; _ = Call print(, , 2)
      ; In = { arr, i, size }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (i)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (size)
        mov     qword rcx, qword ptrC8 ; Pass parameter #0
        mov     qword rdx, qword longC4 ; Pass parameter #1
        call    print
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (i)
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack (size)
      ; Out = { arr, i, size }
      ; /
    
      ; .T57:
      ; In = { arr, i, size }
    .T57:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T59 = Call arrIndex(arr, i)
      ; In = { arr, i, size }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (i)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    arrIndex
        mov     qword r11, qword rax ; Assign return value to <>T59
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (i)
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack (size)
      ; Out = { <>T59, arr, i, size }
      ; /
    
      ; <>T58 = Dereference <>T59
      ; In = { <>T59, arr, i, size }
        mov     qword r12, [r11] ; Dereference <>T59
      ; Out = { <>T58, arr, i, size }
      ; /
    
      ; _ = Call printNum(<>T58)
      ; In = { <>T58, arr, i, size }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (<>T58)
        mov     qword [rsp + 56], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 48], qword r14 ; Store live variable onto stack (i)
        mov     qword [rsp + 40], qword r13 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    printNum
        mov     qword rcx, qword [rsp + 56] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 48] ; Restore live variable from stack (i)
        mov     qword r13, qword [rsp + 40] ; Restore live variable from stack (size)
      ; Out = { arr, i, size }
      ; /
    
      ; Jump .T50
      ; In = { arr, i, size }
        jmp     .T50
      ; Out = { arr, i, size }
      ; /
    
      ; .T52:
      ; In = {  }
    .T52:         
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
arrSize: ; arrSize(arr : ptr) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T62 = ArithmeticNegation 1
      ; In = { arr }
        mov     qword r14, qword TRUE ; Assign operand to target
        neg     qword r14
      ; Out = { arr, <>T62 }
      ; /
    
      ; <>T61 = Call arrIndex(arr, <>T62)
      ; In = { arr, <>T62 }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 40], qword r14 ; Store live variable onto stack (<>T62)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 40] ; Pass parameter #1
        call    arrIndex
        mov     qword r13, qword rax ; Assign return value to <>T61
      ; Out = { <>T61 }
      ; /
    
      ; <>T60 = Dereference <>T61
      ; In = { <>T61 }
        mov     qword r12, [r13] ; Dereference <>T61
      ; Out = { <>T60 }
      ; /
    
      ; Return <>T60
      ; In = { <>T60 }
        mov     qword rax, qword r12 ; Return <>T60
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
arrAlloc: ; arrAlloc(size : long) : ptr
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T64 = size Multiply 8
      ; In = { size }
        mov     qword r15, qword rcx ; Assign LHS to target memory
        imul    qword r15, qword longC9
      ; Out = { <>T64 }
      ; /
    
      ; <>T63 = Call alloc(<>T64)
      ; In = { <>T64 }
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack (<>T64)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        call    alloc
        mov     qword r13, qword rax ; Assign return value to <>T63
      ; Out = { <>T63 }
      ; /
    
      ; Return <>T63
      ; In = { <>T63 }
        mov     qword rax, qword r13 ; Return <>T63
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
arrIndex: ; arrIndex(arr : ptr, index : long) : ptr
        sub     qword rsp, 48 ; Allocate stack
    
        mov     qword r14, qword rdx
      ; <>T66 = arr Add 7
      ; In = { arr, index }
        mov     qword r15, qword rcx ; Assign LHS to target memory
        add     qword r15, qword longC10
      ; Out = { <>T66, index }
      ; /
    
      ; <>T67 = 8 Multiply index
      ; In = { <>T66, index }
        mov     qword r12, qword longC9 ; Assign LHS to target memory
        imul    qword r12, qword r14
      ; Out = { <>T66, <>T67 }
      ; /
    
      ; <>T65 = <>T66 Add <>T67
      ; In = { <>T66, <>T67 }
        mov     qword rcx, qword r15 ; Assign LHS to target memory
        add     qword rcx, qword r12
      ; Out = { <>T65 }
      ; /
    
      ; Return <>T65
      ; In = { <>T65 }
        mov     qword rax, qword rcx ; Return <>T65
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 48 ; Return stack
        ret     
    
alloc: ; alloc(size : long) : ptr
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T68 = Call GetProcessHeap()
      ; In = { size }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (size)
        call    GetProcessHeap
        mov     qword r15, qword rax ; Assign return value to <>T68
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack (size)
      ; Out = { <>T68, size }
      ; /
    
      ; _ = heapHandle Assign <>T68
      ; In = { <>T68, size }
        mov     qword r13, qword r15
      ; Out = { heapHandle, size }
      ; /
    
      ; <>T69 = Call HeapAlloc(heapHandle, 0, size)
      ; In = { heapHandle, size }
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (heapHandle)
        mov     qword [rsp + 40], qword rcx ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    HeapAlloc
        mov     qword r12, qword rax ; Assign return value to <>T69
      ; Out = { <>T69 }
      ; /
    
      ; Return <>T69
      ; In = { <>T69 }
        mov     qword rax, qword r12 ; Return <>T69
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
free: ; free(position : ptr) : bool
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T70 = Call GetProcessHeap()
      ; In = { position }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (position)
        call    GetProcessHeap
        mov     qword r15, qword rax ; Assign return value to <>T70
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack (position)
      ; Out = { <>T70, position }
      ; /
    
      ; _ = heapHandle Assign <>T70
      ; In = { <>T70, position }
        mov     qword r13, qword r15
      ; Out = { heapHandle, position }
      ; /
    
      ; <>T71 = Call HeapFree(heapHandle, 0, position)
      ; In = { heapHandle, position }
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (heapHandle)
        mov     qword [rsp + 40], qword rcx ; Store live variable onto stack (position)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    HeapFree
        mov     qword r12, qword rax ; Assign return value to <>T71
      ; Out = { <>T71 }
      ; /
    
      ; Return <>T71
      ; In = { <>T71 }
        mov     qword rax, qword r12 ; Return <>T71
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T72 = Call printNumAny(num, 10)
      ; In = { num }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (num)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword longC2 ; Pass parameter #1
        call    printNumAny
        mov     qword r15, qword rax ; Assign return value to <>T72
      ; Out = { <>T72 }
      ; /
    
      ; Return <>T72
      ; In = { <>T72 }
        mov     qword rax, qword r15 ; Return <>T72
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printNumAny: ; printNumAny(num : long, base : long) : long
        sub     qword rsp, 80 ; Allocate stack
    
        mov     qword r14, qword rdx
      ; <>T73 = num ComparisonLessThan 0
      ; In = { num, base }
        cmp     qword rcx, qword FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r13, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r13, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { num, base, <>T73 }
      ; /
    
      ; If <>T73 Jump .T74
      ; In = { num, base, <>T73 }
        test    qword r13, qword r13 ; Set condition codes according to condition
        jnz     .T74 ; Jump if condition is true/non-zero
      ; Out = { num, base }
      ; /
    
      ; Jump .T75
      ; In = { num, base }
        jmp     .T75
      ; Out = { num, base }
      ; /
    
      ; .T74:
      ; In = { num, base }
    .T74:         
      ; Out = { num, base }
      ; /
    
      ; _ = Call print(-, 1)
      ; In = { num, base }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (num)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (base)
        mov     qword rcx, qword ptrC11 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (num)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (base)
      ; Out = { num, base }
      ; /
    
      ; <>T76 = ArithmeticNegation num
      ; In = { num, base }
        mov     qword r13, qword rcx ; Assign operand to target
        neg     qword r13
      ; Out = { num, base, <>T76 }
      ; /
    
      ; _ = Call printNum(<>T76)
      ; In = { num, base, <>T76 }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (num)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (base)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (<>T76)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        call    printNum
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (num)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (base)
      ; Out = { num, base }
      ; /
    
      ; Return 
      ; In = { num, base }
        jmp     .__exit
      ; Out = { num, base }
      ; /
    
      ; .T75:
      ; In = { num, base }
    .T75:         
      ; Out = { num, base }
      ; /
    
      ; <>T77 = num Remainder base
      ; In = { num, base }
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword rcx ; Assign LHS to dividend
        idiv    qword r14 ; Assign remainder to RDX, quotient to RAX
        mov     qword r13, qword rdx ; Assign result to target memory
      ; Out = { <>T77, num, base }
      ; /
    
      ; _ = digit Assign <>T77
      ; In = { <>T77, num, base }
        mov     qword r12, qword r13
      ; Out = { digit, num, base }
      ; /
    
      ; <>T78 = num Divide base
      ; In = { digit, num, base }
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword rcx ; Assign LHS to dividend
        idiv    qword r14 ; Assign remainder to RDX, quotient to RAX
        mov     qword r11, qword rax ; Assign result to target memory
      ; Out = { digit, <>T78, base }
      ; /
    
      ; _ = rest Assign <>T78
      ; In = { digit, <>T78, base }
        mov     qword r13, qword r11
      ; Out = { digit, rest, base }
      ; /
    
      ; <>T79 = rest ComparisonGreaterThan 0
      ; In = { digit, rest, base }
        cmp     qword r13, qword FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     qword rcx, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword rcx, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; Out = { digit, rest, base, <>T79 }
      ; /
    
      ; If <>T79 Jump .T80
      ; In = { digit, rest, base, <>T79 }
        test    qword rcx, qword rcx ; Set condition codes according to condition
        jnz     .T80 ; Jump if condition is true/non-zero
      ; Out = { digit, rest, base }
      ; /
    
      ; Jump .T81
      ; In = { digit }
        jmp     .T81
      ; Out = { digit }
      ; /
    
      ; .T80:
      ; In = { digit, rest, base }
    .T80:         
      ; Out = { digit, rest, base }
      ; /
    
      ; _ = Call printNumAny(rest, base)
      ; In = { digit, rest, base }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (digit)
        mov     qword [rsp + 56], qword r13 ; Store live variable onto stack (rest)
        mov     qword [rsp + 48], qword r14 ; Store live variable onto stack (base)
        mov     qword rcx, qword [rsp + 56] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 48] ; Pass parameter #1
        call    printNumAny
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack (digit)
      ; Out = { digit }
      ; /
    
      ; .T81:
      ; In = { digit }
    .T81:         
      ; Out = { digit }
      ; /
    
      ; _ = Call printDigit(digit)
      ; In = { digit }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (digit)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    printDigit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
printDigit: ; printDigit(digit : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; _ = digits Assign 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
      ; In = { digit }
        mov     qword r15, qword ptrC12
      ; Out = { digits, digit }
      ; /
    
      ; <>T83 = digit Multiply 2
      ; In = { digits, digit }
        mov     qword r13, qword rcx ; Assign LHS to target memory
        imul    qword r13, qword longC4
      ; Out = { digits, <>T83 }
      ; /
    
      ; <>T82 = digits Add <>T83
      ; In = { digits, <>T83 }
        mov     qword r12, qword r15 ; Assign LHS to target memory
        add     qword r12, qword r13
      ; Out = { <>T82 }
      ; /
    
      ; _ = Call print(<>T82, 1)
      ; In = { <>T82 }
        mov     qword [rsp + 48], qword r12 ; Store live variable onto stack (<>T82)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
print: ; print(ptr : ptr, len : long) : long
        sub     qword rsp, 96 ; Allocate stack
    
        mov     qword r13, qword rdx
      ; <>T85 = ArithmeticNegation 11
      ; In = { ptr, len }
        mov     qword r15, qword longC13 ; Assign operand to target
        neg     qword r15
      ; Out = { <>T85, ptr, len }
      ; /
    
      ; <>T84 = Call GetStdHandle(<>T85)
      ; In = { <>T85, ptr, len }
        mov     qword [rsp + 80], qword r15 ; Store live variable onto stack (<>T85)
        mov     qword [rsp + 72], qword rcx ; Store live variable onto stack (ptr)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (len)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        call    GetStdHandle
        mov     qword r12, qword rax ; Assign return value to <>T84
        mov     qword rcx, qword [rsp + 72] ; Restore live variable from stack (ptr)
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (len)
      ; Out = { <>T84, ptr, len }
      ; /
    
      ; _ = stdOut Assign <>T84
      ; In = { <>T84, ptr, len }
        mov     qword r11, qword r12
      ; Out = { stdOut, ptr, len }
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
      ; In = { stdOut, ptr, len }
        mov     qword r15, qword FALSE
      ; Out = { stdOut, ptr, len, numberOfCharsWritten }
      ; /
    
      ; <>T87 = Reference numberOfCharsWritten
      ; In = { stdOut, ptr, len, numberOfCharsWritten }
        lea     qword r12, [r15] ; Create reference to numberOfCharsWritten
      ; Out = { stdOut, ptr, len, <>T87 }
      ; /
    
      ; <>T86 = Call WriteConsoleW(stdOut, ptr, len, <>T87, 0)
      ; In = { stdOut, ptr, len, <>T87 }
        mov     qword [rsp + 80], qword r11 ; Store live variable onto stack (stdOut)
        mov     qword [rsp + 72], qword rcx ; Store live variable onto stack (ptr)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (len)
        mov     qword [rsp + 56], qword r12 ; Store live variable onto stack (<>T87)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        mov     qword r8, qword [rsp + 64] ; Pass parameter #2
        mov     qword r9, qword [rsp + 56] ; Pass parameter #3
        mov     qword [rsp + 64], qword FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     qword r10, qword rax ; Assign return value to <>T86
      ; Out = { <>T86 }
      ; /
    
      ; Return <>T86
      ; In = { <>T86 }
        mov     qword rax, qword r10 ; Return <>T86
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 96 ; Return stack
        ret     
    
