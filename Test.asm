section .data
FALSE: equ     0
TRUE: equ     1
longC0: equ     2
longC1: equ     10
ptrC2: db      __utf16__(`-`)
ptrC3: db      __utf16__(`0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ`)
longC4: equ     11
section .text
    global  _start
    extern  GetStdHandle
    extern  WriteConsoleW
_start: ; _start() : long
        sub     rsp, 24 ; Allocate stack
    
      ; .T1:
    .T1:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /   
    
        add     rsp, 24 ; Return stack
        ret     
    
fib: ; fib(inp : long) : long
        sub     rsp, 128 ; Allocate stack
    
      ; inp = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; <>T4 = inp ComparisonEqual 0
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
    
      ; <>T5 = inp ComparisonEqual 1
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        cmp     rbx, TRUE ; Set condition codes according to operands
        je      .CG2 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG3 ; Jump to Exit
    .CG2:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG3:          ; Exit
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T3 = <>T4 LogicalOr <>T5
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        or      rbx, qword [rsp + 32]
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T3 Jump .T6:
        mov     rbx, qword [rsp + 40] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T6 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T7:
        jmp     .T7
      ; /
    
      ; .T6:
    .T6:         
      ; /
    
      ; return inp
        mov     rax, qword [rsp + 16] ; Return inp
        jmp     .__exit
      ; /
    
      ; .T7:
    .T7:         
      ; /
    
      ; <>T10 = inp Subtract 1
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        sub     rbx, TRUE
        mov     qword [rsp + 48], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T9 = call fib(<>T10)
        mov     rcx, qword [rsp + 48] ; Pass parameter #0
        call    fib
        mov     qword [rsp + 72], rax ; Assign return value to <>T9
      ; /
    
      ; <>T12 = inp Subtract 2
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        sub     rbx, longC0
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T11 = call fib(<>T12)
        mov     rcx, qword [rsp + 56] ; Pass parameter #0
        call    fib
        mov     qword [rsp + 80], rax ; Assign return value to <>T11
      ; /
    
      ; <>T8 = <>T9 Add <>T11
        mov     rbx, qword [rsp + 72] ; Move RHS into register so operation is possible
        add     rbx, qword [rsp + 80]
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; return <>T8
        mov     rax, qword [rsp + 64] ; Return <>T8
        jmp     .__exit
      ; /
    
      ; .T2:
    .T2:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
        add     rsp, 128 ; Return stack
        ret     
    
roundDownTo10: ; roundDownTo10(inp : long) : long
        sub     rsp, 88 ; Allocate stack
    
      ; inp = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; <>T14 = inp ComparisonGreaterThanOrEqual 0
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        cmp     rbx, FALSE ; Set condition codes according to operands
        jge     .CG0 ; Jump to True if the comparison is true
        mov     rbx, FALSE ; Assign false to output
        jmp     .CG1 ; Jump to Exit
    .CG0:          ; True
        mov     rbx, TRUE ; Assign true to output
    .CG1:          ; Exit
        mov     qword [rsp + 24], rbx ; Assign result to actual target memory
      ; /
    
      ; If <>T14 Jump .T15:
        mov     rbx, qword [rsp + 24] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T15 ; Jump if condition is true/non-zero
      ; /
    
      ; <>T19 = Negation inp
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T18 = <>T19 Remainder 10
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp + 32] ; Assign LHS to dividend
        mov     rbx, longC1 ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rdx ; Assign result to target memory
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T17 = inp Add <>T18
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        add     rbx, qword [rsp + 40]
        mov     qword [rsp + 48], rbx ; Assign result to actual target memory
      ; /
    
      ; return <>T17
        mov     rax, qword [rsp + 48] ; Return <>T17
        jmp     .__exit
      ; /
    
      ; Jump .T16:
        jmp     .T16
      ; /
    
      ; .T15:
    .T15:         
      ; /
    
      ; <>T21 = inp Remainder 10
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp + 16] ; Assign LHS to dividend
        mov     rbx, longC1 ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rdx ; Assign result to target memory
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T20 = inp Subtract <>T21
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        sub     rbx, qword [rsp + 56]
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; return <>T20
        mov     rax, qword [rsp + 64] ; Return <>T20
        jmp     .__exit
      ; /
    
      ; .T16:
    .T16:         
      ; /
    
      ; .T13:
    .T13:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
        add     rsp, 88 ; Return stack
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
    
      ; Jump .T24:
        jmp     .T24
      ; /
    
      ; .T23:
    .T23:         
      ; /
    
      ; <>T26 = PreDecrement exp
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        dec     rbx
        mov     qword [rsp + 24], rbx ; Assign temporary operand to actual operand
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; .T24:
    .T24:         
      ; /
    
      ; <>T27 = exp ComparisonGreaterThan 0
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
    
      ; If !<>T27 Jump .T25:
        mov     rbx, qword [rsp + 48] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jz      .T25 ; Jump if condition is false/zero
      ; /
    
      ; <>T29 = acc Multiply base
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        imul    rbx, qword [rsp + 16]
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = acc Assign <>T29
        mov     rbx, qword [rsp + 56] ; Move RHS into register so operation is possible
        mov     qword [rsp + 32], rbx
      ; /
    
      ; .T28:
    .T28:         
      ; /
    
      ; Jump .T23:
        jmp     .T23
      ; /
    
      ; .T25:
    .T25:         
      ; /
    
      ; return acc
        mov     rax, qword [rsp + 32] ; Return acc
        jmp     .__exit
      ; /
    
      ; .T22:
    .T22:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
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
    
      ; Jump .T32:
        jmp     .T32
      ; /
    
      ; .T31:
    .T31:         
      ; /
    
      ; <>T34 = PreIncrement i
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        inc     rbx
        mov     qword [rsp + 32], rbx ; Assign temporary operand to actual operand
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; .T32:
    .T32:         
      ; /
    
      ; <>T35 = i ComparisonLessThanOrEqual num
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
    
      ; If !<>T35 Jump .T33:
        mov     rbx, qword [rsp + 48] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jz      .T33 ; Jump if condition is false/zero
      ; /
    
      ; <>T37 = acc Multiply i
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        imul    rbx, qword [rsp + 32]
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = acc Assign <>T37
        mov     rbx, qword [rsp + 56] ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; .T36:
    .T36:         
      ; /
    
      ; Jump .T31:
        jmp     .T31
      ; /
    
      ; .T33:
    .T33:         
      ; /
    
      ; return acc
        mov     rax, qword [rsp + 24] ; Return acc
        jmp     .__exit
      ; /
    
      ; .T30:
    .T30:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
        add     rsp, 72 ; Return stack
        ret     
    
fibFast: ; fibFast(num : long) : long
        sub     rsp, 104 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; <>T39 = num ComparisonEqual 0
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
    
      ; If <>T39 Jump .T40:
        mov     rbx, qword [rsp + 24] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T40 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T41:
        jmp     .T41
      ; /
    
      ; .T40:
    .T40:         
      ; /
    
      ; return 0
        mov     rax, FALSE ; Return 0
        jmp     .__exit
      ; /
    
      ; .T41:
    .T41:         
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
    
      ; Jump .T43:
        jmp     .T43
      ; /
    
      ; .T42:
    .T42:         
      ; /
    
      ; <>T45 = PreIncrement i
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        inc     rbx
        mov     qword [rsp + 48], rbx ; Assign temporary operand to actual operand
        mov     qword [rsp + 56], rbx ; Assign result to actual target memory
      ; /
    
      ; .T43:
    .T43:         
      ; /
    
      ; <>T47 = num Subtract 1
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        sub     rbx, TRUE
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T46 = i ComparisonLessThan <>T47
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
    
      ; If !<>T46 Jump .T44:
        mov     rbx, qword [rsp + 72] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jz      .T44 ; Jump if condition is false/zero
      ; /
    
      ; _ = temp Assign m1
        mov     rbx, qword [rsp + 32] ; Move RHS into register so operation is possible
        mov     qword [rsp + 80], rbx
      ; /
    
      ; _ = m1 Assign m2
        mov     rbx, qword [rsp + 40] ; Move RHS into register so operation is possible
        mov     qword [rsp + 32], rbx
      ; /
    
      ; <>T49 = temp Add m2
        mov     rbx, qword [rsp + 80] ; Move RHS into register so operation is possible
        add     rbx, qword [rsp + 40]
        mov     qword [rsp + 88], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = m2 Assign <>T49
        mov     rbx, qword [rsp + 88] ; Move RHS into register so operation is possible
        mov     qword [rsp + 40], rbx
      ; /
    
      ; .T48:
    .T48:         
      ; /
    
      ; Jump .T42:
        jmp     .T42
      ; /
    
      ; .T44:
    .T44:         
      ; /
    
      ; return m2
        mov     rax, qword [rsp + 40] ; Return m2
        jmp     .__exit
      ; /
    
      ; .T38:
    .T38:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
        add     rsp, 104 ; Return stack
        ret     
    
printNum: ; printNum(num : long) : long
        sub     rsp, 72 ; Allocate stack
    
      ; num = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; _ = call printNumAny(num, 10)
        mov     rcx, qword [rsp + 16] ; Pass parameter #0
        mov     rdx, longC1 ; Pass parameter #1
        call    printNumAny
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
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
    
      ; <>T51 = num ComparisonLessThan 0
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
    
      ; If <>T51 Jump .T52:
        mov     rbx, qword [rsp + 32] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T52 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T53:
        jmp     .T53
      ; /
    
      ; .T52:
    .T52:         
      ; /
    
      ; _ = call print(-, 1)
        mov     rcx, ptrC2 ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; <>T55 = Negation num
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call printNum(<>T55)
        mov     rcx, qword [rsp + 40] ; Pass parameter #0
        call    printNum
      ; /
    
      ; return 
        jmp     .__exit
      ; /
    
      ; .T54:
    .T54:         
      ; /
    
      ; .T53:
    .T53:         
      ; /
    
      ; <>T56 = num Remainder base
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp + 16] ; Assign LHS to dividend
        mov     rbx, qword [rsp + 24] ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rdx ; Assign result to target memory
        mov     qword [rsp + 48], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = digit Assign <>T56
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        mov     qword [rsp + 56], rbx
      ; /
    
      ; <>T57 = num Divide base
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        xor     rdx, rdx ; Empty out higher bits of dividend
        mov     rax, qword [rsp + 16] ; Assign LHS to dividend
        mov     rbx, qword [rsp + 24] ; Move divisor into RBX, as a register is required for idiv
        idiv    rbx ; Assign remainder to RDX, quotient to RAX
        mov     rbx, rax ; Assign result to target memory
        mov     qword [rsp + 64], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = rest Assign <>T57
        mov     rbx, qword [rsp + 64] ; Move RHS into register so operation is possible
        mov     qword [rsp + 72], rbx
      ; /
    
      ; <>T58 = rest ComparisonGreaterThan 0
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
    
      ; If <>T58 Jump .T59:
        mov     rbx, qword [rsp + 80] ; Move condition into register so operation is possible
        test    rbx, rbx ; Set condition codes according to condition
        jnz     .T59 ; Jump if condition is true/non-zero
      ; /
    
      ; Jump .T60:
        jmp     .T60
      ; /
    
      ; .T59:
    .T59:         
      ; /
    
      ; _ = call printNumAny(rest, base)
        mov     rcx, qword [rsp + 72] ; Pass parameter #0
        mov     rdx, qword [rsp + 24] ; Pass parameter #1
        call    printNumAny
      ; /
    
      ; .T60:
    .T60:         
      ; /
    
      ; _ = call printDigit(digit)
        mov     rcx, qword [rsp + 56] ; Pass parameter #0
        call    printDigit
      ; /
    
      ; .T50:
    .T50:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
        add     rsp, 152 ; Return stack
        ret     
    
printDigit: ; printDigit(digit : long) : long
        sub     rsp, 88 ; Allocate stack
    
      ; digit = param 0
        mov     qword [rsp + 16], rcx ; Read parameter #0
      ; /
    
      ; _ = digits Assign 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ
        mov     rbx, ptrC3 ; Move RHS into register so operation is possible
        mov     qword [rsp + 24], rbx
      ; /
    
      ; <>T63 = digit Multiply 2
        mov     rbx, qword [rsp + 16] ; Move RHS into register so operation is possible
        imul    rbx, longC0
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T62 = digits Add <>T63
        mov     rbx, qword [rsp + 24] ; Move RHS into register so operation is possible
        add     rbx, qword [rsp + 32]
        mov     qword [rsp + 40], rbx ; Assign result to actual target memory
      ; /
    
      ; _ = call print(<>T62, 1)
        mov     rcx, qword [rsp + 40] ; Pass parameter #0
        mov     rdx, TRUE ; Pass parameter #1
        call    print
      ; /
    
      ; .T61:
    .T61:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
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
    
      ; <>T66 = Negation 11
        mov     rbx, longC4 ; Move RHS into register so operation is possible
        neg     rbx
        mov     qword [rsp + 32], rbx ; Assign result to actual target memory
      ; /
    
      ; <>T65 = call GetStdHandle(<>T66)
        mov     rcx, qword [rsp + 32] ; Pass parameter #0
        call    GetStdHandle
        mov     qword [rsp + 48], rax ; Assign return value to <>T65
      ; /
    
      ; _ = stdOut Assign <>T65
        mov     rbx, qword [rsp + 48] ; Move RHS into register so operation is possible
        mov     qword [rsp + 40], rbx
      ; /
    
      ; _ = numberOfCharsWritten Assign 0
        mov     qword [rsp + 56], FALSE
      ; /
    
      ; <>T68 = Reference numberOfCharsWritten
        mov     qword [rsp + 64], rsp ; Assign stackpointer
        add     qword [rsp + 64], 56 ; and subtract to get correct position
      ; /
    
      ; <>T67 = call WriteConsoleW(stdOut, ptr, len, <>T68, 0)
        mov     rcx, qword [rsp + 40] ; Pass parameter #0
        mov     rdx, qword [rsp + 16] ; Pass parameter #1
        mov     r8, qword [rsp + 24] ; Pass parameter #2
        mov     r9, qword [rsp + 64] ; Pass parameter #3
        mov     qword [rsp + 152], FALSE ; Pass parameter #4
        call    WriteConsoleW
        mov     qword [rsp + 72], rax ; Assign return value to <>T67
      ; /
    
      ; return <>T67
        mov     rax, qword [rsp + 72] ; Return <>T67
        jmp     .__exit
      ; /
    
      ; .T64:
    .T64:         
      ; /
    
      ; .__exit:
    .__exit:         
      ; /
    
        add     rsp, 160 ; Return stack
        ret     
    
