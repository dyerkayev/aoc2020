using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Numerics;

namespace aoc2020
{
    class Day16
    {
        Dictionary<string, (int, int, int, int)> valueRanges;
        int[] myTicket;
        List<int[]> tickets;
        public Day16()
        {
            valueRanges = new Dictionary<string, (int, int, int, int)>();
            tickets = new List<int[]>();

            int state = 0; //0 - ranges, 1, your ticket, 2 - nearby tickets
            foreach(string line in File.ReadAllLines("day16.txt"))
            {
                if (line.Length == 0)
                {
                    continue;
                }

                if (state == 0)
                {
                    if (line.StartsWith("your ticket:"))
                    {
                        state = 1;
                        continue;
                    }

                    int s1, e1, s2, e2;
                    string[] parts = line.Split(": ");
                    string[] rangeStrings = parts[1].Split(" or ");

                    {
                        string[] rangeParts = rangeStrings[0].Split('-');
                        s1 = int.Parse(rangeParts[0]);
                        e1 = int.Parse(rangeParts[1]);
                    }
                    {
                        string[] rangeParts = rangeStrings[1].Split('-');
                        s2 = int.Parse(rangeParts[0]);
                        e2 = int.Parse(rangeParts[1]);
                    }

                    valueRanges[parts[0]] = (s1, e1, s2, e2);
                }
                else if (state == 1)
                {
                    if (line.StartsWith("nearby tickets:"))
                    {
                        state = 2;
                        continue;
                    }

                    myTicket = Array.ConvertAll(line.Split(','), x => int.Parse(x));
                }
                else if (state == 2)
                {
                    tickets.Add(Array.ConvertAll(line.Split(','), x => int.Parse(x)));
                }
            }
        }

        public void Part1()
        {
            List<(int, int)> allRanges = new List<(int, int)>();

            foreach ((int s1, int e1, int s2, int e2) in valueRanges.Values)
            {
                allRanges.Add((s1, e1));
                allRanges.Add((s2, e2));

            }

            //allRanges.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            int sumInvalidValues = 0;

            foreach(int[] ticket in tickets)
            {
                foreach (int n in ticket)
                {
                    bool foundRange = false;
                    foreach ((int s, int e) in allRanges)
                    {
                        if (n >= s && n <= e)
                        {
                            foundRange = true;
                            break;
                        }
                    }
                    if (!foundRange)
                        sumInvalidValues += n;
                }
            }

            Console.WriteLine($"Day16 Part1 {sumInvalidValues}");
        }

        public void Part2()
        {
            var fieldNames = new Dictionary<int, (HashSet<string>, HashSet<string>)>();
            for (int i = 0; i < valueRanges.Keys.Count; i++)
            {
                fieldNames[i] = (new HashSet<string>(), new HashSet<string>());
            }

            tickets.Add(myTicket);
            

            foreach (int[] ticket in tickets)
            {
                for(int f = 0; f < ticket.Length; f++)
                {
                    HashSet<string> addSet = new HashSet<string>();
                    HashSet<string> removeSet = new HashSet<string>();

                    //for each matching range
                    foreach (var item in valueRanges)
                    {
                        (int s1, int e1, int s2, int e2) = item.Value;
                        int n = ticket[f];
                        if ((n >= s1 && n <= e1) || (n >= s2 && n <= e2))
                            addSet.Add(item.Key);
                        else
                            removeSet.Add(item.Key);
                    }

                    if (addSet.Count > 0)
                    {
                        fieldNames[f].Item1.UnionWith(addSet);
                        fieldNames[f].Item2.UnionWith(removeSet);
                    }
                }
            }

            bool moreThanTwoFields = true;
            Dictionary<string, int> foundFields = new Dictionary<string, int>();
            while (moreThanTwoFields)
            {
                moreThanTwoFields = false;
                for (int i = 0; i < fieldNames.Count; i++)
                {
                    (var addSet, var removeSet) = fieldNames[i];
                    
                    addSet.ExceptWith(removeSet);
                    addSet.ExceptWith(foundFields.Keys);
                    
                    if (addSet.Count > 1)
                        moreThanTwoFields = true;
                    else if (addSet.Count == 1)
                        foreach (string k in addSet)
                            foundFields[k] = i;
                }
            }

            BigInteger result = 1;
            foreach(var field in foundFields)
            {
                int fieldIndex = field.Value;
                string fieldName = field.Key;
                if (fieldName.StartsWith("departure"))
                    result *= myTicket[fieldIndex];
            }    


            Console.WriteLine($"Day16 Part2 {result}");
        }
    }
}
