using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

class Program {
    static void MSort(Func<Tuple<int,int>, Tuple<int,int>, bool?> compare, Tuple<int,int>[] fractions, int x, int y) {
        if (x >= y) return;
        int m = (x + y) / 2;
        if (y - x >= 1) {
            MSort(compare, fractions, x, m);
            MSort(compare, fractions, m + 1, y);
        }
        //else if (threshold > 1) QSort(compare, fractions, x, y);
        Merge(compare, fractions, x, m, y);
    }

    static void Merge(Func<Tuple<int, int>, Tuple<int, int>, bool?> compare, Tuple<int, int>[] fractions, int x, int m, int y) {
        Tuple<int, int>[] temp = new Tuple<int, int>[y - x + 1]; // create an array of the same size as the range
        int i = x;
        int j = m + 1;
        int k = 0;
        while (i <= m && j <= y) {
            if (compare(fractions[i], fractions[j]) == false) {
                temp[k] = fractions[j];
                j++; k++;
            }
            else {
                temp[k] = fractions[i];
                i++; k++;
            }
        }
        while (i <= m) {
            temp[k] = fractions[i];
            k++;
            i++;
        }
        while (j <= y) {
            temp[k] = fractions[j];
            k++;
            j++;
        }
        for (int p = 0; p < temp.Length; p++) {
            fractions[x + p] = temp[p];
        }
    }
    
    static bool? NumeratorIsSmaller(Tuple<int,int> a, Tuple<int,int> b) {
        if (a.Item1 == b.Item1) return null;
        return a.Item1 < b.Item1;
    }
    
    static bool? DenominatorIsLarger(Tuple<int,int> a, Tuple<int,int> b) {
        if (a.Item2 == b.Item2) return true;
        return a.Item2 > b.Item2;
    }

    static bool? ValueIsSmaller(Tuple<int, int> a, Tuple<int, int> b) {
        long p = (long)a.Item1 * (long)b.Item2;
        long q = (long)b.Item1 * (long)a.Item2;
        if (a.Item1 == b.Item1 && a.Item2 == b.Item2) return null;
        return p < q;
    }

    // making the output table for every sorted version of the list
    static void AddToOutput(string[] output, Tuple<int, int>[] fractions) {
        int i = 0;
        foreach (var frac in fractions) {
            output[i] += Fractionifier(frac) + "  ";
            i++;
        }
    }

    static string Fractionifier(Tuple<int, int> t) {
        return t.Item1 + "/" + t.Item2;
    }

    static void DeepCopy(Tuple<int, int>[] original, Tuple<int,int>[] copy) {
        for (int i = 0; i < original.Length; i++) {
            copy[i] = original[i];
        }
    }

    static void Main(string[] args) {
        int n = int.Parse(Console.ReadLine().Split(' ')[0]);
        Tuple<int, int>[] fractions = new Tuple<int, int>[n];
        for (int i = 0; i < n; i++) { // read out the input into the 'fractions' array
            string[] fracString = Console.ReadLine().Split(' ')[0].Split('/');
            Tuple<int, int> frac = new Tuple<int, int>(int.Parse(fracString[0]), int.Parse(fracString[1]));
            fractions[i] = frac;
        }

        string[] output = new string[n];

        Tuple<int, int>[] fractionsOriginal = new Tuple<int, int>[n];
        fractionsOriginal = (Tuple<int,int>[])fractions.Clone();

        //first the column with the fractions sorted by numerator, then by denominator and then by value, and finally printing it
        MSort(NumeratorIsSmaller, fractions, 0, fractions.Length - 1);
        AddToOutput(output, fractions);
        fractions = (Tuple<int,int>[])fractionsOriginal.Clone();
        
        MSort(DenominatorIsLarger, fractions, 0, fractions.Length - 1);
        AddToOutput(output, fractions);
        fractions = (Tuple<int,int>[])fractionsOriginal.Clone();
        
        MSort(ValueIsSmaller, fractions, 0, fractions.Length - 1);
        AddToOutput(output, fractions);
        
        Console.WriteLine(n);
        foreach (var s in output) {
            Console.WriteLine(s);
        }
    }
}