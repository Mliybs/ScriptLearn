using System;
using System.Collections;
using System.Collections.Generic;
using Another;

namespace C_Sharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Anothers.Car car = new Anothers.Car(() => Console.WriteLine("测你妈"));

            car.Invoke();
        }
    }
}
