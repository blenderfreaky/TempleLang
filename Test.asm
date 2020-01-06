section .data
FALSE: equ     0
TRUE: equ     1
ptrC0: db      __utf16__(`Start`)
longC1: equ     5
ptrC2: db      __utf16__(`\n`)
ptrC3: db      __utf16__(`-`)
longC4: equ     10
ptrC5: db      __utf16__(`0123456789`)
longC6: equ     2
longC7: equ     11
section .text
    global  _start
    extern  GetStdHandle
    extern  WriteConsoleW
_start: ; _start() : long
        sub     rsp, 32 ; Allocate stack
    
      ; _ = call print(Start, 5)
        mov     rcx, ptrC0 ; Pass parameter #0
        mov     rdx, longC1 ; Pass parameter #1
        call    print
      ; /
    
      ; _ = call print(\n, 1)
        mov     rcx, ptrC2 ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; _ = call print(\n, 1)
        mov     rcx, ptrC2 ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; .T1:
    .T1:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 32 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     rsp, 112 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp - 16], rcx ; Read parameter #0
      ; /
    
      ; <>T3 = num ComparisonLessThan 0
        mov     rbx, qword [rsp - 16] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG1:          ; Exit
        mov     qword [rsp - 24], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T3 Jump .T4:
        mov     rbx, qword [rsp - 24] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T4 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T5:
        jmp     .T5
      ; /
    
      ; .T4:
    .T4:         
      ; /
    
      ; _ = call print(-, 1)
        mov     rcx, ptrC3 ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; <>T7 = Negation num
        mov     rbx, qword [rsp - 16] ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp - 32], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call printNum(<>T7)
        mov     rcx, qword [rsp - 32] ; Pass parameter #0
        call    printNum
      ; /
    
      ; return 
        jmp     .__exit
      ; /
    
      ; .T6:
    .T6:         
      ; /
    
      ; .T5:
    .T5:         
      ; /
    
      ; <>T8 = num Remainder 10
        mov     rbx, qword [rsp - 16] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp - 16] ; Assign LHS to dividend
        mov     rbx, longC4 ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rdx ; Assign result to target memory
        mov     qword [rsp - 40], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = digit Assign <>T8
        mov     rbx, qword [rsp - 40] ; Move RHS into register so operation is possible
        mov     qword [rsp - 48], rbx
      ; /
    
      ; <>T9 = num Divide 10
        mov     rbx, qword [rsp - 16] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp - 16] ; Assign LHS to dividend
        mov     rbx, longC4 ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rax ; Assign result to target memory
        mov     qword [rsp - 56], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = rest Assign <>T9
        mov     rbx, qword [rsp - 56] ; Move RHS into register so operation is possible
        mov     qword [rsp - 64], rbx
      ; /
    
      ; <>T10 = rest ComparisonGreaterThan 0
        mov     rbx, qword [rsp - 64] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG3:          ; Exit
        mov     qword [rsp - 72], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T10 Jump .T11:
        mov     rbx, qword [rsp - 72] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T11 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T12:
        jmp     .T12
      ; /
    
      ; .T11:
    .T11:         
      ; /
    
      ; _ = call printNum(rest)
        mov     rcx, qword [rsp - 64] ; Pass parameter #0
        call    printNum
      ; /
    
      ; .T12:
    .T12:         
      ; /
    
      ; _ = call printDigit(digit)
        mov     rcx, qword [rsp - 48] ; Pass parameter #0
        call    printDigit
      ; /
    
      ; .T2:
    .T2:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 112 ; Return stack
        ret     
    
printDigit: ; printDigit(digit : long) : long
        sub     rsp, 80 ; Allocate stack
    
      ; digit = param 0
        mov     qword [rsp - 16], rcx ; Read parameter #0
      ; /
    
      ; _ = digits Assign 0123456789
        mov     qword [rsp - 24], ptrC5
      ; /
    
      ; <>T15 = digit Multiply 2
        mov     rbx, qword [rsp - 16] ; Move RHS into register so operation is possible
        imul    rbx, longC6
        mov     qword [rsp - 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T14 = digits Add <>T15
        mov     rbx, qword [rsp - 24] ; Move RHS into register so operation is possible
        add     rbx, qword [rsp - 32]
        mov     qword [rsp - 40], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call print(<>T14, 1)
        mov     rcx, qword [rsp - 40] ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; .T13:
    .T13:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 80 ; Return stack
        ret     
    
print: ; print(ptr : ptr, len : long) : long
        sub     rsp, 152 ; Allocate stack
    
      ; ptr = param 0
        mov     qword [rsp - 16], rcx ; Read parameter #0
      ; /
    
      ; len = param 1
        mov     qword [rsp - 24], rdx ; Read parameter #1
      ; /
    
      ; <>T18 = Negation 11
        mov     rbx, longC7 ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp - 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T17 = call GetStdHandle(<>T18)
        mov     rcx, qword [rsp - 32] ; Pass parameter #0
        call    GetStdHandle
        mov     qword [rsp - 48], rax ; Assign return value to <>T17
      ; /
    
      ; _ = stdOut Assign <>T17
        mov     rbx, qword [rsp - 48] ; Move RHS into register so operation is possible
        mov     qword [rsp - 40], rbx
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
        mov     qword [rsp - 56], FALSE
      ; /
    
      ; <>T20 = Reference numberOfCharsWritten
        mov     qword [rsp - 64], rsp ; Assign stackpointer
        sub     qword [rsp - 64], 56 ; and subtract to get correct position
      ; /
    
      ; <>T19 = call WriteConsoleW(stdOut, ptr, len, <>T20, 0)
        mov     rcx, qword [rsp - 40] ; Pass parameter #0
        mov     rdx, qword [rsp - 16] ; Pass parameter #1
        mov     r8, qword [rsp - 24] ; Pass parameter #2
        mov     r9, qword [rsp - 64] ; Pass parameter #3
        mov     qword [rsp - 152], FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     qword [rsp - 72], rax ; Assign return value to <>T19
      ; /
    
      ; return <>T19
        mov     rax, qword [rsp - 72] ; Return <>T19
        jmp     .__exit
      ; /
    
      ; .T16:
    .T16:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 152 ; Return stack
        ret     
    
