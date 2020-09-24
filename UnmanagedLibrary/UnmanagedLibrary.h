extern "C" __declspec(dllexport) int nUnmanaged;
extern "C" __declspec(dllexport) int fnUnmanaged(int argument);

typedef int(__stdcall* PFN_MYCALLBACK)(int );
extern "C" __declspec(dllexport) int fnUnmanagedWithCallback(int argument, PFN_MYCALLBACK callback);
