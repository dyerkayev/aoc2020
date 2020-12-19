using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace aoc2020
{
    struct Rule
    { 
        public Rule(string ruleStr)
        {
            string[] ruleParts = ruleStr.Split(" | ");
            string[] indices = ruleParts[0].Split(' ');
            ruleIndices1 = Array.ConvertAll(indices, x => int.Parse(x));

            if (ruleParts.Length > 1)
            {
                indices = ruleParts[1].Split(' ');
                ruleIndices2 = Array.ConvertAll(indices, x => int.Parse(x));
            }
            else
                ruleIndices2 = null;
            match1 = (char)0;
        }
        
        public Rule(char matchChar)
        {
            match1 = matchChar;
            ruleIndices1 = null;
            ruleIndices2 = null;
        }

        public int IsMatch(char c)
        {
            if (match1 != (char)0)
                if (match1 == c)
                    return 1;
                else
                    return -1;

            return 0;
        }
        public int[] ruleIndices1;
        public int[] ruleIndices2;
        public char match1;
    }
    class Day19
    {
        List<string> ruleLines;
        List<string> messages;
        Dictionary<int, Rule> rules;
        public Day19()
        {
            string[] allLines = File.ReadAllLines("day19.txt");
            ruleLines = new List<string>();
            messages = new List<string>();
            rules = new Dictionary<int, Rule>();
            bool msgLines = false;
            foreach (string line in allLines)
            {
                if (line.Length > 1)
                {
                    if (msgLines)
                    {
                        messages.Add(line);
                    }
                    else
                    {
                        ParseAndAddRule(line);
                    }
                }
                else
                    msgLines = true;
            }
        }

        public void ParseAndAddRule(string line)
        {
            string[] numAndRule = line.Split(": ");
            int num = int.Parse(numAndRule[0]);

            if (numAndRule[1].StartsWith('\"'))
            {
                Rule rule = new Rule(numAndRule[1].Trim('\"')[0]);
                rules[num] = rule;
            }
            else 
            {
                Rule rule = new Rule(numAndRule[1]);
                rules[num] = rule;
            }
        }

        public bool CheckRules( int[] rules, char c, HashSet<int> failingRules)
        {
            if (rules is null)
                return false;

            foreach (int r in rules)
            {
                if (failingRules.Contains(r))
                    return false;

                if (!CheckMatch(r, c, failingRules))
                {
                    failingRules.Add(r);
                    return false;
                }
            }

            return true;
        }

        private bool CheckMatch(int ruleIndex, char c, HashSet<int> failingRules)
        {
            if (failingRules.Contains(ruleIndex))
                return false;

            Rule rule = rules[ruleIndex];
            int match = rule.IsMatch(c);

            if (match == 1)
                return true;
            else if (match == -1)
                return false;
            else
                return CheckRules(rule.ruleIndices1, c, failingRules)
                    || CheckRules(rule.ruleIndices2, c, failingRules);
        }

        public void Part1()
        {
            HashSet<int> rulesFailingA = new HashSet<int>();
            HashSet<int> rulesFailingB = new HashSet<int>();
            int ruleIndex = 0;
            int numMatches = 0;
            foreach (string str in messages)
            {
                bool fullMatch = true;
                foreach(char c in str)
                {
                    if (!CheckMatch(ruleIndex, c, c == 'a' ? rulesFailingA : rulesFailingB))
                    {
                        fullMatch = false;
                        break;
                    }
                }

                if (fullMatch)
                {
                    Console.WriteLine($"Full match: {str}");
                    numMatches++;
                }
            }

            Console.WriteLine($"Day 19 Part1: {numMatches}");
        }
    }
}
