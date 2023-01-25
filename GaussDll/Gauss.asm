.data
kernel: 
word 1
word 1
word 1
word 2
word 2
word 2
word 1
word 1
word 1
word 2
word 2
word 2
word 4
word 4
word 4
word 2
word 2
word 2
word 1
word 1
word 1
word 2
word 2
word 2
word 1
word 1
word 1

.code

;params: bmap data - rcx, bmap size - rdx, bmap width - r8, start index - r9, end index - stack/r10
gaussianBlur proc EXPORT

;get weights
movdqu xmm0, oword ptr[kernel]
movdqu xmm1, oword ptr[kernel + 6]
movdqu xmm2, oword ptr[kernel + 12]
movdqu xmm3, oword ptr[kernel + 18]
movdqu xmm4, oword ptr[kernel + 24]
movdqu xmm5, oword ptr[kernel + 30]

;registers:
;r11 
;rdi szerokosc bmp * 4
;r12 end index  -to revise

xor r10, r10
mov ebx, dword ptr[rbp+48] 
mov r10, rbx ;end index
mov r15, rcx ;pointer to bmap
mov rax, r8	 ;width
imul rax, 4	 ;RGBA parts
mov rdi, rax ;rdi width multiplied by 4
imul rax, r9 ;start index*width - first row
mov rcx, rax ;rcx - counter
mov rax, rdi 
imul rax, r10 ;end index*width - last row
mov r12, rax ;r12 - end
sub r10, r9
shr r10, 2 

;check if top left out of bounds
loop1:
add rcx, 4 
mov rax, rcx
sub rax, rdi
sub rax, 4 
cmp rax, 0
jl loop1

;check if bottom right out of bounds
mov rax, rcx
add rax, rdi
add rax, 4 
cmp rax, rdx 
jae endBlur 

;setting registers
mov rax, r15
add rax, rcx ;counter+pointer to begining of gmap
sub rax, rdi ;row abowe
sub rax, 4 ;top left corner
pmovzxbw xmm6, [rax] 
add rax, 4 ;1 pixel right
pmovzxbw xmm7, [rax]
add rax, 4
pmovzxbw xmm8, [rax]
add rax, rdi ;row below
pmovzxbw xmm9, [rax]
sub rax, 4
pmovzxbw xmm10, [rax]
sub rax, 4
pmovzxbw xmm11, [rax]
add rax, rdi
pmovzxbw xmm12, [rax]
add rax, 4
pmovzxbw xmm13, [rax]
add rax, 4
pmovzxbw xmm14, [rax]

;multiply by weights
pmullw xmm6, xmm0
pmullw xmm7, xmm1
pmullw xmm8, xmm2
pmullw xmm9, xmm3
pmullw xmm10, xmm4
pmullw xmm11, xmm5
pmullw xmm12, xmm0
pmullw xmm13, xmm1
pmullw xmm14, xmm2

;sum pixels
paddw xmm6, xmm7
paddw xmm6, xmm8
paddw xmm6, xmm9
paddw xmm6, xmm10
paddw xmm6, xmm11
paddw xmm6, xmm12
paddw xmm6, xmm13
paddw xmm6, xmm14

;alpha?

psrlw xmm6, 4 ;divide by weights sum
pextrw r9, xmm6, 0 ;red
pextrw r13, xmm6, 1 ;green
pextrw r14, xmm6, 2 ;blue

mov r11, r15 ;start of bmap
add r11, rcx ;kernel center

;jmp endblur

pextrb byte ptr [r11+0], xmm6, 0		
pextrb byte ptr [r11+1], xmm6, 1		
pextrb byte ptr [r11+2], xmm6, 2

cmp rcx, r12 ;sprawdz czy licznik osiagnal punkt koncowy algorytmu
jae endBlur
jmp loop1

endBlur:
ret

gaussianBlur endp
end