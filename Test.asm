section .data
FALSE: equ     0
TRUE: equ     1
ptrC0: db      __utf16__(`Start\n`)
longC1: equ     6
longC2: equ     8
longC3: equ     10
ptrC4: db      __utf16__(`-`)
ptrC5: db      __utf16__(`0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ`)
longC6: equ     2
longC7: equ     11
section .text
    global  _start
    extern  GetStdHandle
    extern  WriteConsoleW
_start: ; _start() : long
        sub     rsp, 72 ; Allocate stack
    
      ; _ = call print(Start\n, 6)
        mov     rcx, ptrC0 ; Pass parameter #0
        mov     rdx, longC1 ; Pass parameter #1
        call    print
      ; /
    
      ; <>T2 = call fac(8)
        mov     rcx, longC2 ; Pass parameter #0
        call    fac
        mov     qword [rsp + 16], rax ; Assign return value to <>T2
      ; /
    
      ; _ = call printNum(<>T2)
        mov     rcx, qword [rsp + 16] ; Pass parameter #0
        call    printNum
      ; /
    
      ; .T1:
    .T1:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 72 ; Return stack
        ret     
    
yeet: ; yeet() : long
        sub     rsp, 24 ; Allocate stack
    
      ; .T3:
    .T3:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 24 ; Return stack
        ret     
    
pow: ; pow(base : long, exp : long) : long
        sub     rsp, 72 ; Allocate stack
    
      ; base = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; exp = param 1
        mov     qword [rsp + 24], rdx ; Read parameter #1
      ; /
    
      ; _ = acc Assign 1
        mov     qword [rsp + 32], TRUE
      ; /
    
      ; Jump .T6:
        jmp     .T6
      ; /
    
      ; .T5:
    .T5:         
      ; /
    
      ; <>T8 = PreDecrement exp
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        dec     rbx
        mov     qword [rsp + 24], rbx ; Assign temporary operand to actual operand
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; .T6:
    .T6:         
      ; /
    
      ; <>T9 = exp ComparisonGreaterThan 0
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        jg      .CG0 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG1:          ; Exit
        mov     qword [rsp + 48], rbx ; Assign result to actual target memory
      ; /
    
      ; If !<>T9 Jump .T7:
        mov     rbx, qword [rsp + 48] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jz      .T7 ; Jump if condition is false/zero
      ; /
    
      ; <>T11 = acc Multiply base
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        imul    rbx, qword [rsp + 16]
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = acc Assign <>T11
        mov     rbx, qword [rsp + 56] ; Move RHS into register so operation is possible
        mov     qword [rsp + 32], rbx
      ; /
    
      ; .T10:
    .T10:         
      ; /
    
      ; Jump .T5:
        jmp     .T5
      ; /
    
      ; .T7:
    .T7:         
      ; /
    
      ; return acc
        mov     rax, qword [rsp + 32] ; Return acc
        jmp     .__exit
      ; /
    
      ; .T4:
    .T4:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 72 ; Return stack
        ret     
    
fac: ; fac(num : long) : long
        sub     rsp, 72 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; _ = acc Assign 1
        mov     qword [rsp + 24], TRUE
      ; /
    
      ; _ = i Assign 1
        mov     qword [rsp + 32], TRUE
      ; /
    
      ; Jump .T14:
        jmp     .T14
      ; /
    
      ; .T13:
    .T13:         
      ; /
    
      ; <>T16 = PreIncrement i
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        inc     rbx
        mov     qword [rsp + 32], rbx ; Assign temporary operand to actual operand
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; .T14:
    .T14:         
      ; /
    
      ; <>T17 = i ComparisonLessThanOrEqual num
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        cmp     rbx, qword [rsp + 16] ; Set condition codes according to operands
        jle     .CG0 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG1:          ; Exit
        mov     qword [rsp + 48], rbx ; Assign result to actual target memory
      ; /
    
      ; If !<>T17 Jump .T15:
        mov     rbx, qword [rsp + 48] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jz      .T15 ; Jump if condition is false/zero
      ; /
    
      ; <>T19 = acc Multiply i
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        imul    rbx, qword [rsp + 32]
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = acc Assign <>T19
        mov     rbx, qword [rsp + 56] ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; .T18:
    .T18:         
      ; /
    
      ; Jump .T13:
        jmp     .T13
      ; /
    
      ; .T15:
    .T15:         
      ; /
    
      ; return acc
        mov     rax, qword [rsp + 24] ; Return acc
        jmp     .__exit
      ; /
    
      ; .T12:
    .T12:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 72 ; Return stack
        ret     
    
fib: ; fib(num : long) : long
        sub     rsp, 104 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; <>T21 = num ComparisonEqual 0
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        je      .CG0 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG1:          ; Exit
        mov     qword [rsp + 24], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T21 Jump .T22:
        mov     rbx, qword [rsp + 24] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T22 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T23:
        jmp     .T23
      ; /
    
      ; .T22:
    .T22:         
      ; /
    
      ; return 0
        mov     rax, FALSE ; Return 0
        jmp     .__exit
      ; /
    
      ; .T23:
    .T23:         
      ; /
    
      ; _ = m1 Assign 0
        mov     qword [rsp + 32], FALSE
      ; /
    
      ; _ = m2 Assign 1
        mov     qword [rsp + 40], TRUE
      ; /
    
      ; _ = i Assign 0
        mov     qword [rsp + 48], FALSE
      ; /
    
      ; Jump .T25:
        jmp     .T25
      ; /
    
      ; .T24:
    .T24:         
      ; /
    
      ; <>T27 = PreIncrement i
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        inc     rbx
        mov     qword [rsp + 48], rbx ; Assign temporary operand to actual operand
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; .T25:
    .T25:         
      ; /
    
      ; <>T29 = num Subtract 1
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        sub     rbx, TRUE
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T28 = i ComparisonLessThan <>T29
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        cmp     rbx, qword [rsp + 64] ; Set condition codes according to operands
        jl      .CG2 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG3:          ; Exit
        mov     qword [rsp + 72], rbx ; Assign result to actual target memory
      ; /
    
      ; If !<>T28 Jump .T26:
        mov     rbx, qword [rsp + 72] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jz      .T26 ; Jump if condition is false/zero
      ; /
    
      ; _ = temp Assign m1
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        mov     qword [rsp + 80], rbx
      ; /
    
      ; _ = m1 Assign m2
        mov     rbx, qword [rsp + 40] ; Move RHS into register so operation is possible
        mov     qword [rsp + 32], rbx
      ; /
    
      ; <>T31 = temp Add m2
        mov     rbx, qword [rsp + 80] ; Move RHS into register so operation is possible
        add     rbx, qword [rsp + 40]
        mov     qword [rsp + 88], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = m2 Assign <>T31
        mov     rbx, qword [rsp + 88] ; Move RHS into register so operation is possible
        mov     qword [rsp + 40], rbx
      ; /
    
      ; .T30:
    .T30:         
      ; /
    
      ; Jump .T24:
        jmp     .T24
      ; /
    
      ; .T26:
    .T26:         
      ; /
    
      ; return m2
        mov     rax, qword [rsp + 40] ; Return m2
        jmp     .__exit
      ; /
    
      ; .T20:
    .T20:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 104 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     rsp, 72 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; _ = call printNumAny(num, 10)
        mov     rcx, qword [rsp + 16] ; Pass parameter #0
        mov     rdx, longC3 ; Pass parameter #1
        call    printNumAny
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 72 ; Return stack
        ret     
    
printNumAny: ; printNumAny(num : long, base : long) : long
        sub     rsp, 152 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; base = param 1
        mov     qword [rsp + 24], rdx ; Read parameter #1
      ; /
    
      ; <>T33 = num ComparisonLessThan 0
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        jl      .CG0 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG1:          ; Exit
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T33 Jump .T34:
        mov     rbx, qword [rsp + 32] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T34 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T35:
        jmp     .T35
      ; /
    
      ; .T34:
    .T34:         
      ; /
    
      ; _ = call print(-, 1)
        mov     rcx, ptrC4 ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; <>T37 = Negation num
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call printNum(<>T37)
        mov     rcx, qword [rsp + 40] ; Pass parameter #0
        call    printNum
      ; /
    
      ; return 
        jmp     .__exit
      ; /
    
      ; .T36:
    .T36:         
      ; /
    
      ; .T35:
    .T35:         
      ; /
    
      ; <>T38 = num Remainder base
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp + 16] ; Assign LHS to dividend
        mov     rbx, qword [rsp + 24] ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rdx ; Assign result to target memory
        mov     qword [rsp + 48], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = digit Assign <>T38
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        mov     qword [rsp + 56], rbx
      ; /
    
      ; <>T39 = num Divide base
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp + 16] ; Assign LHS to dividend
        mov     rbx, qword [rsp + 24] ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rax ; Assign result to target memory
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = rest Assign <>T39
        mov     rbx, qword [rsp + 64] ; Move RHS into register so operation is possible
        mov     qword [rsp + 72], rbx
      ; /
    
      ; <>T40 = rest ComparisonGreaterThan 0
        mov     rbx, qword [rsp + 72] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        jg      .CG2 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG3:          ; Exit
        mov     qword [rsp + 80], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T40 Jump .T41:
        mov     rbx, qword [rsp + 80] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T41 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T42:
        jmp     .T42
      ; /
    
      ; .T41:
    .T41:         
      ; /
    
      ; _ = call printNumAny(rest, base)
        mov     rcx, qword [rsp + 72] ; Pass parameter #0
        mov     rdx, qword [rsp + 24] ; Pass parameter #1
        call    printNumAny
      ; /
    
      ; .T42:
    .T42:         
      ; /
    
      ; _ = call printDigit(digit)
        mov     rcx, qword [rsp + 56] ; Pass parameter #0
        call    printDigit
      ; /
    
      ; .T32:
    .T32:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 152 ; Return stack
        ret     
    
printDigit: ; printDigit(digit : long) : long
        sub     rsp, 88 ; Allocate stack
    
      ; digit = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; _ = digits Assign 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
        mov     rbx, ptrC5 ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; <>T45 = digit Multiply 2
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        imul    rbx, longC6
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T44 = digits Add <>T45
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        add     rbx, qword [rsp + 32]
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call print(<>T44, 1)
        mov     rcx, qword [rsp + 40] ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; .T43:
    .T43:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 88 ; Return stack
        ret     
    
print: ; print(ptr : ptr, len : long) : long
        sub     rsp, 160 ; Allocate stack
    
      ; ptr = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; len = param 1
        mov     qword [rsp + 24], rdx ; Read parameter #1
      ; /
    
      ; <>T48 = Negation 11
        mov     rbx, longC7 ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T47 = call GetStdHandle(<>T48)
        mov     rcx, qword [rsp + 32] ; Pass parameter #0
        call    GetStdHandle
        mov     qword [rsp + 48], rax ; Assign return value to <>T47
      ; /
    
      ; _ = stdOut Assign <>T47
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        mov     qword [rsp + 40], rbx
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
        mov     qword [rsp + 56], FALSE
      ; /
    
      ; <>T50 = Reference numberOfCharsWritten
        mov     qword [rsp + 64], rsp ; Assign stackpointer
        add     qword [rsp + 64], 56 ; and subtract to get correct position
      ; /
    
      ; <>T49 = call WriteConsoleW(stdOut, ptr, len, <>T50, 0)
        mov     rcx, qword [rsp + 40] ; Pass parameter #0
        mov     rdx, qword [rsp + 16] ; Pass parameter #1
        mov     r8, qword [rsp + 24] ; Pass parameter #2
        mov     r9, qword [rsp + 64] ; Pass parameter #3
        mov     qword [rsp + 152], FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     qword [rsp + 72], rax ; Assign return value to <>T49
      ; /
    
      ; return <>T49
        mov     rax, qword [rsp + 72] ; Return <>T49
        jmp     .__exit
      ; /
    
      ; .T46:
    .T46:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
    .__exit:          ; Function exit/return label
        add     rsp, 160 ; Return stack
        ret     
    
