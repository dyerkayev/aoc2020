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
            match = (char)0;
            isRecursive = false;
            limitLoops = false;
        }
        
        public Rule(char matchChar)
        {
            match = matchChar;
            ruleIndices1 = null;
            ruleIndices2 = null;
            isRecursive = false;
            limitLoops = false;
        }

        public int IsMatch(char c)
        {
            if (match != (char)0)
                if (match == c)
                    return 1;
                else
                    return -1;

            return 0;
        }
        public int[] ruleIndices1;
        public int[] ruleIndices2;
        public char match;
        public bool isRecursive;
        public bool limitLoops;
    }
    class Day19
    {
        List<string> ruleLines;
        List<string> messages;
        Dictionary<int, Rule> rules;
        int loopLimitPart2;
        public Day19()
        {
            string[] allLines = File.ReadAllLines("day19.txt");
            ruleLines = new List<string>();
            messages = new List<string>();
            rules = new Dictionary<int, Rule>();
            loopLimitPart2 = 0;
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

        public bool CheckRules( int[] rules, string str, ref int idx)
        {
            if (rules is null)
                return false;
            int index = idx;
            foreach (int r in rules)
            {
                if (index > str.Length - 1)
                    return false;

                if (!CheckMatch(r, str, ref index))
                    return false;
            }
            
            idx = index;
            return true;
        }

        public bool CheckRules2(int[] rules, string str, ref int idx)
        {
            if (rules is null)
                return false;
            int index = idx;
            foreach (int r in rules)
            {
                if (index > str.Length - 1)
                    return false;

                if (!CheckMatch2(r, str, ref index))
                    return false;
            }

            idx = index;
            return true;
        }

        private bool CheckMatch(int ruleIndex, string str, ref int idx)
        {
            Rule rule = rules[ruleIndex];
            int match = rule.IsMatch(str[idx]);

            if (match == 1)
            {
                idx++;
                return true;
            }
            else if (match == -1)
                return false;
            else
            {                
                return CheckRules(rule.ruleIndices1, str, ref idx)
                    || CheckRules(rule.ruleIndices2, str, ref idx);
            }
        }

        private bool CheckMatch2(int ruleIndex, string str, ref int idx)
        {
            Rule rule = rules[ruleIndex];
            int match = rule.IsMatch(str[idx]);

            if (match == 1)
            {
                idx++;
                return true;
            }
            else if (match == -1)
                return false;
            else
            {
                if (rule.isRecursive)
                {
                    int index1 = idx;
                    bool check1 = CheckRules2(rule.ruleIndices1, str, ref index1);

                    int index2 = idx;
                    bool check2 = true;
                    if (rule.limitLoops && loopLimitPart2 > 0)
                    {
                        loopLimitPart2--;
                        check2 = CheckRules2(rule.ruleIndices2, str, ref index2);
                    }                

                    if (check1 && check2)
                    {
                        idx = Math.Max(index1, index2);
                        return true;
                    }

                    if (check1)
                        idx = index1;
                    else if (check2)
                        idx = index2;

                    return check1 || check2;
                }
                else
                    return CheckRules2(rule.ruleIndices1, str, ref idx)
                        || CheckRules2(rule.ruleIndices2, str, ref idx);
            }
        }

        public void Part1()
        {
            int ruleIndex = 0;
            int numMatches = 0;
            foreach (string str in messages)
            {
                int indexToCheck = 0;
                bool fullMatch = CheckMatch(ruleIndex, str, ref indexToCheck);

                if (fullMatch && indexToCheck == str.Length)
                {
                    Console.WriteLine($"Full match: {str}");
                    numMatches++;
                }
            }

            Console.WriteLine($"Day 19 Part1: {numMatches}");
        }

        public void Part2()
        {
            //replace rules
            Rule rule8 = new Rule("42 | 42 8");
            rule8.isRecursive = true;
            rule8.limitLoops = true;
            rules[8] = rule8;
            
            Rule rule11 = new Rule("42 31 | 42 11 31");
            rules[11] = rule11;
            rule11.isRecursive = true;

            int ruleIndex = 0;
            int numMatches = 0;
            foreach (string str in messages)
            {
                for (int loopLimit = 0; loopLimit < 128; loopLimit++)
                {
                    loopLimitPart2 = loopLimit;
                    int indexToCheck = 0;
                    bool fullMatch = CheckMatch2(ruleIndex, str, ref indexToCheck);

                    if (fullMatch && indexToCheck == str.Length)
                    {
                        Console.WriteLine($"Full match: {str}");
                        numMatches++;
                        break;
                    }
                }
            }

            Console.WriteLine($"Day 19 Part2: {numMatches}");


            //Rule rule101 = new Rule("42 42 31 | 42 42 11 31");
            //Rule rule102 = new Rule("42 8 42 31 | 42 8 42 11 31");
            //rules[101] = rule101;
            //rules[102] = rule102;
            //Rule rule0 = new Rule("101 | 102");
            ////rule0.isRecursive = true;

            //rules[0] = rule0;
        }
    }
}
