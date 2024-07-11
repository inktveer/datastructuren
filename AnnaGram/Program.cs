using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;


class Program {
    static void Main(string[] args) {
        string[]                  firstline  = Console.ReadLine().Split(' ');
        char                      mode       = char.Parse(firstline[1]);
        string                    wordstring = Console.ReadLine();
        Word                      word       = new Word(wordstring,         null, Operation.Root);
        Word                      targetWord = new Word(Console.ReadLine(), word, Operation.Root);
        Processing                processing = new Processing(word, targetWord, wordstring.Length);
        List<Tuple<char, string>> path       = processing.BacktrackPath(word, processing.Search());

        Print(mode, path, wordstring.Length, wordstring);
    }

    static void Print(char mode, List<Tuple<char, string>> path, int length, string start) {
        Console.WriteLine(path.Count);
        switch (mode) {
            case 'S':
                foreach (var t in path) {
                    Console.Write(t.Item1);
                }
                break;
            case 'A':
                foreach (var t in path) {
                    Console.Write(t.Item1);
                }
                Console.Write("\nS " + start + ".?\n");

                foreach (var t in path) {
                    Console.WriteLine(t.Item1 + " " + t.Item2.Substring(t.Item2.Length - length - 2));
                }

                break;
        }
    }
}

public class Processing {
    //iets van een dictionary met alle al geweeste dingen
    //iets van een queue met alle dingen die nog uitgerekend moeten worden
    private Word          Word;
    private Word          TargetWord;
    private HashSet<ulong> Processed;
    private Queue<Word>   ToDo;
    private int           Length;
    private string        wordstring;

    public Processing(Word word, Word targetWord, int length) {
        Word       = word;
        Processed  = new HashSet<ulong>();
        ToDo       = new Queue<Word>();
        Length     = length;
        TargetWord = targetWord;
        
        ToDo.Enqueue(Word);
        

        
    }
    
    public Word Search() {
        Word iterator = Word;
        while (!Processed.Contains(TargetWord.Data)) {
            iterator = ToDo.Dequeue();
            if (Processed.Add(iterator.Data)) {
                //RECHTS
                Word right = iterator.Copy().Rstep(Length);
                right.Parent    = iterator;
                right.Operation = Operation.RightStep;
                ToDo.Enqueue(right);

                //LINKS
                Word left = iterator.Copy().Lstep(Length);
                left.Parent    = iterator;
                left.Operation = Operation.LeftStep;
                ToDo.Enqueue(left);

                //WISSEL
                Word swap = iterator.Copy().Swap(Length);
                swap.Parent    = iterator;
                swap.Operation = Operation.Swap;
                ToDo.Enqueue(swap);
            }

        }

        return iterator;
    }
    
    // debug
    static string ToBinary(ulong n) {
        if (n < 2) return n.ToString();

        var divisor   = n / 2;
        var remainder = n % 2;

        return ToBinary(divisor) + remainder;
    }

    // debug
    void PrintBinary(Word n) {
        Console.WriteLine(n);
    }


    Word AddPath(Word word, ulong mode) {
        ulong mask = ulong.MaxValue >> 4;
        word.Data &= mask;
        word.Data |= mode << 60;
        return word;
    }

    Word RemovePath(Word word) {
        word.Data &= ulong.MaxValue >> 4;
        return word;
    }

    ulong GetPath(Word word) {
        return (ulong)(word.Data >> 60);
    }

    public List<Tuple<char,string>> BacktrackPath(Word word, Word targetWord) {
        Word                      iterator = targetWord;
        List<Tuple<char, string>> list     = new List<Tuple<char, string>>();

        while (iterator.Data != word.Data) {
            char operation;
            switch (iterator.Operation) {
                case Operation.LeftStep:
                    operation = 'L';
                    break;
                case Operation.RightStep:
                    operation = 'R';
                    break;
                case Operation.Swap:
                    operation = 'W';
                    break;
                case Operation.Root:
                    operation = '>';
                    break;
                default:
                    operation = '?';
                    break;
            }
            
            list.Add(new Tuple<char, string>(
                         operation,
                         iterator.ToString())
                );

            iterator = iterator.Parent;
        }
        
        list.Reverse();
        return list;
    }
}

public enum Operation {
    RightStep,
    LeftStep,
    Swap,
    Root
}












public class Word {
    public ulong Data;
    public Word  Parent;
    public Operation  Operation;
    
    public Word(string word, Word parent, Operation operation) {
        Data      = FindWordValue(word);
        Parent    = parent;
        Operation = operation;
    }
    public Word() { }
    
    private ulong this[int index] {
        get => (ulong)((Data >> (index * 5)) & 0b11111);
        set => Data = (Data & ~((ulong)0b11111 << 5 * index)) | (ulong)value << 5 * index; 
    }

    public Word Copy() {
        Word copy = new Word();
        copy.Data      = Data;
        return copy;
    }
    
    public override string ToString() {
        string res = string.Empty;
        for (int i = 10; i >= 0; i--) {
            res += Word.FindLetter(this[i]);
        }
        
        return res + "." + Word.FindLetter(this[11]);
    }

    public static char GetMode(ulong i) {
        switch (i) {
            case 1:  return 'R';
            case 2:  return 'L';
            default: return 'W';
        }
    }
    
    public Word Rstep(int length) {
        Word res   = this;
        ulong first = res[0];
        ulong plate = res[11];
        res.Data                 >>= 5;
        res.Data                 &=  ~(ulong.MaxValue << (5 * length));
        res[length - 1] =   first;
        res[11]                  =   plate;
        return res;
    }

    public Word Lstep(int length) {
        Word res   = this;
        ulong last  = res[length - 1];
        ulong plate = res[11];
        res.Data <<= 5;
        res.Data &=  ~(ulong.MaxValue << (5 * length));
        res[0]   =   last;
        res[11]  =   plate;
        return res;
    }

    public Word Swap(int length) {
        Word res   = this;
        ulong plate = res[11]; // Max woordlengte is 11 dus op de 12e plek kan de swap ulong staan
        res[11] = res[0];
        res[0]  = plate;
        return res;
    }

    // Geeft de waarde van een bepaalde letter om in een ulong te plaatsen
    public static ulong FindValue(char c) {
        return (ulong)((ulong)c - 0x60);
    }

    public static ulong FindWordValue(string s) {
        ulong res = 0;
        for (int i = 0; i < s.Length; i++) {
            ulong letter = FindValue(s[s.Length - i - 1]);
            res |= letter << (i * 5);
        }
        return res;
    }

    // Converteert een bepaalde waarde in een ulong naar een letter
    public static char FindLetter(ulong b) {
        return b == 0 ? '?' : (char)(b + 0x60);
    }
}
