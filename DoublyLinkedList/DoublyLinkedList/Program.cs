using System;
using System.Collections.Generic;
using System.Linq;

class Program {
    static void Main(string[] args) {
        string[] firstLine = Console.ReadLine().Split(' ');
        char mode = char.Parse(firstLine[0]);
        int n = int.Parse(firstLine[1]);
        
        // n lines with a value of 0 to 100 (7-bit number)
        LinkedList linkedList = new LinkedList();
        if (mode == 'D') linkedList.D = true;
        if (mode == 'P') linkedList.P = true;
        LinkedList.Node current = linkedList.tail;
        
        for (int i = 0; i < n; i++) {
            string[] input = Console.ReadLine().Split();
            LinkedList.Node nodeToAdd = new LinkedList.Node() {
                Value = sbyte.Parse(input[0]),
                Operation = char.Parse(input[1]),
                x = sbyte.Parse(input[2]),
                y = input[1][0] == 'A' ? sbyte.Parse(input[3]) : (sbyte)0
            };
            
            linkedList.Append(nodeToAdd);
            current = current.Next;
            linkedList.front.Previous = current;
        }
        
        linkedList.ExecuteAll();
        
        Print(mode, linkedList);
    }

    private static void Print(char mode, LinkedList linkedList) {
        switch (mode) {
            case 'C':
                Console.WriteLine(linkedList.Count);
                break;
            case 'S':
                int i = 0;
                LinkedList.Node n = linkedList.tail.Next;
                while (i < linkedList.Count) {
                    Console.WriteLine(n.Value);
                    n = n.Next;
                    i++;
                }
                break;
            case 'D':
                List<string> freq = new List<string>();
                for (int k = 0; k < 100; k++) {
                    int j = linkedList.Frequency[k];
                    if (j == 0) continue;
                    freq.Add(k + ": " + j);
                }
                foreach (string s in freq) Console.WriteLine(s);
                break;
        }
    }
}


public class LinkedList {
    public Node front; 
    public Node tail;
    public int Count = 0;
    public bool P = false;
    public bool D = false;
    public int[] Frequency = new int[100];
    public List<string> PGraph = new List<string>();
    public LinkedList() {
        front = new Node() {
            Value = -1,
            x = 1
        };
        tail = new Node() {
            Value = -1,
            x = -1
        };
        front.Next = front;
        front.Previous = tail;
        tail.Previous = tail;
        tail.Next = front;
    }
    
    public class Node {
        public Node Previous;
        public Node Next;
        public sbyte Value;
        public sbyte x;
        public sbyte y;
        public char Operation;
    }

    public void Add(Node nodeToAdd, Node iterator, sbyte index) {
        sbyte i = 0;
        Node n = iterator;
        
        if (index < 0) {
            while (i != index) {
                n = n.Previous;
                i--;
            }
            if (n == tail) return;
        }
        else {
            while (i != index) {
                n = n.Next;
                i++;
            }
            if (n == front) return;
        }

        nodeToAdd.Operation = 'N';
        nodeToAdd.Previous = n.Previous;
        nodeToAdd.Next = n;
        nodeToAdd.Previous.Next = nodeToAdd;
        nodeToAdd.Next.Previous = nodeToAdd;
        Count++;
        if (D) Frequency[int.Parse(nodeToAdd.Value.ToString())]++;
        if (P) Console.WriteLine("toegevoegd " + nodeToAdd.Value);
    }

    public void Append(Node nodeToAdd) {
        if (D) Frequency[int.Parse(nodeToAdd.Value.ToString())]++;
        Count++;

        nodeToAdd.Previous = front.Previous;
        nodeToAdd.Next = front;
        front.Previous.Next = nodeToAdd;
        front.Previous = nodeToAdd;
    }

    public void Remove(Node iterator, sbyte index) {
        sbyte i = 0;
        Node n = iterator;
        
        if (index < 0) {
            bool reachedFirst = false;
            while (i != index) {
                n = n.Previous;
                i--;
                if (reachedFirst) return;
                if (n == tail) reachedFirst = true;
            }
            if (n == tail) return;
        }
        else {
            bool reachedLast = false;
            while (i != index) {
                n = n.Next;
                i++;
                if (reachedLast) return;
                if (n == front) reachedLast = true;
            }

            if (n == front) return;
        }
        n.Previous.Next = n.Next;
        n.Next.Previous = n.Previous;
        Count--;
        if (D) Frequency[int.Parse(n.Value.ToString())]--;
        if (P) Console.WriteLine("verwijderd " + n.Value);
    }
    
    void Execute(Node iterator) {
        switch (iterator.Operation) {
            case 'A':
                Node nodeToAdd = new Node() {
                    Value = iterator.y,
                };
                Add(nodeToAdd, iterator, iterator.x);
                break;
            case 'R':
                Remove(iterator, iterator.x);
                break;
        }
    }
    
    public void ExecuteAll() {
        int i = 0;
        Node iterator = tail.Next;
        while (i <= Count) {
            Execute(iterator); // doet niks als operation=null
            iterator = iterator.Next;
            i++;
        }
    }
    
}
