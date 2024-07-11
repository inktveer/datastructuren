using System;
using System.Collections.Generic;

class Program
{
    static long collatzLogic(long i) {
        if (i % 2 == 0) {
            return i / 2;
        }
        return i * 3 + 1;
    }

    static long collatz(long i) {
        List<long> list = new List<long>();
        while (i != 1) {
            list.Add(i);
            i = collatzLogic(i);
        }
        return list.Count;
    }

    static void Main(string[] args) {
        List<long> inputs = new List<long>();
        for (int i = 0; i < 4; i++) {
            inputs.Add(long.Parse(Console.ReadLine()));
        }
        foreach (var i in inputs) {
            Console.WriteLine(collatz(i));
        }
    }
}