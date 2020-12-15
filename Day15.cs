using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace aoc2020
{
    class Day15
    {
        Dictionary<int, (int, int)> numbers;
        int lastNumber;
        public Day15()
        {
            numbers = new Dictionary<int, (int, int)>();
            int turn = 1;
            foreach ( string s in File.ReadAllLines("day15.txt")[0].Split(','))
            {
                lastNumber = int.Parse(s);
                numbers[lastNumber] = (turn, turn);
                turn++;
            }
        }
        public void Part1()
        {
            int count = 30000000;
            for (int i = numbers.Count + 1; i <= count; i++)
            {
                (int turn1, int turn2) = numbers[lastNumber];
                int diff = turn2 - turn1;

                if (numbers.ContainsKey(diff))
                {
                    (int t1, int t2) = numbers[diff];
                    numbers[diff] = (t2, i);
                }
                else 
                {
                    numbers[diff] = (i, i);
                }

                lastNumber = diff;                
            }
            Console.WriteLine($"{lastNumber}");
        }
    }
}
