.class public CILApp.Program
{
	// Trivial learnig experiment to test wrapping a method with one more parameter
	.method public static int32 JitThunk(int32 number) cil managed
	{		
		ldarg.0
		ldc.i4 0x1000
		call int32 CILApp.Program::JitStub(int32, int32)
		ret
	}

	.method public static int32 JitStub(int32 number, int32 number2) cil managed
	{
		ldarg.0
		dup
		mul
		ldarg.1
		add
		ret
	}

	// Trivial example of incorrect type passed to a method
	// the result is: 44, because JIT does not check types, it assumes
	// string.Length is the first field of the passed argument:
	// ConsumeString(System.String)
    //		L0000: mov eax, [rdx+0x8]
    //		L0003: ret
	.method public static int32 IncorrectTypeUse() cil managed
	{		
		newobj instance void CILApp.C::.ctor()
		call int32 CILApp.Program::ConsumeString(string)
		ret
	}

	.method public static int32 ConsumeString(string number) cil managed
	{		
		ldarg.0
        callvirt instance int32 [System.Private.CoreLib]System.String::get_Length()
        ret
	}
}

.class public CILApp.C extends [System.Private.CoreLib]System.Object
{
	.field public int32 X

	.method public hidebysig specialname rtspecialname instance void .ctor () cil managed 
    {
		ldarg.0
        ldc.i4.s 44
        stfld int32 CILApp.C::X
        ldarg.0
        call instance void [System.Private.CoreLib]System.Object::.ctor()
        ret
    }
}
