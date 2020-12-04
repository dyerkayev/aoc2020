using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

namespace aoc2020
{
    class Day3
    {
        string[] allLines;
        public Day3()
        {
            allLines = File.ReadAllLines("day3.txt");
        }

        private char GetCharAt(string line, int index)
        {
            return line[index % line.Length];
        }
        private int CountTreesWithStep(int right, int down)
        {
            int col = 0;
            int treeCount = 0;
            for (int lineIdx = 0; lineIdx < allLines.Length; lineIdx+=down)
            {
                if (lineIdx < allLines.Length)
                {
                    if (GetCharAt(allLines[lineIdx], col) == '#')
                        treeCount++;
                    col += right;
                }

            }

            return treeCount;
        }
        public void Part1()
        {
            Console.WriteLine("Day3 Part 1 result {0}", CountTreesWithStep(3, 1));
        }

        public void Part2()
        {
            BigInteger result = new BigInteger(1);
            int[] slopes = new int[] {1, 1, 3, 1, 5, 1,  7, 1, 1, 2};
            for(int i = 0; i < slopes.Length; i+=2)
            {
                result *= CountTreesWithStep(slopes[i], slopes[i + 1]);
            }

            Console.WriteLine("Day3 Part 2 result {0}",result);
        }

    }
}
