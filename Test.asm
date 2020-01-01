section .data
FALSE: equ     0
TRUE: equ     1
stringC0: db      __utf16__(`0123456789`)
stringC1: db      __utf16__(`\n`)
longC2: equ     2
longC3: equ     42
stringC4: db      __utf16__(`-`)
stringC5: db      __utf16__(`0`)
longC6: equ     10
longC7: equ     11
section .text
    global  _start
    extern  GetStdHandle
    extern  WriteConsoleW
_start: ; _start() : long
        sub     RSP, 48 ; Allocate stack
      ; _ = digits Assign 0123456789
        mov     R15, stringC0
      ; /
    
      ; <>T1 = digits Add 0
        mov     R14, R15 ; Assign LHS to target memory
        add     R14, FALSE
      ; /
    
      ; _ = call print(<>T1, 1)
        mov     qword [rsp - 24], R14 ; Store live variable onto stack
        mov     RCX, R14 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
        mov     R14, qword [rsp - 24] ; Restore live variable from stack
      ; /
    
      ; _ = call print(\n, 2)
        mov     RCX, stringC1 ; Pass parameter #0
        mov     RDX, longC2 ; Pass parameter #1
        call    print
      ; /
    
      ; _ = call printNum(42)
        mov     RCX, longC3 ; Pass parameter #0
        call    printNum
      ; /
    
    .__exit:          ; Function exit/return label
        add     RSP, 48 ; Return stack
        ret     
printNum: ; printNum(num : long) : long
        sub     RSP, 64 ; Allocate stack
      ; num = param 0
        mov     R15, RCX ; Read parameter #0
      ; /
    
      ; <>T2 = num ComparisonLessThan 0
        mov     R14, R15 ; Assign LHS to target memory
        cmp     R14, FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     R14, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     R14, TRUE ; Assign true to output
    .CG1:          ; Exit
      ; /
    
      ; If <>T2 Jump .T3:
        test    R14, R14 ; Set condition codes according to condition
        jnz     .T3 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T4:
        jmp     .T4
      ; /
    
      ; .T3:
    .T3:         
      ; /
    
      ; _ = call print(-, 1)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     RCX, stringC4 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
      ; /
    
      ; <>T6 = Negation 1
        mov     R14, TRUE ; Assign operand to target
        neg     R14
      ; /
    
      ; <>T5 = num Multiply <>T6
        mov     R13, R15 ; Assign LHS to target memory
        imul    R13, R14
      ; /
    
      ; _ = num Assign <>T5
        mov     R15, R13
      ; /
    
      ; .T4:
    .T4:         
      ; /
    
      ; <>T7 = num ComparisonEqual 0
        mov     R13, R15 ; Assign LHS to target memory
        cmp     R13, FALSE ; Set condition codes according to operands
        je      .CG2 ; Jump to True if the comparison is true
        mov     R13, FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     R13, TRUE ; Assign true to output
    .CG3:          ; Exit
      ; /
    
      ; If <>T7 Jump .T8:
        test    R13, R13 ; Set condition codes according to condition
        jnz     .T8 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T9:
        jmp     .T9
      ; /
    
      ; .T8:
    .T8:         
      ; /
    
      ; _ = call print(0, 1)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     RCX, stringC5 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
      ; /
    
      ; return 0
        mov     RAX, FALSE ; Return 0
        jmp     .__exit
      ; /
    
      ; .T9:
    .T9:         
      ; /
    
      ; _ = digits Assign 0123456789
        mov     R13, stringC0
      ; /
    
      ; .T11:
    .T11:         
      ; /
    
      ; <>T10 = num ComparisonGreaterThan 0
        mov     R14, R15 ; Assign LHS to target memory
        cmp     R14, FALSE ; Set condition codes according to operands
        jg      .CG4 ; Jump to True if the comparison is true
        mov     R14, FALSE ; Assign false to output
        jmp     .CG5 ; Jump to Exit
    .CG4:          ; True
        mov     R14, TRUE ; Assign true to output
    .CG5:          ; Exit
      ; /
    
      ; If !<>T10 Jump .T12:
        test    R14, R14 ; Set condition codes according to condition
        jz      .T12 ; Jump if condition is false/zero
      ; /
    
      ; <>T13 = num Remainder 10
        mov     R14, R15 ; Assign LHS to target memory
        xor     RDX, RDX ; Empty out higher bits of dividend
        mov     RAX, R15 ; Assign lhs to dividend
        mov     RBX, longC6 ; Move divisor into RBX, as a register is required for idiv
        idiv    RBX ; Assign remainder to RDX, quotient to RAX
        mov     R14, RDX ; Assign result to target memory
      ; /
    
      ; _ = remainder Assign <>T13
        mov     R12, R14
      ; /
    
      ; <>T14 = num Divide 10
        mov     R14, R15 ; Assign LHS to target memory
        xor     RDX, RDX ; Empty out higher bits of dividend
        mov     RAX, R15 ; Assign lhs to dividend
        mov     RBX, longC6 ; Move divisor into RBX, as a register is required for idiv
        idiv    RBX ; Assign remainder to RDX, quotient to RAX
        mov     R14, RAX ; Assign result to target memory
      ; /
    
      ; _ = num Assign <>T14
        mov     R15, R14
      ; /
    
      ; _ = call printNum(num)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     qword [rsp - 32], R13 ; Store live variable onto stack
        mov     qword [rsp - 40], R12 ; Store live variable onto stack
        mov     RCX, R15 ; Pass parameter #0
        call    printNum
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
        mov     R13, qword [rsp - 32] ; Restore live variable from stack
        mov     R12, qword [rsp - 40] ; Restore live variable from stack
      ; /
    
      ; <>T16 = remainder Multiply 2
        mov     R14, R12 ; Assign LHS to target memory
        imul    R14, longC2
      ; /
    
      ; <>T15 = digits Add <>T16
        mov     R12, R13 ; Assign LHS to target memory
        add     R12, R14
      ; /
    
      ; _ = call print(<>T15, 1)
        mov     qword [rsp - 24], R12 ; Store live variable onto stack
        mov     RCX, R12 ; Pass parameter #0
        mov     RDX, TRUE ; Pass parameter #1
        call    print
        mov     R12, qword [rsp - 24] ; Restore live variable from stack
      ; /
    
      ; Jump .T11:
        jmp     .T11
      ; /
    
      ; .T12:
    .T12:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     RSP, 64 ; Return stack
        ret     
print: ; print(ptr : long, len : long) : long
        sub     RSP, 88 ; Allocate stack
      ; ptr = param 0
        mov     R15, RCX ; Read parameter #0
      ; /
    
      ; len = param 1
        mov     R14, RDX ; Read parameter #1
      ; /
    
      ; <>T18 = Negation 11
        mov     R13, longC7 ; Assign operand to target
        neg     R13
      ; /
    
      ; <>T17 = call GetStdHandle(<>T18)
        mov     qword [rsp - 24], R15 ; Store live variable onto stack
        mov     qword [rsp - 32], R14 ; Store live variable onto stack
        mov     qword [rsp - 40], R13 ; Store live variable onto stack
        mov     RCX, R13 ; Pass parameter #0
        call    GetStdHandle
        mov     R12, RAX ; Assign return value to <>T17
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
        mov     R14, qword [rsp - 32] ; Restore live variable from stack
        mov     R13, qword [rsp - 40] ; Restore live variable from stack
      ; /
    
      ; _ = stdOut Assign <>T17
        mov     R13, R12
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
        mov     R12, FALSE
      ; /
    
      ; <>T20 = Reference numberOfCharsWritten
        lea     R11, [R12] ; Create reference to numberOfCharsWritten
      ; /
    
      ; <>T19 = call WriteConsoleW(stdOut, ptr, len, <>T20, 0)
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
        mov     R11, RAX ; Assign return value to <>T19
        mov     R15, qword [rsp - 24] ; Restore live variable from stack
        mov     R14, qword [rsp - 32] ; Restore live variable from stack
        mov     R13, qword [rsp - 40] ; Restore live variable from stack
        mov     R11, qword [rsp - 48] ; Restore live variable from stack
      ; /
    
      ; return <>T19
        mov     RAX, R11 ; Return <>T19
        jmp     .__exit
      ; /
    
    .__exit:          ; Function exit/return label
        add     RSP, 88 ; Return stack
        ret     
