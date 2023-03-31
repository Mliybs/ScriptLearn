using System;

namespace Another
{
    public static class Anothers
    {
        public delegate void Car();
    }

    public class AnotherDual
    {
        public AnotherDual ouo() => this;
    }

    public static class Statics
    {
        public static void Print(this object self) => Console.WriteLine(self);
    }
}