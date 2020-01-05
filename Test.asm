section .data
FALSE: equ     0
TRUE: equ     1
ptrC0: db      __utf16__(`\n`)
longC1: equ     10
ptrC2: db      __utf16__(`-`)
ptrC3: db      __utf16__(`0123456789`)
longC4: equ     2
longC5: equ     11
section .text
    global  _start
    extern  GetStdHandle
    extern  WriteConsoleW
_start: ; _start() : long
        sub     RSP, 48 ; Allocate stack
    
      ; _ = call print(\n, 1)
        mov     RCX, ptrC0 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; _ = call print(\n, 1)
        mov     RCX, ptrC0 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; _ = i Assign 0
        mov     R15, FALSE
      ; /
    
      ; Jump .T3:
        jmp     .T3
      ; /
    
      ; .T2:
    .T2:         
      ; /
    
      ; <>T5 = PostIncrement i
        mov     R14, R15 ; Assign operand to target
        inc     R15
      ; /
    
      ; .T3:
    .T3:         
      ; /
    
      ; <>T6 = i ComparisonLessThan 10
        cmp     R15, longC1 ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     R14, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     R14, TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If !<>T6 Jump .T4:
        test    R14, R14 ; Set condition codes according to condition
        jz      .T4 ; Jump if condition is false/zero
      ; /
    
      ; _ = call printNum(i)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     RCX, R15 ; Pass parameter #0
        call    printNum
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
      ; /
    
      ; _ = call print(\n, 1)
        mov     RCX, ptrC0 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; .T7:
    .T7:         
      ; /
    
      ; Jump .T2:
        jmp     .T2
      ; /
    
      ; .T4:
    .T4:         
      ; /
    
      ; _ = call print(\n, 1)
        mov     RCX, ptrC0 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; .T1:
    .T1:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     RSP, 48 ; Return stack
        ret     
    
fib: ; fib(num : long) : long
        sub     RSP, 16 ; Allocate stack
    
      ; num = param 0
        mov     R15, RCX ; Read parameter #0
      ; /
    
      ; _ = m1 Assign 0
        mov     R14, FALSE
      ; /
    
      ; _ = m2 Assign 1
        mov     R13, TRUE
      ; /
    
      ; _ = i Assign 0
        mov     R12, FALSE
      ; /
    
      ; Jump .T10:
        jmp     .T10
      ; /
    
      ; .T9:
    .T9:         
      ; /
    
      ; <>T12 = PreIncrement i
        inc     R12
        mov     R11, R12 ; Assign operand to target
      ; /
    
      ; .T10:
    .T10:         
      ; /
    
      ; <>T13 = i ComparisonLessThan num
        cmp     R12, R15 ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     R11, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     R11, TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If !<>T13 Jump .T11:
        test    R11, R11 ; Set condition codes according to condition
        jz      .T11 ; Jump if condition is false/zero
      ; /
    
      ; _ = temp Assign m1
        mov     R11, R14
      ; /
    
      ; _ = m1 Assign m2
        mov     R14, R13
      ; /
    
      ; <>T15 = temp Add m2
        mov     R12, R11 ; Assign LHS to target memory
        add     R12, R13
      ; /
    
      ; _ = m2 Assign <>T15
        mov     R13, R12
      ; /
    
      ; .T14:
    .T14:         
      ; /
    
      ; Jump .T9:
        jmp     .T9
      ; /
    
      ; .T11:
    .T11:         
      ; /
    
      ; return m1
        mov     RAX, R14 ; Return m1
        jmp     .__exit
      ; /
    
      ; .T8:
    .T8:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     RSP, 16 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     RSP, 48 ; Allocate stack
    
      ; num = param 0
        mov     R15, RCX ; Read parameter #0
      ; /
    
      ; <>T17 = num ComparisonLessThan 0
        cmp     R15, FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     R14, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     R14, TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If <>T17 Jump .T18:
        test    R14, R14 ; Set condition codes according to condition
        jnz     .T18 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T19:
        jmp     .T19
      ; /
    
      ; .T18:
    .T18:         
      ; /
    
      ; _ = call print(-, 1)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     RCX, ptrC2 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
      ; /
    
      ; <>T21 = Negation num
        mov     R14, R15 ; Assign operand to target
        neg     R14
      ; /
    
      ; _ = call printNum(<>T21)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     qword [rsp - 32], R14 ; Store live variable onto stack
        mov     RCX, R14 ; Pass parameter #0
        call    printNum
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
        mov     R14, qword [rsp - 32] ; Restore live variable from stack
      ; /
    
      ; return 
        jmp     .__exit
      ; /
    
      ; .T20:
    .T20:         
      ; /
    
      ; .T19:
    .T19:         
      ; /
    
      ; <>T22 = num Remainder 10
        xor     RDX, RDX ; Empty out higher bits of dividend
        mov     RAX, R15 ; Assign lhs to dividend
        mov     RBX, longC1 ; Move divisor into RBX, as a register is required for idiv
        idiv    RBX ; Assign remainder to RDX, quotient to RAX
        mov     R14, RDX ; Assign result to target memory
      ; /
    
      ; _ = digit Assign <>T22
        mov     R13, R14
      ; /
    
      ; <>T23 = num Divide 10
        xor     RDX, RDX ; Empty out higher bits of dividend
        mov     RAX, R15 ; Assign lhs to dividend
        mov     RBX, longC1 ; Move divisor into RBX, as a register is required for idiv
        idiv    RBX ; Assign remainder to RDX, quotient to RAX
        mov     R14, RAX ; Assign result to target memory
      ; /
    
      ; _ = rest Assign <>T23
        mov     R15, R14
      ; /
    
      ; <>T24 = rest ComparisonGreaterThan 0
        cmp     R15, FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     R14, FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     R14, TRUE ; Assign true to output
    .CG3:          ; Exit
      ; /
    
      ; If <>T24 Jump .T25:
        test    R14, R14 ; Set condition codes according to condition
        jnz     .T25 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T26:
        jmp     .T26
      ; /
    
      ; .T25:
    .T25:         
      ; /
    
      ; _ = call printNum(rest)
        mov     qword [rsp - 24], R13 ; Store live variable onto stack
        mov     qword [rsp - 32], R15 ; Store live variable onto stack
        mov     RCX, R15 ; Pass parameter #0
        call    printNum
        mov     R13, qword [rsp - 24] ; Restore live variable from stack
        mov     R15, qword [rsp - 32] ; Restore live variable from stack
      ; /
    
      ; .T26:
    .T26:         
      ; /
    
      ; _ = call printDigit(digit)
        mov     qword [rsp - 24], R13 ; Store live variable onto stack
        mov     RCX, R13 ; Pass parameter #0
        call    printDigit
        mov     R13, qword [rsp - 24] ; Restore live variable from stack
      ; /
    
      ; .T16:
    .T16:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     RSP, 48 ; Return stack
        ret     
    
printDigit: ; printDigit(digit : long) : long
        sub     RSP, 48 ; Allocate stack
    
      ; digit = param 0
        mov     R15, RCX ; Read parameter #0
      ; /
    
      ; _ = digits Assign 0123456789
        mov     R14, ptrC3
      ; /
    
      ; <>T29 = digit Multiply 2
        mov     R13, R15 ; Assign LHS to target memory
        imul    R13, longC4
      ; /
    
      ; <>T28 = digits Add <>T29
        mov     R15, R14 ; Assign LHS to target memory
        add     R15, R13
      ; /
    
      ; _ = call print(<>T28, 1)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     RCX, R15 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
      ; /
    
      ; .T27:
    .T27:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     RSP, 48 ; Return stack
        ret     
    
print: ; print(ptr : ptr, len : long) : long
        sub     RSP, 88 ; Allocate stack
    
      ; ptr = param 0
        mov     R15, RCX ; Read parameter #0
      ; /
    
      ; len = param 1
        mov     R14, RDX ; Read parameter #1
      ; /
    
      ; <>T32 = Negation 11
        mov     R13, longC5 ; Assign operand to target
        neg     R13
      ; /
    
      ; <>T31 = call GetStdHandle(<>T32)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     qword [rsp - 32], R14 ; Store live variable onto stack
        mov     qword [rsp - 40], R13 ; Store live variable onto stack
        mov     RCX, R13 ; Pass parameter #0
        call    GetStdHandle
        mov     R12, RAX ; Assign return value to <>T31
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
        mov     R14, qword [rsp - 32] ; Restore live variable from stack
        mov     R13, qword [rsp - 40] ; Restore live variable from stack
      ; /
    
      ; _ = stdOut Assign <>T31
        mov     R13, R12
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
        mov     R12, FALSE
      ; /
    
      ; <>T34 = Reference numberOfCharsWritten
        lea     R11, [R12] ; Create reference to numberOfCharsWritten
      ; /
    
      ; <>T33 = call WriteConsoleW(stdOut, ptr, len, <>T34, 0)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     qword [rsp - 32], R14 ; Store live variable onto stack
        mov     qword [rsp - 40], R13 ; Store live variable onto stack
        mov     qword [rsp - 48], R11 ; Store live variable onto stack
        mov     RCX, R13 ; Pass parameter #0
        mov     RDX, R15 ; Pass parameter #1
        mov     R8, R14 ; Pass parameter #2
        mov     R9, R11 ; Pass parameter #3
        mov     qword [rsp - 88], FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     R11, RAX ; Assign return value to <>T33
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
        mov     R14, qword [rsp - 32] ; Restore live variable from stack
        mov     R13, qword [rsp - 40] ; Restore live variable from stack
        mov     R11, qword [rsp - 48] ; Restore live variable from stack
      ; /
    
      ; return <>T33
        mov     RAX, R11 ; Return <>T33
        jmp     .__exit
      ; /
    
      ; .T30:
    .T30:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     RSP, 88 ; Return stack
        ret     
    
