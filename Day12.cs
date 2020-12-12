using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace aoc2020
{
    struct MoveInstruction
    {
        public char move;
        public int val;
    }
    class Day12
    {
        MoveInstruction[] instructions;
        char[] directions;
        (int, int)[] steps;

        public Day12()
        {
            instructions = Array.ConvertAll(File.ReadAllLines("day12.txt"), s => new MoveInstruction { move = s[0], val = int.Parse(s.Substring(1)) }) ;
            directions = new char[] {'E', 'S', 'W', 'N' };
            steps = new (int, int)[] { (1, 0), (0, -1), (-1, 0), (0, 1) };
        }
                
        static int DirectionFromDegrees(int degrees, int currDir)
        {
            return (currDir + degrees / 90) % 4;
        }

        static int ManhattanDist(int x, int y)
        {
            return Math.Abs(x) + Math.Abs(y);
        }
        public void Part1()
        {
            int dirIndex = 0;
            int x = 0;
            int y = 0;

            foreach (MoveInstruction instruction in instructions)
            {
                switch(instruction.move)
                {
                    case 'N':
                        y += instruction.val;
                        break;
                    case 'S':
                        y -= instruction.val;
                        break;
                    case 'E':
                        x += instruction.val;
                        break;
                    case 'W':
                        x -= instruction.val;
                        break;
                    case 'L':
                        dirIndex = DirectionFromDegrees(360-instruction.val, dirIndex);
                        break;
                    case 'R':
                        dirIndex = DirectionFromDegrees(instruction.val, dirIndex);
                        break;
                    case 'F':
                        (int x, int y) s = steps[dirIndex];
                        x += s.x * instruction.val;
                        y += s.y * instruction.val;
                        break;
                    default:
                        Debug.Assert(false, $"Unknown move {instruction.move}");
                        break;
                }
            }

            Console.WriteLine($"Day12 Part1 {ManhattanDist(x, y)}");
        }

        static (int, int) Rotate(int x, int y, int degrees)
        {
            switch((degrees / 90) % 4)
            {
                case 1:
                    return (y, -x);
                case 2:
                    return (-x, -y);
                case 3:
                    return (-y, x);
            }

            return (x, y);
        }

        public void Part2()
        {
            int wx = 10;
            int wy = 1;
            int sx = 0;
            int sy = 0;

            for (int instrIndex = 0; instrIndex < instructions.Length; instrIndex++)
            {
                MoveInstruction instruction = instructions[instrIndex];
            
                switch (instruction.move)
                {
                    case 'N':
                        wy += instruction.val;
                        break;
                    case 'S':
                        wy -= instruction.val;
                        break;
                    case 'E':
                        wx += instruction.val;
                        break;
                    case 'W':
                        wx -= instruction.val;
                        break;
                    case 'L':
                        (wx, wy) = Rotate(wx, wy, 360-instruction.val);
                        break;
                    case 'R':
                        (wx, wy) = Rotate(wx, wy, instruction.val);
                        break;
                    case 'F':                        
                        sx += wx * instruction.val;
                        sy += wy * instruction.val;
                        break;
                    default:
                        Debug.Assert(false, $"Unknown move {instruction.move}");
                        break;
                }
            }

            Console.WriteLine($"Day12 Part2 {ManhattanDist(sx, sy)}");
        }
    }
}
