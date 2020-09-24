// UnmanagedLibrary.cpp : Defines the exported functions for the DLL.
//

#include "framework.h"
#include "UnmanagedLibrary.h"


// This is an example of an exported variable
__declspec(dllexport) int nUnmanaged=0;

__declspec(dllexport) int fnUnmanaged(int argument)
{
    return argument * argument;
}

__declspec(dllexport) int fnUnmanagedWithCallback(int argument, PFN_MYCALLBACK callback)
{
	// It seems it translates simply to (ecx = argument, edx = callback):
    // mov         eax, ecx
    // imul        eax, ecx
    // mov         ecx, eax
    // call        qword ptr [rdx]
    int c = callback(argument * argument);
	// inc         eax
    return c + 1;
}
