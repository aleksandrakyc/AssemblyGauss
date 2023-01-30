.data
kernel: 
oword 0

.code

;params: bmap data - rcx, bmap size - rdx, bmap width - r8, start index - r9, end index - stack
;r10 - loopcounter

gaussianBlur proc EXPORT

mov				ebx, dword ptr[rbp + 48]	;move start index from stack (cannot move to r10 directly because of size mismatch)
mov				r10, rbx					;move start index(row) to r10
;mov				rax, r8
;mul				r10							;multiply end row index by image width - number of pixels
;mov				r10, rax		
sub				r10, r9						;loop counter - number of pixels to change
add				rcx, r9						;counter starts at bmap beginning+start index
;add				rdx, rcx					;rdx holds the end - unnecessary?

imageLoop:

cmp				rdx, 0h						;if we reach end of bmap, end proc
je				endBlur

pmovzxbw		xmm1, [rcx]					;put hopefully pixel values to xmm
pmovzxbw		xmm2, [rcx+8]
;movdqu			xmm1, oword ptr[rcx]		;cant multiply bytes?
movdqu			xmm0, oword ptr[kernel]		;get out 0s ready
pmulhw			xmm1, xmm0					;multiply by 0?
pmulhw			xmm2, xmm0					;multiply by 0?

xorps			xmm3, xmm3					;need to clear the register to make operations on it?
xorps			xmm4, xmm4
PACKUSWB 		xmm3, xmm1					;returns bytes to their previous form
PACKUSWB 		xmm4, xmm2
MOVHLPS			xmm4, xmm3					;now our hopefully processed bytes are back in their byte form.

MOVDQU			oword ptr [rcx], xmm4		;;-;

add				rcx, 16						;move forward
sub				rdx, 1						;we worked on 2 pix
jmp				imageLoop				

endBlur:
 ret

gaussianBlur endp
end