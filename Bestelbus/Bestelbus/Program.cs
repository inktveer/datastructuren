using System;
using System.Collections.Generic;

namespace Bestelbus;

class Program
{
    static Tuple<List<long>, long> readInput()
    {
        List<long> packets = new List<long>();
        string[] first = Console.ReadLine().Split(" ");
        long n = long.Parse(first[0]);
        long r = long.Parse(first[1]);

        while (packets.Count <= n)
        {
            string[] line = Console.ReadLine().Split(new char[] { ' ', '\r' });
            foreach (var i in line)
            {
                packets.Add(long.Parse(i));
            }
        }

        return new Tuple<List<long>, long>(packets, r);
    }

    // maxweight moet groter zijn dan het grootste element in de lijst
    static int ritten(int maxWeight, long maxRitten, List<long> packets)
    {
        int res = 0;
        for(int i = 0; i < )
    }

    static long bereken(Tuple<List<long>, long> input)
    {
        List<long> packets = input.Item1;
        long n = packets.Count;
        long r = input.Item2;
        long maxWeight = packets.Max();
        long totalWeight = packets.Sum();
        
        

    }
    
    static void Main(string[] args)
    {
        
    }
}