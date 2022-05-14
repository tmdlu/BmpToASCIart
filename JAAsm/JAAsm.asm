.code
MyProc1 proc
add RCX, RDX
mov RAX, RCX
ret
MyProc1 endp
.code
gray2432funcASM PROC                     ;Function calculates gray value of a pixel, selects and returns ASCII value        

                             
		;read values of pixel from stack
		cvtsi2sd xmm0, ecx			     ;Convert DWORD integer - value of blue color to scalar double
		cvtsi2sd xmm1, edx				 ;Convert DWORD integer - value of green color to scalar double
		cvtsi2sd xmm2, r8d               ;Convert DWORD integer - value of red color to scalar double
        mulsd   xmm0, QWORD PTR val011   ;multiplying the value of the xmm0 (value of blue) with val011 (0.11) 
        mulsd   xmm1, QWORD PTR val059   ;multiplying the value of the xmm1 (value of green) with val03 (0.59) 

        addsd   xmm0, xmm1				 ;adding values of xmm1 (0.11 * value of blue) and xmm1 (0.59 * value of green)					
        mulsd   xmm2, QWORD PTR val03    ;multiplying the value of the xmm2 (value of red) with val03 (0.3) 
        addsd   xmm0, xmm2               ;adding values of xmm0 (0.11 * value of blue + 0.59 * value of green) and xmm2 (0.3 * value of red)			
        divsd   xmm0, QWORD PTR val255	 ;dividing value of register xmm0 (0.11 * value of blue + 0.59 * value of green + 0.3 * value of red) by val255 (255)
        comisd  xmm0, QWORD PTR val067   
        jbe     ifMid					 ;if xmm0 (value of grayscale) =< 0.67 jumping to ifMid segment
        subsd   xmm0, QWORD PTR val067   ;xmm0 (value of grayscale) subtract val067 (0.67)
        mulsd   xmm0, QWORD PTR val77    ;multiplying the value of the xmm0 (value of grayscale) with val77 (77)
        addsd   xmm0, QWORD PTR val65    ;adding values of xmm0 (value of grayscale) and val65 (65)		
        jmp     return					 ;jumping to return segment

ifMid:
        comisd  xmm0, QWORD PTR val034
        jb      ifBottom                 ;if xmm0 (value of grayscale) < 0.34 jumping to ifBottom segment
        subsd   xmm0, QWORD PTR val034   ;xmm0 (value of grayscale) subtract val034 (0.34)
        mulsd   xmm0, QWORD PTR val78    ;multiplying the value of the xmm0 (value of grayscale) with val78 (78)
        addsd   xmm0, QWORD PTR val97    ;adding values of xmm0 (value of grayscale) and val97 (97)
        jmp     return					 ;jumping to return segment

ifBottom:
        mulsd   xmm0, QWORD PTR val46    ;multiplying the value of the xmm0 (value of grayscale) with val46 (46)
        addsd   xmm0, QWORD PTR val32    ;adding values of xmm0 (value of grayscale) and val32 (32)
 
return:
		cvttsd2si eax, xmm0              ;Converts double to DWORD integer
      
        ret     0
gray2432funcASM ENDP                     ;End of function

gray16funcASM PROC                     ;Function calculates gray value of a pixel, selects and returns ASCII value        

                             
		;read values of pixel from stack
		cvtsi2sd xmm0, ecx			     ;Convert DWORD integer - value of blue color to scalar double
		cvtsi2sd xmm1, edx				 ;Convert DWORD integer - value of green color to scalar double
		cvtsi2sd xmm2, r8d               ;Convert DWORD integer - value of red color to scalar double
        mulsd   xmm0, QWORD PTR val011   ;multiplying the value of the xmm0 (value of blue) with val011 (0.11) 
        mulsd   xmm2, QWORD PTR val03    ;multiplying the value of the xmm2 (value of red) with val03 (0.3) 
		addsd   xmm0, xmm2               ;adding values of xmm0 (0.11 * value of blue) and xmm2 (0.3 * value of red)
		divsd   xmm0, QWORD PTR val32	 ;dividing value of register xmm0 (0.11 * value of blue + 0.3 * value of red) by val32 (32)
		mulsd   xmm1, QWORD PTR val059   ;multiplying the value of the xmm1 (value of green) with val059 (0.59) 
		divsd	xmm1, QWORD PTR val63	 ;dividing value of register xmm1 (0.59 * value of green) by val63 (63)
		addsd   xmm0, xmm1				 ;adding values of xmm0 ((0.11 * value of blue + 0.3 * value of red)/32) and xmm1 ((0.59 * value of green)/63)					
        comisd  xmm0, QWORD PTR val067   
        jbe     ifMid					 ;if xmm0 (value of grayscale) =< 0.67 jumping to ifBottom segment
        subsd   xmm0, QWORD PTR val067   ;xmm0 (value of grayscale) subtract val067 (0.67)
        mulsd   xmm0, QWORD PTR val77    ;multiplying the value of the xmm0 (value of grayscale) with val77 (77)
        addsd   xmm0, QWORD PTR val65    ;adding values of xmm0 (value of grayscale) and val65 (65)		
        jmp     return					 ;jumping to return segment

ifMid:
        comisd  xmm0, QWORD PTR val034
        jb      ifBottom                 ;if xmm0 (value of grayscale) < 0.34 jumping to ifMid segment
        subsd   xmm0, QWORD PTR val034   ;xmm0 (value of grayscale) subtract val034 (0.34)
        mulsd   xmm0, QWORD PTR val78    ;multiplying the value of the xmm0 (value of grayscale) with val78 (78)
        addsd   xmm0, QWORD PTR val97    ;adding values of xmm0 (value of grayscale) and val97 (97)
        jmp     return					 ;jumping to return segment

ifBottom:
        mulsd   xmm0, QWORD PTR val46    ;multiplying the value of the xmm0 (value of grayscale) with val46 (46)
        addsd   xmm0, QWORD PTR val32    ;adding values of xmm0 (value of grayscale) and val32 (32)
 
return:
		cvttsd2si eax, xmm0              ;Converts double to DWORD integer
       
        ret     0
gray16funcASM ENDP                       ;End of function



.data
;Costant values
val255 DQ 255.0  
val97 DQ 97.0   
val78 DQ 78.0   
val77 DQ 77.0   
val65 DQ 65.0  
val63 DQ 63.0
val46 DQ 46.0  
val32 DQ 32.0  
val067 DQ 0.67  
val059 DQ 0.59 
val034 DQ 0.34 
val03 DQ 0.3 
val011 DQ 0.11 



end


