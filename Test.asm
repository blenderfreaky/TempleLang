section .data
FALSE: equ     0
TRUE: equ     1
ptrC0: dq      __utf16__(`Start\n`)
longC1: equ     6
longC2: equ     100000
longC3: equ     10
longC4: equ     14214
longC5: equ     2
ptrC6: dq      __utf16__(`\n`)
longC7: equ     94813
longC8: equ     42133
ptrC9: dq      __utf16__(`, `)
longC10: equ     8
longC11: equ     7
ptrC12: dq      __utf16__(`-`)
ptrC13: dq      __utf16__(`0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ`)
longC14: equ     11
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
    
      ; _ = i Assign 0
      ; In = {  }
        mov     qword r15, qword FALSE
      ; Out = { i }
      ; /
    
      ; Jump .T2
      ; In = { i }
        jmp     .T2
      ; Out = { i }
      ; /
    
      ; .T1:
      ; In = { i }
    .T1:         
      ; Out = { i }
      ; /
    
      ; <>T4 = PreIncrement i
      ; In = { i }
        inc     qword r15
      ; Out = { i }
      ; /
    
      ; .T2:
      ; In = { i }
    .T2:         
      ; Out = { i }
      ; /
    
      ; <>T5 = i ComparisonLessThan 100000
      ; In = { i }
        cmp     qword r15, qword longC2 ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r14, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r14, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { <>T5, i }
      ; /
    
      ; If !<>T5 Jump .T3
      ; In = { <>T5, i }
        test    qword r14, qword r14 ; Set condition codes according to condition
        jz      .T3 ; Jump if condition is false/zero
      ; Out = { i }
      ; /
    
      ; _ = Call arrAlloc(10)
      ; In = { i }
        mov     qword [rsp + 64], qword r15 ; Store live variable onto stack (i)
        mov     qword rcx, qword longC3 ; Pass parameter #0
        call    arrAlloc
        mov     qword r15, qword [rsp + 64] ; Restore live variable from stack (i)
      ; Out = { i }
      ; /
    
      ; Jump .T1
      ; In = { i }
        jmp     .T1
      ; Out = { i }
      ; /
    
      ; .T3:
      ; In = {  }
    .T3:         
      ; Out = {  }
      ; /
    
      ; _ = size Assign 10
      ; In = {  }
        mov     qword r14, qword longC3
      ; Out = { size }
      ; /
    
      ; <>T6 = Call arrInit(size)
      ; In = { size }
        mov     qword [rsp + 64], qword r14 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrInit
        mov     qword r13, qword rax ; Assign return value to <>T6
        mov     qword r14, qword [rsp + 64] ; Restore live variable from stack (size)
      ; Out = { <>T6, size }
      ; /
    
      ; _ = arr Assign <>T6
      ; In = { <>T6, size }
        mov     qword r12, qword r13
      ; Out = { arr, size }
      ; /
    
      ; _ = seed Assign 14214
      ; In = { arr, size }
        mov     qword r11, qword longC4
      ; Out = { arr, seed, size }
      ; /
    
      ; _ = i Assign 0
      ; In = { arr, seed, size }
        mov     qword r15, qword FALSE
      ; Out = { arr, seed, size, i }
      ; /
    
      ; Jump .T8
      ; In = { arr, seed, size, i }
        jmp     .T8
      ; Out = { arr, seed, size, i }
      ; /
    
      ; .T7:
      ; In = { arr, seed, size, i }
    .T7:         
      ; Out = { arr, seed, size, i }
      ; /
    
      ; <>T10 = PreIncrement i
      ; In = { arr, seed, size, i }
        inc     qword r15
      ; Out = { arr, seed, size, i }
      ; /
    
      ; .T8:
      ; In = { arr, seed, size, i }
    .T8:         
      ; Out = { arr, seed, size, i }
      ; /
    
      ; <>T11 = i ComparisonLessThan size
      ; In = { arr, seed, size, i }
        cmp     qword r15, qword r14 ; Set condition codes according to operands
        jl      .CG2 ; Jump to True if the comparison is true
        mov     qword r13, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r13, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; Out = { arr, seed, size, i, <>T11 }
      ; /
    
      ; If !<>T11 Jump .T9
      ; In = { arr, seed, size, i, <>T11 }
        test    qword r13, qword r13 ; Set condition codes according to condition
        jz      .T9 ; Jump if condition is false/zero
      ; Out = { arr, seed, size, i }
      ; /
    
      ; <>T12 = Call pseudoRandom(seed)
      ; In = { arr, seed, size, i }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r11 ; Store live variable onto stack (seed)
        mov     qword [rsp + 48], qword r14 ; Store live variable onto stack (size)
        mov     qword [rsp + 40], qword r15 ; Store live variable onto stack (i)
        mov     qword rcx, qword [rsp + 56] ; Pass parameter #0
        call    pseudoRandom
        mov     qword r10, qword rax ; Assign return value to <>T12
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 48] ; Restore live variable from stack (size)
        mov     qword r15, qword [rsp + 40] ; Restore live variable from stack (i)
      ; Out = { arr, <>T12, size, i }
      ; /
    
      ; _ = seed Assign <>T12
      ; In = { arr, <>T12, size, i }
        mov     qword r11, qword r10
      ; Out = { arr, seed, size, i }
      ; /
    
      ; <>T13 = Call arrIndex(arr, i)
      ; In = { arr, seed, size, i }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r11 ; Store live variable onto stack (seed)
        mov     qword [rsp + 48], qword r14 ; Store live variable onto stack (size)
        mov     qword [rsp + 40], qword r15 ; Store live variable onto stack (i)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 40] ; Pass parameter #1
        call    arrIndex
        mov     qword r13, qword rax ; Assign return value to <>T13
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 56] ; Restore live variable from stack (seed)
        mov     qword r14, qword [rsp + 48] ; Restore live variable from stack (size)
        mov     qword r15, qword [rsp + 40] ; Restore live variable from stack (i)
      ; Out = { <>T13, seed, size, arr, i }
      ; /
    
      ; <>T15 = size Multiply 2
      ; In = { <>T13, seed, size, arr, i }
        mov     qword r10, qword r14 ; Assign LHS to target memory
        imul    qword r10, qword longC5
      ; Out = { <>T13, seed, <>T15, arr, size, i }
      ; /
    
      ; <>T14 = seed Remainder <>T15
      ; In = { <>T13, seed, <>T15, arr, size, i }
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword r11 ; Assign LHS to dividend
        idiv    qword r10 ; Assign remainder to RDX, quotient to RAX
        mov     qword r9, qword rdx ; Assign result to target memory
      ; Out = { <>T13, <>T14, arr, seed, size, i }
      ; /
    
      ; _ = <>T13 ReferenceAssign <>T14
      ; In = { <>T13, <>T14, arr, seed, size, i }
        mov     qword [r13], qword r9
      ; Out = { arr, seed, size, i }
      ; /
    
      ; Jump .T7
      ; In = { arr, seed, size, i }
        jmp     .T7
      ; Out = { arr, seed, size, i }
      ; /
    
      ; .T9:
      ; In = { arr, size }
    .T9:         
      ; Out = { arr, size }
      ; /
    
      ; _ = Call printArr(arr, size)
      ; In = { arr, size }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    printArr
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (size)
      ; Out = { arr, size }
      ; /
    
      ; _ = Call print(\n, 1)
      ; In = { arr, size }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (size)
        mov     qword rcx, qword ptrC6 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (size)
      ; Out = { arr, size }
      ; /
    
      ; _ = Call quickSort(arr, size)
      ; In = { arr, size }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    quickSort
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (size)
      ; Out = { arr, size }
      ; /
    
      ; _ = Call printArr(arr, size)
      ; In = { arr, size }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    printArr
        mov     qword r12, qword [rsp + 64] ; Restore live variable from stack (arr)
      ; Out = { arr }
      ; /
    
      ; _ = Call arrFree(arr)
      ; In = { arr }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (arr)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrFree
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
pseudoRandom: ; pseudoRandom(seed : long) : long
        sub     qword rsp, 48 ; Allocate stack
    
      ; <>T17 = seed Multiply 94813
      ; In = { seed }
        mov     qword r15, qword rcx ; Assign LHS to target memory
        imul    qword r15, qword longC7
      ; Out = { <>T17 }
      ; /
    
      ; <>T16 = <>T17 Remainder 42133
      ; In = { <>T17 }
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword r15 ; Assign LHS to dividend
        mov     qword rbx, qword longC8 ; Move divisor into RBX, as a register is required for idiv
        idiv    qword rbx ; Assign remainder to RDX, quotient to RAX
        mov     qword r13, qword rdx ; Assign result to target memory
      ; Out = { <>T16 }
      ; /
    
      ; Return <>T16
      ; In = { <>T16 }
        mov     qword rax, qword r13 ; Return <>T16
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 48 ; Return stack
        ret     
    
quickSort: ; quickSort(arr : ptr, size : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
        mov     qword r13, qword rdx
      ; <>T19 = size Subtract 1
      ; In = { arr, size }
        mov     qword r14, qword r13 ; Assign LHS to target memory
        sub     qword r14, qword TRUE
      ; Out = { arr, <>T19 }
      ; /
    
      ; <>T18 = Call quickSortCore(arr, 0, <>T19)
      ; In = { arr, <>T19 }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 40], qword r14 ; Store live variable onto stack (<>T19)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    quickSortCore
        mov     qword r12, qword rax ; Assign return value to <>T18
      ; Out = { <>T18 }
      ; /
    
      ; Return <>T18
      ; In = { <>T18 }
        mov     qword rax, qword r12 ; Return <>T18
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
quickSortCore: ; quickSortCore(arr : ptr, lo : long, hi : long) : long
        sub     qword rsp, 96 ; Allocate stack
    
        mov     qword r13, qword rdx
      ; <>T20 = lo ComparisonGreaterThanOrEqual hi
      ; In = { arr, hi, lo }
        cmp     qword r13, qword r8 ; Set condition codes according to operands
        jge     .CG0 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { arr, hi, lo, <>T20 }
      ; /
    
      ; If <>T20 Jump .T21
      ; In = { arr, hi, lo, <>T20 }
        test    qword r12, qword r12 ; Set condition codes according to condition
        jnz     .T21 ; Jump if condition is true/non-zero
      ; Out = { arr, hi, lo }
      ; /
    
      ; Jump .T22
      ; In = { arr, hi, lo }
        jmp     .T22
      ; Out = { arr, hi, lo }
      ; /
    
      ; .T21:
      ; In = { arr, hi, lo }
    .T21:         
      ; Out = { arr, hi, lo }
      ; /
    
      ; Return 0
      ; In = { arr, hi, lo }
        mov     qword rax, qword FALSE ; Return 0
        jmp     .__exit
      ; Out = { arr, hi, lo }
      ; /
    
      ; .T22:
      ; In = { arr, hi, lo }
    .T22:         
      ; Out = { arr, hi, lo }
      ; /
    
      ; <>T23 = Call partition(arr, lo, hi)
      ; In = { arr, hi, lo }
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 72], qword r8 ; Store live variable onto stack (hi)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (lo)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 64] ; Pass parameter #1
        mov     qword r8, qword [rsp + 72] ; Pass parameter #2
        call    partition
        mov     qword r12, qword rax ; Assign return value to <>T23
        mov     qword rcx, qword [rsp + 80] ; Restore live variable from stack (arr)
        mov     qword r8, qword [rsp + 72] ; Restore live variable from stack (hi)
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (lo)
      ; Out = { arr, <>T23, hi, lo }
      ; /
    
      ; _ = partition Assign <>T23
      ; In = { arr, <>T23, hi, lo }
        mov     qword r11, qword r12
      ; Out = { arr, partition, hi, lo }
      ; /
    
      ; <>T24 = partition Subtract 1
      ; In = { arr, partition, hi, lo }
        mov     qword r10, qword r11 ; Assign LHS to target memory
        sub     qword r10, qword TRUE
      ; Out = { arr, partition, hi, lo, <>T24 }
      ; /
    
      ; _ = Call quickSortCore(arr, lo, <>T24)
      ; In = { arr, partition, hi, lo, <>T24 }
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 72], qword r11 ; Store live variable onto stack (partition)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword [rsp + 56], qword r13 ; Store live variable onto stack (lo)
        mov     qword [rsp + 48], qword r10 ; Store live variable onto stack (<>T24)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        mov     qword r8, qword [rsp + 48] ; Pass parameter #2
        call    quickSortCore
        mov     qword rcx, qword [rsp + 80] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 72] ; Restore live variable from stack (partition)
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack (hi)
      ; Out = { arr, partition, hi }
      ; /
    
      ; <>T25 = partition Add 1
      ; In = { arr, partition, hi }
        mov     qword r12, qword r11 ; Assign LHS to target memory
        add     qword r12, qword TRUE
      ; Out = { arr, <>T25, hi }
      ; /
    
      ; _ = Call quickSortCore(arr, <>T25, hi)
      ; In = { arr, <>T25, hi }
        mov     qword [rsp + 80], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 72], qword r12 ; Store live variable onto stack (<>T25)
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
      ; <>T27 = Call arrIndex(arr, hi)
      ; In = { arr, lo, hi }
        mov     qword [rsp + 96], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 88], qword r14 ; Store live variable onto stack (lo)
        mov     qword [rsp + 80], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 96] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 80] ; Pass parameter #1
        call    arrIndex
        mov     qword r13, qword rax ; Assign return value to <>T27
        mov     qword rcx, qword [rsp + 96] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 88] ; Restore live variable from stack (lo)
        mov     qword r8, qword [rsp + 80] ; Restore live variable from stack (hi)
      ; Out = { arr, lo, <>T27, hi }
      ; /
    
      ; <>T26 = Dereference <>T27
      ; In = { arr, lo, <>T27, hi }
        mov     qword r11, [r13] ; Dereference <>T27
      ; Out = { arr, lo, <>T26, hi }
      ; /
    
      ; _ = pivot Assign <>T26
      ; In = { arr, lo, <>T26, hi }
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
    
      ; Jump .T29
      ; In = { i, arr, j, pivot, hi }
        jmp     .T29
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T28:
      ; In = { i, arr, j, pivot, hi }
    .T28:         
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; <>T31 = PreIncrement j
      ; In = { i, arr, j, pivot, hi }
        inc     qword r11
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T29:
      ; In = { i, arr, j, pivot, hi }
    .T29:         
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; <>T32 = j ComparisonLessThanOrEqual hi
      ; In = { i, arr, j, pivot, hi }
        cmp     qword r11, qword r8 ; Set condition codes according to operands
        jle     .CG0 ; Jump to True if the comparison is true
        mov     qword r14, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r14, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { i, arr, j, pivot, hi, <>T32 }
      ; /
    
      ; If !<>T32 Jump .T30
      ; In = { i, arr, j, pivot, hi, <>T32 }
        test    qword r14, qword r14 ; Set condition codes according to condition
        jz      .T30 ; Jump if condition is false/zero
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; <>T34 = Call arrIndex(arr, j)
      ; In = { i, arr, j, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r11 ; Store live variable onto stack (j)
        mov     qword [rsp + 72], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 80] ; Pass parameter #1
        call    arrIndex
        mov     qword r9, qword rax ; Assign return value to <>T34
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 80] ; Restore live variable from stack (j)
        mov     qword r10, qword [rsp + 72] ; Restore live variable from stack (pivot)
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack (hi)
      ; Out = { i, arr, <>T34, j, pivot, hi }
      ; /
    
      ; <>T33 = Dereference <>T34
      ; In = { i, arr, <>T34, j, pivot, hi }
        mov     qword r14, [r9] ; Dereference <>T34
      ; Out = { i, arr, <>T33, j, pivot, hi }
      ; /
    
      ; _ = elem Assign <>T33
      ; In = { i, arr, <>T33, j, pivot, hi }
        mov     qword [rsp + 120], qword r14
      ; Out = { i, arr, elem, j, pivot, hi }
      ; /
    
      ; <>T35 = elem ComparisonLessThan pivot
      ; In = { i, arr, elem, j, pivot, hi }
        cmp     qword [rsp + 120], qword r10 ; Set condition codes according to operands
        jl      .CG2 ; Jump to True if the comparison is true
        mov     qword r9, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r9, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; Out = { i, arr, elem, j, <>T35, pivot, hi }
      ; /
    
      ; If <>T35 Jump .T36
      ; In = { i, arr, elem, j, <>T35, pivot, hi }
        test    qword r9, qword r9 ; Set condition codes according to condition
        jnz     .T36 ; Jump if condition is true/non-zero
      ; Out = { i, arr, elem, j, pivot, hi }
      ; /
    
      ; Jump .T37
      ; In = { i, arr, j, pivot, hi }
        jmp     .T37
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T36:
      ; In = { i, arr, elem, j, pivot, hi }
    .T36:         
      ; Out = { i, arr, elem, j, pivot, hi }
      ; /
    
      ; <>T38 = Call arrIndex(arr, j)
      ; In = { i, arr, elem, j, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r11 ; Store live variable onto stack (j)
        mov     qword [rsp + 72], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 80] ; Pass parameter #1
        call    arrIndex
        mov     qword r9, qword rax ; Assign return value to <>T38
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 80] ; Restore live variable from stack (j)
        mov     qword r10, qword [rsp + 72] ; Restore live variable from stack (pivot)
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack (hi)
      ; Out = { i, arr, elem, <>T38, j, pivot, hi }
      ; /
    
      ; <>T40 = Call arrIndex(arr, i)
      ; In = { i, arr, elem, <>T38, j, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r9 ; Store live variable onto stack (<>T38)
        mov     qword [rsp + 72], qword r11 ; Store live variable onto stack (j)
        mov     qword [rsp + 64], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 56], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r14, qword rax ; Assign return value to <>T40
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r9, qword [rsp + 80] ; Restore live variable from stack (<>T38)
        mov     qword r11, qword [rsp + 72] ; Restore live variable from stack (j)
        mov     qword r10, qword [rsp + 64] ; Restore live variable from stack (pivot)
        mov     qword r8, qword [rsp + 56] ; Restore live variable from stack (hi)
      ; Out = { i, arr, elem, <>T38, <>T40, j, pivot, hi }
      ; /
    
      ; <>T39 = Dereference <>T40
      ; In = { i, arr, elem, <>T38, <>T40, j, pivot, hi }
        mov     qword rbx, qword r14 ; Move RHS into register so operation is possible
        mov     qword rbx, [rbx] ; Dereference <>T40
        mov     qword [rsp + 112], qword rbx ; Assign result to actual target memory
      ; Out = { i, arr, elem, <>T38, <>T39, j, pivot, hi }
      ; /
    
      ; _ = <>T38 ReferenceAssign <>T39
      ; In = { i, arr, elem, <>T38, <>T39, j, pivot, hi }
        mov     qword rbx, qword [rsp + 112]
        mov     qword [r9], qword rbx
      ; Out = { i, arr, elem, j, pivot, hi }
      ; /
    
      ; <>T41 = Call arrIndex(arr, i)
      ; In = { i, arr, elem, j, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r11 ; Store live variable onto stack (j)
        mov     qword [rsp + 72], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 64], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r14, qword rax ; Assign return value to <>T41
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r11, qword [rsp + 80] ; Restore live variable from stack (j)
        mov     qword r10, qword [rsp + 72] ; Restore live variable from stack (pivot)
        mov     qword r8, qword [rsp + 64] ; Restore live variable from stack (hi)
      ; Out = { i, <>T41, elem, arr, j, pivot, hi }
      ; /
    
      ; _ = <>T41 ReferenceAssign elem
      ; In = { i, <>T41, elem, arr, j, pivot, hi }
        mov     qword rbx, qword [rsp + 120]
        mov     qword [r14], qword rbx
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; <>T42 = i Add 1
      ; In = { i, arr, j, pivot, hi }
        mov     qword r9, qword r13 ; Assign LHS to target memory
        add     qword r9, qword TRUE
      ; Out = { <>T42, arr, j, pivot, hi }
      ; /
    
      ; _ = i Assign <>T42
      ; In = { <>T42, arr, j, pivot, hi }
        mov     qword r13, qword r9
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T37:
      ; In = { i, arr, j, pivot, hi }
    .T37:         
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; Jump .T28
      ; In = { i, arr, j, pivot, hi }
        jmp     .T28
      ; Out = { i, arr, j, pivot, hi }
      ; /
    
      ; .T30:
      ; In = { i, arr, pivot, hi }
    .T30:         
      ; Out = { i, arr, pivot, hi }
      ; /
    
      ; <>T43 = Call arrIndex(arr, hi)
      ; In = { i, arr, pivot, hi }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 72], qword r8 ; Store live variable onto stack (hi)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        call    arrIndex
        mov     qword r9, qword rax ; Assign return value to <>T43
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r10, qword [rsp + 80] ; Restore live variable from stack (pivot)
      ; Out = { i, arr, pivot, <>T43 }
      ; /
    
      ; <>T45 = Call arrIndex(arr, i)
      ; In = { i, arr, pivot, <>T43 }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r10 ; Store live variable onto stack (pivot)
        mov     qword [rsp + 72], qword r9 ; Store live variable onto stack (<>T43)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r14, qword rax ; Assign return value to <>T45
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 88] ; Restore live variable from stack (arr)
        mov     qword r10, qword [rsp + 80] ; Restore live variable from stack (pivot)
        mov     qword r9, qword [rsp + 72] ; Restore live variable from stack (<>T43)
      ; Out = { i, arr, pivot, <>T43, <>T45 }
      ; /
    
      ; <>T44 = Dereference <>T45
      ; In = { i, arr, pivot, <>T43, <>T45 }
        mov     qword r8, [r14] ; Dereference <>T45
      ; Out = { i, arr, pivot, <>T43, <>T44 }
      ; /
    
      ; _ = <>T43 ReferenceAssign <>T44
      ; In = { i, arr, pivot, <>T43, <>T44 }
        mov     qword [r9], qword r8
      ; Out = { i, arr, pivot }
      ; /
    
      ; <>T46 = Call arrIndex(arr, i)
      ; In = { i, arr, pivot }
        mov     qword [rsp + 96], qword r13 ; Store live variable onto stack (i)
        mov     qword [rsp + 88], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 80], qword r10 ; Store live variable onto stack (pivot)
        mov     qword rcx, qword [rsp + 88] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 96] ; Pass parameter #1
        call    arrIndex
        mov     qword r14, qword rax ; Assign return value to <>T46
        mov     qword r13, qword [rsp + 96] ; Restore live variable from stack (i)
        mov     qword r10, qword [rsp + 80] ; Restore live variable from stack (pivot)
      ; Out = { i, <>T46, pivot }
      ; /
    
      ; _ = <>T46 ReferenceAssign pivot
      ; In = { i, <>T46, pivot }
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
    
      ; <>T47 = Call arrAlloc(size)
      ; In = { size }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        call    arrAlloc
        mov     qword r15, qword rax ; Assign return value to <>T47
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (size)
      ; Out = { <>T47, size }
      ; /
    
      ; _ = arr Assign <>T47
      ; In = { <>T47, size }
        mov     qword r13, qword r15
      ; Out = { arr, size }
      ; /
    
      ; _ = i Assign 0
      ; In = { arr, size }
        mov     qword r12, qword FALSE
      ; Out = { arr, i, size }
      ; /
    
      ; Jump .T49
      ; In = { arr, i, size }
        jmp     .T49
      ; Out = { arr, i, size }
      ; /
    
      ; .T48:
      ; In = { arr, i, size }
    .T48:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T51 = PreIncrement i
      ; In = { arr, i, size }
        inc     qword r12
      ; Out = { arr, i, size }
      ; /
    
      ; .T49:
      ; In = { arr, i, size }
    .T49:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T52 = i ComparisonLessThan size
      ; In = { arr, i, size }
        cmp     qword r12, qword rcx ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r15, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r15, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { arr, i, <>T52, size }
      ; /
    
      ; If !<>T52 Jump .T50
      ; In = { arr, i, <>T52, size }
        test    qword r15, qword r15 ; Set condition codes according to condition
        jz      .T50 ; Jump if condition is false/zero
      ; Out = { arr, i, size }
      ; /
    
      ; <>T53 = Call arrIndex(arr, i)
      ; In = { arr, i, size }
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r12 ; Store live variable onto stack (i)
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    arrIndex
        mov     qword r11, qword rax ; Assign return value to <>T53
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r12, qword [rsp + 56] ; Restore live variable from stack (i)
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack (size)
      ; Out = { <>T53, arr, i, size }
      ; /
    
      ; _ = <>T53 ReferenceAssign 0
      ; In = { <>T53, arr, i, size }
        mov     qword [r11], qword FALSE
      ; Out = { arr, i, size }
      ; /
    
      ; Jump .T48
      ; In = { arr, i, size }
        jmp     .T48
      ; Out = { arr, i, size }
      ; /
    
      ; .T50:
      ; In = { arr }
    .T50:         
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
    
      ; <>T54 = Call free(arr)
      ; In = { arr }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (arr)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        call    free
        mov     qword r15, qword rax ; Assign return value to <>T54
      ; Out = { <>T54 }
      ; /
    
      ; Return <>T54
      ; In = { <>T54 }
        mov     qword rax, qword r15 ; Return <>T54
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
    
      ; Jump .T56
      ; In = { arr, i, size }
        jmp     .T56
      ; Out = { arr, i, size }
      ; /
    
      ; .T55:
      ; In = { arr, i, size }
    .T55:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T58 = PreIncrement i
      ; In = { arr, i, size }
        inc     qword r14
      ; Out = { arr, i, size }
      ; /
    
      ; .T56:
      ; In = { arr, i, size }
    .T56:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T59 = i ComparisonLessThan size
      ; In = { arr, i, size }
        cmp     qword r14, qword r13 ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r12, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r12, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { arr, i, <>T59, size }
      ; /
    
      ; If !<>T59 Jump .T57
      ; In = { arr, i, <>T59, size }
        test    qword r12, qword r12 ; Set condition codes according to condition
        jz      .T57 ; Jump if condition is false/zero
      ; Out = { arr, i, size }
      ; /
    
      ; <>T60 = i ComparisonGreaterThan 0
      ; In = { arr, i, size }
        cmp     qword r14, qword FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     qword r11, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword r11, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; Out = { arr, i, <>T60, size }
      ; /
    
      ; If <>T60 Jump .T61
      ; In = { arr, i, <>T60, size }
        test    qword r11, qword r11 ; Set condition codes according to condition
        jnz     .T61 ; Jump if condition is true/non-zero
      ; Out = { arr, i, size }
      ; /
    
      ; Jump .T62
      ; In = { arr, i, size }
        jmp     .T62
      ; Out = { arr, i, size }
      ; /
    
      ; .T61:
      ; In = { arr, i, size }
    .T61:         
      ; Out = { arr, i, size }
      ; /
    
      ; _ = Call print(, , 2)
      ; In = { arr, i, size }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (i)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (size)
        mov     qword rcx, qword ptrC9 ; Pass parameter #0
        mov     qword rdx, qword longC5 ; Pass parameter #1
        call    print
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (i)
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack (size)
      ; Out = { arr, i, size }
      ; /
    
      ; .T62:
      ; In = { arr, i, size }
    .T62:         
      ; Out = { arr, i, size }
      ; /
    
      ; <>T64 = Call arrIndex(arr, i)
      ; In = { arr, i, size }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (i)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 64] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 56] ; Pass parameter #1
        call    arrIndex
        mov     qword r11, qword rax ; Assign return value to <>T64
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (arr)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (i)
        mov     qword r13, qword [rsp + 48] ; Restore live variable from stack (size)
      ; Out = { <>T64, arr, i, size }
      ; /
    
      ; <>T63 = Dereference <>T64
      ; In = { <>T64, arr, i, size }
        mov     qword r12, [r11] ; Dereference <>T64
      ; Out = { <>T63, arr, i, size }
      ; /
    
      ; _ = Call printNum(<>T63)
      ; In = { <>T63, arr, i, size }
        mov     qword [rsp + 64], qword r12 ; Store live variable onto stack (<>T63)
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
    
      ; Jump .T55
      ; In = { arr, i, size }
        jmp     .T55
      ; Out = { arr, i, size }
      ; /
    
      ; .T57:
      ; In = {  }
    .T57:         
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 80 ; Return stack
        ret     
    
arrSize: ; arrSize(arr : ptr) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T67 = ArithmeticNegation 1
      ; In = { arr }
        mov     qword r14, qword TRUE ; Assign operand to target
        neg     qword r14
      ; Out = { arr, <>T67 }
      ; /
    
      ; <>T66 = Call arrIndex(arr, <>T67)
      ; In = { arr, <>T67 }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (arr)
        mov     qword [rsp + 40], qword r14 ; Store live variable onto stack (<>T67)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 40] ; Pass parameter #1
        call    arrIndex
        mov     qword r13, qword rax ; Assign return value to <>T66
      ; Out = { <>T66 }
      ; /
    
      ; <>T65 = Dereference <>T66
      ; In = { <>T66 }
        mov     qword r12, [r13] ; Dereference <>T66
      ; Out = { <>T65 }
      ; /
    
      ; Return <>T65
      ; In = { <>T65 }
        mov     qword rax, qword r12 ; Return <>T65
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
arrAlloc: ; arrAlloc(size : long) : ptr
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T69 = size Multiply 8
      ; In = { size }
        mov     qword r15, qword rcx ; Assign LHS to target memory
        imul    qword r15, qword longC10
      ; Out = { <>T69 }
      ; /
    
      ; <>T68 = Call alloc(<>T69)
      ; In = { <>T69 }
        mov     qword [rsp + 48], qword r15 ; Store live variable onto stack (<>T69)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        call    alloc
        mov     qword r13, qword rax ; Assign return value to <>T68
      ; Out = { <>T68 }
      ; /
    
      ; Return <>T68
      ; In = { <>T68 }
        mov     qword rax, qword r13 ; Return <>T68
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
arrIndex: ; arrIndex(arr : ptr, index : long) : ptr
        sub     qword rsp, 48 ; Allocate stack
    
        mov     qword r14, qword rdx
      ; <>T71 = arr Add 7
      ; In = { arr, index }
        mov     qword r15, qword rcx ; Assign LHS to target memory
        add     qword r15, qword longC11
      ; Out = { <>T71, index }
      ; /
    
      ; <>T72 = 8 Multiply index
      ; In = { <>T71, index }
        mov     qword r12, qword longC10 ; Assign LHS to target memory
        imul    qword r12, qword r14
      ; Out = { <>T71, <>T72 }
      ; /
    
      ; <>T70 = <>T71 Add <>T72
      ; In = { <>T71, <>T72 }
        mov     qword rcx, qword r15 ; Assign LHS to target memory
        add     qword rcx, qword r12
      ; Out = { <>T70 }
      ; /
    
      ; Return <>T70
      ; In = { <>T70 }
        mov     qword rax, qword rcx ; Return <>T70
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 48 ; Return stack
        ret     
    
alloc: ; alloc(size : long) : ptr
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T73 = Call GetProcessHeap()
      ; In = { size }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (size)
        call    GetProcessHeap
        mov     qword r15, qword rax ; Assign return value to <>T73
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack (size)
      ; Out = { <>T73, size }
      ; /
    
      ; _ = heapHandle Assign <>T73
      ; In = { <>T73, size }
        mov     qword r13, qword r15
      ; Out = { heapHandle, size }
      ; /
    
      ; <>T74 = Call HeapAlloc(heapHandle, 0, size)
      ; In = { heapHandle, size }
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (heapHandle)
        mov     qword [rsp + 40], qword rcx ; Store live variable onto stack (size)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    HeapAlloc
        mov     qword r12, qword rax ; Assign return value to <>T74
      ; Out = { <>T74 }
      ; /
    
      ; Return <>T74
      ; In = { <>T74 }
        mov     qword rax, qword r12 ; Return <>T74
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
free: ; free(position : ptr) : bool
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T75 = Call GetProcessHeap()
      ; In = { position }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (position)
        call    GetProcessHeap
        mov     qword r15, qword rax ; Assign return value to <>T75
        mov     qword rcx, qword [rsp + 48] ; Restore live variable from stack (position)
      ; Out = { <>T75, position }
      ; /
    
      ; _ = heapHandle Assign <>T75
      ; In = { <>T75, position }
        mov     qword r13, qword r15
      ; Out = { heapHandle, position }
      ; /
    
      ; <>T76 = Call HeapFree(heapHandle, 0, position)
      ; In = { heapHandle, position }
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (heapHandle)
        mov     qword [rsp + 40], qword rcx ; Store live variable onto stack (position)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword FALSE ; Pass parameter #1
        mov     qword r8, qword [rsp + 40] ; Pass parameter #2
        call    HeapFree
        mov     qword r12, qword rax ; Assign return value to <>T76
      ; Out = { <>T76 }
      ; /
    
      ; Return <>T76
      ; In = { <>T76 }
        mov     qword rax, qword r12 ; Return <>T76
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     qword rsp, 64 ; Allocate stack
    
      ; <>T77 = Call printNumAny(num, 10)
      ; In = { num }
        mov     qword [rsp + 48], qword rcx ; Store live variable onto stack (num)
        mov     qword rcx, qword [rsp + 48] ; Pass parameter #0
        mov     qword rdx, qword longC3 ; Pass parameter #1
        call    printNumAny
        mov     qword r15, qword rax ; Assign return value to <>T77
      ; Out = { <>T77 }
      ; /
    
      ; Return <>T77
      ; In = { <>T77 }
        mov     qword rax, qword r15 ; Return <>T77
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 64 ; Return stack
        ret     
    
printNumAny: ; printNumAny(num : long, base : long) : long
        sub     qword rsp, 80 ; Allocate stack
    
        mov     qword r14, qword rdx
      ; <>T78 = num ComparisonLessThan 0
      ; In = { num, base }
        cmp     qword rcx, qword FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     qword r13, qword FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     qword r13, qword TRUE ; Assign true to output
    .CG1:          ; Exit
      ; Out = { num, base, <>T78 }
      ; /
    
      ; If <>T78 Jump .T79
      ; In = { num, base, <>T78 }
        test    qword r13, qword r13 ; Set condition codes according to condition
        jnz     .T79 ; Jump if condition is true/non-zero
      ; Out = { num, base }
      ; /
    
      ; Jump .T80
      ; In = { num, base }
        jmp     .T80
      ; Out = { num, base }
      ; /
    
      ; .T79:
      ; In = { num, base }
    .T79:         
      ; Out = { num, base }
      ; /
    
      ; _ = Call print(-, 1)
      ; In = { num, base }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (num)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (base)
        mov     qword rcx, qword ptrC12 ; Pass parameter #0
        mov     qword rdx, qword TRUE ; Pass parameter #1
        call    print
        mov     qword rcx, qword [rsp + 64] ; Restore live variable from stack (num)
        mov     qword r14, qword [rsp + 56] ; Restore live variable from stack (base)
      ; Out = { num, base }
      ; /
    
      ; <>T81 = ArithmeticNegation num
      ; In = { num, base }
        mov     qword r13, qword rcx ; Assign operand to target
        neg     qword r13
      ; Out = { num, base, <>T81 }
      ; /
    
      ; _ = Call printNum(<>T81)
      ; In = { num, base, <>T81 }
        mov     qword [rsp + 64], qword rcx ; Store live variable onto stack (num)
        mov     qword [rsp + 56], qword r14 ; Store live variable onto stack (base)
        mov     qword [rsp + 48], qword r13 ; Store live variable onto stack (<>T81)
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
    
      ; .T80:
      ; In = { num, base }
    .T80:         
      ; Out = { num, base }
      ; /
    
      ; <>T82 = num Remainder base
      ; In = { num, base }
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword rcx ; Assign LHS to dividend
        idiv    qword r14 ; Assign remainder to RDX, quotient to RAX
        mov     qword r13, qword rdx ; Assign result to target memory
      ; Out = { <>T82, num, base }
      ; /
    
      ; _ = digit Assign <>T82
      ; In = { <>T82, num, base }
        mov     qword r12, qword r13
      ; Out = { digit, num, base }
      ; /
    
      ; <>T83 = num Divide base
      ; In = { digit, num, base }
        xor     qword rdx, qword rdx ; Empty out higher bits of dividend
        mov     qword rax, qword rcx ; Assign LHS to dividend
        idiv    qword r14 ; Assign remainder to RDX, quotient to RAX
        mov     qword r11, qword rax ; Assign result to target memory
      ; Out = { digit, <>T83, base }
      ; /
    
      ; _ = rest Assign <>T83
      ; In = { digit, <>T83, base }
        mov     qword r13, qword r11
      ; Out = { digit, rest, base }
      ; /
    
      ; <>T84 = rest ComparisonGreaterThan 0
      ; In = { digit, rest, base }
        cmp     qword r13, qword FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     qword rcx, qword FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     qword rcx, qword TRUE ; Assign true to output
    .CG3:          ; Exit
      ; Out = { digit, rest, base, <>T84 }
      ; /
    
      ; If <>T84 Jump .T85
      ; In = { digit, rest, base, <>T84 }
        test    qword rcx, qword rcx ; Set condition codes according to condition
        jnz     .T85 ; Jump if condition is true/non-zero
      ; Out = { digit, rest, base }
      ; /
    
      ; Jump .T86
      ; In = { digit }
        jmp     .T86
      ; Out = { digit }
      ; /
    
      ; .T85:
      ; In = { digit, rest, base }
    .T85:         
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
    
      ; .T86:
      ; In = { digit }
    .T86:         
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
        mov     qword r15, qword ptrC13
      ; Out = { digits, digit }
      ; /
    
      ; <>T88 = digit Multiply 2
      ; In = { digits, digit }
        mov     qword r13, qword rcx ; Assign LHS to target memory
        imul    qword r13, qword longC5
      ; Out = { digits, <>T88 }
      ; /
    
      ; <>T87 = digits Add <>T88
      ; In = { digits, <>T88 }
        mov     qword r12, qword r15 ; Assign LHS to target memory
        add     qword r12, qword r13
      ; Out = { <>T87 }
      ; /
    
      ; _ = Call print(<>T87, 1)
      ; In = { <>T87 }
        mov     qword [rsp + 48], qword r12 ; Store live variable onto stack (<>T87)
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
      ; <>T90 = ArithmeticNegation 11
      ; In = { ptr, len }
        mov     qword r15, qword longC14 ; Assign operand to target
        neg     qword r15
      ; Out = { <>T90, ptr, len }
      ; /
    
      ; <>T89 = Call GetStdHandle(<>T90)
      ; In = { <>T90, ptr, len }
        mov     qword [rsp + 80], qword r15 ; Store live variable onto stack (<>T90)
        mov     qword [rsp + 72], qword rcx ; Store live variable onto stack (ptr)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (len)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        call    GetStdHandle
        mov     qword r12, qword rax ; Assign return value to <>T89
        mov     qword rcx, qword [rsp + 72] ; Restore live variable from stack (ptr)
        mov     qword r13, qword [rsp + 64] ; Restore live variable from stack (len)
      ; Out = { <>T89, ptr, len }
      ; /
    
      ; _ = stdOut Assign <>T89
      ; In = { <>T89, ptr, len }
        mov     qword r11, qword r12
      ; Out = { stdOut, ptr, len }
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
      ; In = { stdOut, ptr, len }
        mov     qword r15, qword FALSE
      ; Out = { stdOut, ptr, len, numberOfCharsWritten }
      ; /
    
      ; <>T92 = Reference numberOfCharsWritten
      ; In = { stdOut, ptr, len, numberOfCharsWritten }
        lea     qword r12, [r15] ; Create reference to numberOfCharsWritten
      ; Out = { stdOut, ptr, len, <>T92 }
      ; /
    
      ; <>T91 = Call WriteConsoleW(stdOut, ptr, len, <>T92, 0)
      ; In = { stdOut, ptr, len, <>T92 }
        mov     qword [rsp + 80], qword r11 ; Store live variable onto stack (stdOut)
        mov     qword [rsp + 72], qword rcx ; Store live variable onto stack (ptr)
        mov     qword [rsp + 64], qword r13 ; Store live variable onto stack (len)
        mov     qword [rsp + 56], qword r12 ; Store live variable onto stack (<>T92)
        mov     qword rcx, qword [rsp + 80] ; Pass parameter #0
        mov     qword rdx, qword [rsp + 72] ; Pass parameter #1
        mov     qword r8, qword [rsp + 64] ; Pass parameter #2
        mov     qword r9, qword [rsp + 56] ; Pass parameter #3
        mov     qword [rsp + 64], qword FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     qword r10, qword rax ; Assign return value to <>T91
      ; Out = { <>T91 }
      ; /
    
      ; Return <>T91
      ; In = { <>T91 }
        mov     qword rax, qword r10 ; Return <>T91
        jmp     .__exit
      ; Out = {  }
      ; /
    
    .__exit:          ; Function exit/return label
        add     qword rsp, 96 ; Return stack
        ret     
    
