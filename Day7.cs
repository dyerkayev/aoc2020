using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace aoc2020
{
    class Day7
    {
        Dictionary<string, List<string>> rules;

        public Day7()
        {
            rules = new Dictionary<string, List<string>>();
            foreach (string line in File.ReadAllLines("day7.txt"))
            {
                string[] parts = line.Split(" bags contain ");
                string containerBag = parts[0];
                List<string> listOfBags = new List<string>();
                if (!parts[1].StartsWith("no other bags"))
                {
                    string[] bagStrings = parts[1].Split(", ");
                    foreach(string bagString in bagStrings)
                    {
                        string[] bagAndAmount = bagString.Split(' ');
                        int amount = Int32.Parse(bagAndAmount[0]);
                        string bagName = "";
                        for(int i = 1; i < bagAndAmount.Length; i++)
                        {
                            if (bagAndAmount[i].StartsWith("bag"))
                                break;
                            if (i > 1) bagName += " ";
                            bagName += bagAndAmount[i];
                        }

                        for (int i = 0; i < amount; i++)
                        {
                            listOfBags.Add(bagName);
                        }
                    }
                }
                rules.Add(containerBag, listOfBags);
            }
        }

        public void Part1()
        {
            string bagName = "shiny gold";
            //go through all the bags
            HashSet<string> bagSet = new HashSet<string>();
            bagSet.Add(bagName);
            int addedCount = 1;
            while (addedCount != 0)
            {
                addedCount = 0;
                foreach (var keyValue in rules)
                {
                    if (!bagSet.Contains(keyValue.Key))
                    {
                        foreach (string bag in keyValue.Value)
                        {
                            if (bagSet.Contains(bag))
                            {
                                bagSet.Add(keyValue.Key);
                                addedCount++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Day7 part1: {0}", bagSet.Count - 1);
        }

        private int CountBagsInBag(string bag, int bagCount)
        {
            if (rules[bag].Count == 0)
                return bagCount;

            foreach(string subBag in rules[bag])
            {
                return 1;
            }
            return 0;
        }
        public void Part2()
        {
            string bagName = "shiny gold";

            int count = CountBagsInBag(bagName, 1);

            

            Console.WriteLine("Day7 part2 {0}", count - 1);
        }

    }
}
