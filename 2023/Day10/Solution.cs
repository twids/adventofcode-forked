using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;

namespace AdventOfCode.Y2023.Day10;

[ProblemName("Pipe Maze")]
class Solution : Solver {

    public object PartOne(string input) {

        Map map = new Map(input);
        (int y, int x) curr = Map.Next(map.start, Dir.Left); // cheat... :(
        (int y, int x) previous = map.start;
        var steps = 1;

        do {
            var temp = curr;
            curr = Map.MoveNext(previous, curr, map.map[curr.y, curr.x]);
            previous = temp;
            steps++;
        } while (map.map[curr.y, curr.x] != 'S');

        return steps / 2;
    }

    public object PartTwo(string input) {
        return 0;
    }
}

internal static class Dir {
    public static (int y, int x) Left = new(0, -1);
    public static (int y, int x) Right = new(0, 1);
    public static (int y, int x) Up = new(-1, 0);
    public static (int y, int x) Down = new(1, 0);
}
internal class Map {
    public char[,] map;
    public (int y, int x) start;
    public (int y, int x) current;
    public Map(string input) {
        var row = input.Split("\n");
        map = new char[row.Length, row[0].ToCharArray().Length];
        Console.WriteLine($"{row.Length} {row[0].ToCharArray().Length}");
        for (var y = 0; y < row.Length; y++) {
            var col = row[y].ToCharArray();
            for (var x = 0; x < col.Length; x++) {
                if (col[x] == 'S') { start = new(y, x); }
                map[y, x] = col[x];
            }
        }
    }

    public static (int y, int x) MoveNext((int y, int x) old, (int y, int x) curr, char symbol) {
        //Console.WriteLine($"{string.Join(" ", Directions[symbol].Where(d => !(d.y == old.y - curr.y && d.x == old.x - curr.x)).ToList())} | x: {old.x - curr.x} y: {old.y - curr.y}");
        var (movementy, movementx) = Directions[symbol].Where(d => !(d.y == old.y - curr.y && d.x == old.x - curr.x)).Single();
        return new(curr.y + movementy, curr.x + movementx);
    }
    public static (int y, int x) Next((int y, int x) position, (int y, int x) dir) => new(position.y + dir.y, position.x + dir.x);
    public static Dictionary<char, (int y, int x)[]> Directions = new(){
        {'7', [Dir.Left, Dir.Down]},
        {'F', [Dir.Right, Dir.Down ]},
        {'L', [Dir.Up, Dir.Right ]},
        {'J', [Dir.Up, Dir.Left ]},
        {'|', [Dir.Up, Dir.Down ]},
        {'-', [Dir.Left, Dir.Right ]},
        {'S', [Dir.Right, Dir.Down, Dir.Up, Dir.Down ]}
    };

}