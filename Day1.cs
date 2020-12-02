using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace aoc2020
{
    class Day1
    {
        int[] numbers;
        public Day1()
        {
            numbers = Array.ConvertAll(File.ReadAllLines("day1_1.txt"), int.Parse);
        }
        public void Part1()
        {
            int result = 0;
            for (int i = 0; i < numbers.Length; ++i) 
            {
                int firstNumber = numbers[i];
                for (int j = 0; j < numbers.Length; ++j)
                {
                    int secondNumber = numbers[j];
                    if (firstNumber + secondNumber == 2020)
                        result = firstNumber * secondNumber;
                }
            }
            Console.WriteLine("Day1 Result 1: {0}", result);
        }

        public void Part2()
        {
            int result = 0;
            for (int i = 0; i < numbers.Length; ++i)
            {
                int firstNumber = numbers[i];
                for (int j = 0; j < numbers.Length; ++j)
                {
                    int secondNumber = numbers[j];
                    for (int k = 0; k < numbers.Length; ++k)
                    {
                        int thirdNumber = numbers[k];
                        if (firstNumber + secondNumber + thirdNumber == 2020)
                        {
                            result = firstNumber * secondNumber * thirdNumber;
                            break;
                        }
                    }
                    if (result > 0)
                        break;
                }

                if (result > 0)
                    break;

            }

            Console.WriteLine("Day1 Result 2: {0}", result);
        }
    }
}
