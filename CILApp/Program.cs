using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CILApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int number = int.Parse(Console.ReadLine());
            Console.WriteLine(JitThunk(number));

            Console.WriteLine(IncorrectTypeUse());
            Console.WriteLine(IncorrectTypeUseWithDynamic());
        }

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern int JitThunk(int number);

        [MethodImpl(MethodImplOptions.ForwardRef)]
        public static extern int IncorrectTypeUse();

        public static int IncorrectTypeUseWithDynamic()
        {
            var s = new object();
            // Throws: Unhandled Exception: Microsoft.CSharp.RuntimeBinder.RuntimeBinderException: 'object' does not contain a definition for 'Length'
            return N(s);
        }

        public static int N(dynamic s)
        {
            return s.Length;
        }
    }
}
