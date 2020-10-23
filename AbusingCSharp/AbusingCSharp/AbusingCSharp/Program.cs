using AbusingCSharp.Library;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AbusingCSharp
{
    public record Person
    {
        public DateTime RecordedAt { get; init; }
        public string LastName { get; }
        public string FirstName { get; }

        public Person(string first, string last) => (FirstName, LastName) = (first, last);
    }

    class Program
    {
        static void Main(string[] args)
        {
            Shape[] arrayOfShapes = new Shape[128];
            var area = CalculateArea(ref arrayOfShapes[4]);
            Console.WriteLine($"Area is {area}");
        }

        private static float CalculateArea(ref Shape shape)
        {
            switch (shape.Type)
            {
                case Shape.ShapeType.Rectangle:
                {
                    ref var rect = ref Shape.AsRectangle(ref shape);
                    return rect.Height * rect.Width;
                }
                case Shape.ShapeType.Circle:
                {
                    ref var circle = ref Shape.AsCircle(ref shape);
                    return 2.0f * (float)Math.PI * circle.Radius;
                }
                default: return 0.0f;
            }
        }
    }

    
}
