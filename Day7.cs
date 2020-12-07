using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace aoc2020
{
    class Day7
    {
        Dictionary<string, List<(string, int)>> rules;

        public Day7()
        {
            rules = new Dictionary<string, List<(string, int)>>();
            foreach (string line in File.ReadAllLines("day7.txt"))
            {
                string[] parts = line.Split(" bags contain ");
                string containerBag = parts[0];
                List<(string, int)> listOfBags = new List<(string, int)>();
                if (!parts[1].StartsWith("no other bags"))
                {
                    string[] bagStrings = parts[1].Split(", ");
                    foreach(string bagString in bagStrings)
                    {
                        string[] bagAndCount = bagString.Split(' ');
                        int bagCount = Int32.Parse(bagAndCount[0]);
                        string bagName = "";
                        for(int i = 1; i < bagAndCount.Length; i++)
                        {
                            if (bagAndCount[i].StartsWith("bag"))
                                break;
                            if (i > 1) bagName += " ";
                            bagName += bagAndCount[i];
                        }

                        listOfBags.Add((bagName, bagCount));
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
                        foreach ((string name, int count) bag in keyValue.Value)
                        {
                            if (bagSet.Contains(bag.name))
                            {
                                bagSet.Add(keyValue.Key);
                                addedCount += bag.count;
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Day7 part1: {bagSet.Count - 1}");
        }

        private int CountBagsInBag((string name, int count) b)
        {
            if (rules[b.name].Count == 0)
                return b.count;

            int bagsCount = 0;
            foreach((string name, int count) subBag in rules[b.name])
            {
                bagsCount += CountBagsInBag(subBag);            
            }
            return bagsCount * b.count + b.count;
        }

        public void Part2()
        {
            string bagName = "shiny gold";

            int count = CountBagsInBag(("shiny gold", 1));            

            Console.WriteLine($"Day7 part2 {count - 1}");
        }

    }
}
