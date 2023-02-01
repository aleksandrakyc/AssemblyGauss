.data
kernel: 
oword 0

.code

;params: bmap data - rcx, kernel - rdx, bmap width - r8, start index - r9, end index - stack
;r10 - loopcounter

;1pixel=16

gaussianBlur proc EXPORT

mov				r12, rdx
mov				ebx, dword ptr[rbp + 48]	;move start index from stack (cannot move to r10 directly because of size mismatch)
mov				r10, rbx					;move start index(row) to r10

;mul r8 by bytes in pixel
mov				rax, 16
mul				r8
mov				r8, rax

sub				r10, 1
mov				rax, r8						
mul				r10
mov				r10, rax					;loop counter - number of ROWS to change
add				r10, rcx					;last pixel

mov				rax, r8						;width
mul				r9
mov				r9, rax
add				rcx, r9						;start at starty*beginning of map

mov				r11, r8
sub				r11, 32						;hold imagewidth-2 : -1 pixel from both sides

;get kernel - make the placement make sense
movdqu			xmm10, oword ptr [r12]		;corner val
movdqu			xmm11, oword ptr [r12+16]	;side val
movdqu			xmm12, oword ptr [r12+32]	;center val

incX:
sub				r11, 16
cmp				r11, 0h						;we reach end of line
je				incY						;if we do, skip those edge pixels
add				rcx, 16						;if not, continue as normal

;actual gauss:

movdqu			xmm1, oword ptr [rcx]		;put hopefully pixel values to xmm
add				rcx, 16
movdqu			xmm2, oword ptr [rcx]
add				rcx, r8
movdqu			xmm3, oword ptr [rcx]
sub				rcx, 16
movdqu			xmm4, oword ptr [rcx]
sub				rcx, 16
movdqu			xmm5, oword ptr [rcx]
sub				rcx, r8
movdqu			xmm6, oword ptr [rcx]
sub				rcx, r8
movdqu			xmm7, oword ptr [rcx]		
add				rcx, 16
movdqu			xmm8, oword ptr [rcx]		
add				rcx, 16
movdqu			xmm9, oword ptr [rcx]
add				rcx, r8
sub				rcx, 16
;movdqu			xmm10, oword ptr [rcx]		;test if were back home

;multiply by weights

;center pixel
mulps			xmm1, xmm12
;side pixels
mulps			xmm2, xmm11
mulps			xmm4, xmm11
mulps			xmm6, xmm11
mulps			xmm8, xmm11
;corner pixels
mulps			xmm3, xmm10
mulps			xmm5, xmm10
mulps			xmm7, xmm10
mulps			xmm9, xmm10

;add them up
addps			xmm8, xmm9
addps			xmm7, xmm8
addps			xmm6, xmm7
addps			xmm5, xmm6
addps			xmm4, xmm5
addps			xmm3, xmm4
addps			xmm2, xmm3
addps			xmm1, xmm2

MOVDQU			xmm13, xmm1					;see final values	
MOVDQU			[rcx], xmm1

jmp				incX				

endBlur:
ret

incY:
mov				r11, r8
sub				r11, 32						;hold imagewidth-2 : -1 from both sides
add				rcx, 48						;skip 3 pix - first iteration fd up
cmp				r10, rcx
jl				endblur
jmp				incX

gaussianBlur endp
end