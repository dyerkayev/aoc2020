using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace aoc2020
{
    class Day6
    {
        private List<List<string>> answersPerGroup;
      
        public Day6()
        {
            answersPerGroup = new List<List<string>>();
            List<string> currentGroup = new List<string>();
            foreach (string line in File.ReadAllLines("day6.txt"))
            {
                if (line.Length == 0)
                {
                    answersPerGroup.Add(currentGroup);
                    currentGroup = new List<string>();
                }
                else
                {
                    currentGroup.Add(line);
                }
            }

            answersPerGroup.Add(currentGroup);
        }
        public void Part1()
        {
            int totalCount = 0;
            foreach (var group in answersPerGroup)
            {
                int groupCount = 0;
                int markedLetters = 0;
                foreach (string answer in group)
                {
                    foreach (char c in answer)
                    {
                        int mask = 1 << (int)(c - 'a');
                        if ((markedLetters & mask) == 0)
                        {
                            markedLetters |= mask;
                            groupCount++;
                        }
                    }
                }

                totalCount += groupCount;
            }

            Console.WriteLine("Day5 part1: {0}", totalCount);
        }

        public void Part2()
        {
            int totalCount = 0;
            int[] answerCounts = new int[26] ;
            Array.Clear(answerCounts,0,26);
            foreach (var group in answersPerGroup)
            {
                int groupCount = 0;

                foreach (string answer in group)
                {
                    foreach (char c in answer)
                    {
                        int index = (int)(c - 'a');
                        answerCounts[index]++;
                    }
                }

                foreach (int numAnswers in answerCounts)
                {
                    if (numAnswers == group.Count)
                        groupCount++;
                }
                Array.Clear(answerCounts, 0, 26);


                totalCount += groupCount;
            }

            Console.WriteLine("Day5 part2: {0}", totalCount);
        }
    
    }
}
