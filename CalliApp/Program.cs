// Undef to check whether a delegate of proper return type/arguments needs to
// be defined for calli to work (current finding - it is not necessary)
//#define DEFINE_DELEGATE

using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security;

namespace CalliApp
{
    class Program
    {
        [SuppressUnmanagedCodeSecurity] // Investigate!
        [DllImport("UnmanagedLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int fnUnmanaged(int argument);

        public delegate int Callback(int param);
        [SuppressUnmanagedCodeSecurity] // Investigate!
        [DllImport("UnmanagedLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int fnUnmanagedWithCallback(int argument, Callback callback);

#if DEFINE_DELEGATE
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int UnmanagedFnDelegate(int argument);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int UnmanagedFnDelegateWithCallback(int argument, Callback callback);
#endif

        static void Main(string[] args)
        {
            Console.Write("> ");
            int value = int.Parse(Console.ReadLine());
            //UseExplicitPInvoke(value);
            //UseLoadLibrary(value);
            //UseCalli(value);

            //UseExplicitPInvokeWithCallback(value);
            //UseLoadLibraryWithCallback(value);
            UseCalliWithCallback(value);
            UseCalliWithCallback(10);
        }
        static int CallbackExample(int param)
        {
            Console.WriteLine($"From callback: {param}");
            return param * param;
        }

        static void UseExplicitPInvoke(int argument)
        {
            int result = fnUnmanaged(argument);
            Console.WriteLine(result);
        }

        static void UseExplicitPInvokeWithCallback(int argument)
        {
            int result = fnUnmanagedWithCallback(argument, CallbackExample);
            Console.WriteLine(result);
        }

#if DEFINE_DELEGATE
        static void UseLoadLibrary(int argument)
        {
            IntPtr libraryPtr = WinApiMethods.LoadLibrary("UnmanagedLibrary.dll");
            IntPtr methodPtr = WinApiMethods.GetProcAddress(libraryPtr, "fnUnmanaged");
            var fnDelegate =
                (UnmanagedFnDelegate) Marshal.GetDelegateForFunctionPointer(methodPtr, typeof(UnmanagedFnDelegate));

            int result = fnDelegate(argument);
            Console.WriteLine(result);
        }

        static void UseLoadLibraryWithCallback(int argument)
        {
            IntPtr libraryPtr = WinApiMethods.LoadLibrary("UnmanagedLibrary.dll");
            IntPtr methodPtr = WinApiMethods.GetProcAddress(libraryPtr, "fnUnmanagedWithCallback");
            var fnDelegate =
                (UnmanagedFnDelegateWithCallback)Marshal.GetDelegateForFunctionPointer(methodPtr, typeof(UnmanagedFnDelegateWithCallback));

            int result = fnDelegate(argument, CallbackExample);
            Console.WriteLine(result);
        }
#endif

        static void UseCalli(int argument)
        {
            IntPtr libraryPtr = WinApiMethods.LoadLibrary("UnmanagedLibrary.dll");
            IntPtr methodPtr = WinApiMethods.GetProcAddress(libraryPtr, "fnUnmanaged");

            var method = new DynamicMethod("CalliInvoke", typeof(int), new []{ typeof(int)});
            var generator = method.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldc_I8, methodPtr.ToInt64());
            generator.EmitCalli(OpCodes.Calli, CallingConvention.Cdecl, typeof(int), new []{ typeof(int)});
            generator.Emit(OpCodes.Ret);
            var result = method.Invoke(null, new[] {(object)argument});
            Console.WriteLine(result);
        }

        static private Callback callback = new Callback(CallbackExample);
        static void UseCalliWithCallback(int argument)
        {
            IntPtr libraryPtr = WinApiMethods.LoadLibrary("UnmanagedLibrary.dll");
            IntPtr methodPtr = WinApiMethods.GetProcAddress(libraryPtr, "fnUnmanagedWithCallback");

            var method = new DynamicMethod("CalliInvoke", typeof(int), new[] { typeof(int), typeof(Callback) });
            var generator = method.GetILGenerator();
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldc_I8, methodPtr.ToInt64());
            generator.EmitCalli(OpCodes.Calli, CallingConvention.Cdecl, typeof(int), new[] { typeof(int), typeof(Callback) });
            generator.Emit(OpCodes.Ret);
            var result = method.Invoke(null, new[] { (object)argument, callback});

            // NOTE: we can use Marshal.GetFunctionPointerForDelegate(callback) to get a function pointer that is callable from unmanaged code
            
            Console.WriteLine(result);
        }
    }

    static class WinApiMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }
}
