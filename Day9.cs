using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

namespace aoc2020
{
    class Day9
    {
        BigInteger[] numbers;
        public Day9()
        {
            numbers = Array.ConvertAll(File.ReadAllLines("day9.txt"), BigInteger.Parse);
        }

        private bool CheckNumberAtPos(int pos, int preambleLength)
        {
            for (int i = pos - preambleLength; i < pos; i++)
            {
                for (int j = pos - preambleLength; j < pos; j++)
                {
                    if (numbers[pos] == numbers[i] + numbers[j])
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        private (int, int, bool) ContiguousRangeSumEqual(int pos, BigInteger number)
        {
            for (int i = pos; i < numbers.Length; i++)
            {
                BigInteger sum = 0;
                for (int j = pos; j < numbers.Length; j++)
                {
                    sum += numbers[j];
                    if (sum == number)
                        return (pos, j, true);

                    if (sum > number)
                        break;
                }
            }
            return (0, 0, false);
        }

        public void Part1()
        {
            int preambleLength = 25;
            BigInteger foundNumber = 0;
            for (int pos = preambleLength; pos < numbers.Length; pos++)
            {
                if (!CheckNumberAtPos(pos, preambleLength))
                {
                    Console.WriteLine($"Day9 Part1: {numbers[pos]}");
                    break;
                }
            }
        }

        public void Part2()
        {
            int preambleLength = 25;
            BigInteger foundNumber = 0;
            for (int pos = preambleLength; pos < numbers.Length; pos++)
            {
                if (!CheckNumberAtPos(pos, preambleLength))
                {
                    foundNumber = numbers[pos];
                    break;
                }
            }

            for (int pos = 0; pos < numbers.Length; pos++)
            {
                (int start, int end, bool result) t = ContiguousRangeSumEqual(pos, foundNumber);
                if (t.result)
                {
                    BigInteger min = 1000000000000000;
                    BigInteger max = 0;
                    for (int i = t.start; i < t.end +1; i++)
                    {
                        min = BigInteger.Min(min, numbers[i]);
                        max = BigInteger.Max(max, numbers[i]);
                    }
                    Console.WriteLine($"Day9 Part2: {min + max}");
                    break;
                }
            }

        }
    }
}
