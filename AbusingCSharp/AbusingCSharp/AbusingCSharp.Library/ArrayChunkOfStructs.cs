using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AbusingCSharp.Library
{
    public class ArrayChunkOfStructs<T> where T : struct
    {
        private readonly T[] _array;

        public ArrayChunkOfStructs(int size)
        {
            _array = new T[size];
        }

        public ref T this[int index] => ref _array[index];

        public ref T ItemRef(int index)
        {
            return ref Unsafe.Add(ref _array[0], index);
        }
        public ref T ItemRef2(int index)
        {
            var span = new Span<T>(_array);
            return ref Unsafe.Add(ref MemoryMarshal.GetReference(span), index);
        }

        public ref T ItemRef3(int index)
        {
            ref var data = ref MemoryMarshal.GetArrayDataReference(_array);
            return ref Unsafe.Add(ref data, index);
        }
    }
}
