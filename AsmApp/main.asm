extern GetStdHandle : proc
extern WriteFile : proc

.data
   hello_msg db "Hello World!", 0dh, 0ah
   bytes_written dw 0
   eval_stack byte 256 dup (?)
   ;eval_index qword 0

.code
_ldc_i4 PROC
    mov  qword ptr [rcx + rdi], rdx
    add  rdi, 8
    ret
_ldc_i4 ENDP

_add PROC
    sub  rdi, 8
    mov  rax, qword ptr [rcx + rdi]
    sub  rdi, 8
    add  rax, qword ptr [rcx + rdi]
    ret
_add ENDP

main PROC
    ;; Prolog & epilog are only necessary when callin Win API
    ;push rbp
    ;mov  rbp, rsp
    ;sub  rsp, 32

    ;; Calling Win API to getn stdout handle and print something
    ;mov  rcx, -11                 ; STD_OUTPUT_HANDLE
    ;call GetStdHandle
    ;mov  rcx, rax

    ;lea  rdx, [hello_msg]
    ;mov  r8, 14                   ; hello_msg length
    ;lea  r9, [bytes_written]
    ;mov  qword ptr [rbp + 32], 0
    ;call WriteFile

    xor  rdi, rdi
    lea  rcx, [eval_stack]

    mov  rdx, 1
    call _ldc_i4 
    mov  rdx, 2
    call _ldc_i4
    call _add

    ;; Prolog & epilog are only necessary when callin Win API
    ;mov  rsp, rbp
    ;pop  rbp
    ret 
main ENDP 
END