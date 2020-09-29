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
            
            Console.WriteLine("Hello World!");            
        }
    }

    
}
