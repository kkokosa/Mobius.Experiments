using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AbusingCSharp.Library
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Shape
    {
        public enum ShapeType : int
        {
            Rectangle,
            Circle,
            Triangle
        }

        [FieldOffset(0)]
        public ShapeType Type;
        [FieldOffset(4)]
        internal float a;
        [FieldOffset(8)]
        internal float b;
        [FieldOffset(12)]
        internal float c;

        [StructLayout(LayoutKind.Explicit)]
        public struct Rectangle
        {
            [FieldOffset(4)]
            public float Width;
            [FieldOffset(8)]
            public float Height;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct Circle
        {
            [FieldOffset(4)]
            public float Radius;
        }

        public static ref Rectangle AsRectangle(ref Shape shape)
            => ref Unsafe.As<Shape, Rectangle>(ref shape);
        public static ref Circle AsCircle(ref Shape shape)
            => ref Unsafe.As<Shape, Circle>(ref shape);
    }
}
