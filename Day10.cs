using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

namespace aoc2020
{
    class Day10
    {
        int[] numbers;
        public Day10()
        {
            numbers = Array.ConvertAll(File.ReadAllLines("day10.txt"), Int32.Parse);
            Array.Sort(numbers);
        }

        public void Part1()
        {
            int chargerVoltage = numbers[numbers.Length - 1] + 3;
            int adapterVoltage = 0;
            int[] differences = new int[] { 0, 0, 0 };
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int inc = 1; inc < 4; inc++)
                {
                    int index = Array.IndexOf(numbers, adapterVoltage + inc, i);
                    if (index != -1)
                    {
                        adapterVoltage += inc;
                        differences[inc - 1]++;
                        break;
                    }
                }
            }

            differences[2]++;
            Console.WriteLine($"Day10 Part1 {differences[0] * differences[2]}");
        }

        private static void CountVariants(int index, int[] numbers, int[] numVariants)
        {
            int number = numbers[index];
            int countVariants = 0;

            for (int i = index + 1; i < numbers.Length; i++)
            {
                int diff = numbers[i] - number;
                if (diff > 3)
                    break;

                countVariants++;
            }
            numVariants[index] = countVariants;
        }

        private static BigInteger CountArrangements(int[] numVariants, int index, BigInteger currentCount)
        {
            BigInteger result = currentCount;
            if (index != numVariants.Length)
            {
                int variantcount = numVariants[index];
                for (int i = index + 1; i <= index + variantcount; i++)
                {
                    result += CountArrangements(numVariants, i, currentCount + (i - index - 1));
                }
            }

            return result;
            
        }
        public void Part2()
        {
            int len = numbers.Length + 2;
            int[] newNumbers = new int[len];
            newNumbers[0] = 0;
            for (int i = 0; i < numbers.Length; i++)
                newNumbers[i + 1] = numbers[i];
            newNumbers[len-1] = numbers[numbers.Length - 1] + 3;

            BigInteger[] numBranches = new BigInteger[len];

            numBranches[0] = 1;
            
            for (int i = 1; i < len; i++)
            {
                numBranches[i] = numBranches[i - 1];
                if (i > 1 && (newNumbers[i] - newNumbers[i - 2]) <= 3)
                    numBranches[i] += numBranches[i - 2];
                if (i > 2 && (newNumbers[i] - newNumbers[i - 3]) <= 3)
                    numBranches[i] += numBranches[i - 3] ;

            }

            
            Console.WriteLine($"Day10 Part2 {numBranches[len-1]}");

        }
    }
}
