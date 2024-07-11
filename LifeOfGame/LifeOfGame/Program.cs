using System;
using System.Diagnostics;
using System.Collections.Generic;

class Program
{
    static void GameOfLife(HashSet<Tuple<int, int>> livingcells, int generations, Tuple<int, int> coords) {
        for (int i = 0; i < generations; i++) {
            HashSet<Tuple<int, int>> newLivingCells = new HashSet<Tuple<int, int>>();
            foreach (Tuple<int, int> cell in livingcells) {
                int x = cell.Item1;
                int y = cell.Item2;
                int count = 0;
                count = CountLivingNeighbours(livingcells, x, y);
                if (count == 2 || count == 3) {
                    newLivingCells.Add(cell);
                }
                
                // check the neighbours of every cell
                for (int dx = -1; dx <= 1; dx++) {
                    for (int dy = -1; dy <= 1; dy++) {
                        if (dx == 0 && dy == 0) continue;
                        Tuple<int, int> newCell = new Tuple<int, int>(x + dx, y + dy);
                        if (newLivingCells.Contains(newCell) || livingcells.Contains(newCell)) continue; //optimization
                        count = CountLivingNeighbours(livingcells, x + dx, y + dy);
                        if (count == 3) { // if the amount of neighbours is 3, the cell will be alive next gen
                            newLivingCells.Add(newCell);
                        }
                    }
                }
            }
            livingcells = newLivingCells;
        }
        draw(livingcells, coords);
    }

    private static int CountLivingNeighbours(HashSet<Tuple<int, int>> livingcells, int x, int y) {
        int result = 0;
        for (int dx = -1; dx <= 1; dx++) { // count the living neighbours
            for (int dy = -1; dy <= 1; dy++) {
                if (dx == 0 && dy == 0) continue;
                if (livingcells.Contains(new Tuple<int, int>(x + dx, y + dy))) {
                    result++;
                }
            }
        }
        return result;
    }

    static void draw(HashSet<Tuple<int,int>> livingcells, Tuple<int, int> coords) {
        Console.WriteLine(livingcells.Count);
        for (int dy = 2; dy >= -2; dy--) {
            string row = "";
            for (int dx = -2; dx <= 2; dx++) {
                Tuple<int, int> cell = new Tuple<int, int>(coords.Item1 + dx, coords.Item2 + dy);
                if (livingcells.Contains(cell)) {
                    row += "0";
                }
                else {
                    row += ".";
                }
            }
            Console.WriteLine(row);
        }
    }
    
    static void Main(string[] args) {
        string[] firstline = Console.ReadLine().Split(' ');
        int r = int.Parse(firstline[0]); // amount of lines
        int t = int.Parse(firstline[1]); // amount of generations
        Tuple<int, int> coords = new Tuple<int, int>(int.Parse(firstline[3]), int.Parse(firstline[4])); // the coordinates to check
        
        HashSet<Tuple<int,int>> livingcells = new HashSet<Tuple<int,int>>();
        for (int i = 0; i < r; i++) { // read the initial state
            string[] line = Console.ReadLine().Split();
            int y = int.Parse(line[0]);
            for (int j = 1; j < line.Length; j++) {
                if (line[j] == "") continue;
                livingcells.Add(new Tuple<int, int>(int.Parse(line[j]), y));
            }
        }
        GameOfLife(livingcells, t, coords);
    }
}
