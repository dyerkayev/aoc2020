using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace aoc2020
{
    struct PasswordRule
    {
        public int min;
        public int max;
        public char letter;
    }
    class Day2
    {
        private string[] lines;
        private string pattern;
        public Day2()
        {
            lines = File.ReadAllLines("day2.txt");
            pattern = @"(\d+)-(\d+)\s([a-z]):\s([a-z]+)";
        }
        
        private (PasswordRule rule, string password) ParseLine(string line)
        {
            Match match = Regex.Match(line, pattern);
            PasswordRule rule = new PasswordRule();
            rule.min = int.Parse(match.Groups[1].Value);
            rule.max = int.Parse(match.Groups[2].Value);
            rule.letter = (match.Groups[3].Value)[0];

            return (rule, match.Groups[4].Value);
        }
        private bool IsValidPasswordLine1(string line)
        {
            (PasswordRule rule, string password) = ParseLine(line);
            int count = 0;
            foreach (char c in password)
            {
                if (c == rule.letter)
                    count++;
            }

            return count >= rule.min && count <= rule.max;
        }

        private bool IsValidPasswordLine2(string line)
        {
            (PasswordRule rule, string password) = ParseLine(line);

            int count = 0;
            if (password[rule.min - 1] == rule.letter)
                count++;

            if (password[rule.max - 1] == rule.letter)
                count++;


            return count == 1;
        }

        public void Part1()
        {
            int count = 0;

            foreach (string line in lines)
            {
                if (IsValidPasswordLine1(line))
                    count++;
            }

            Console.WriteLine("Day2 result 1: {0}", count);
        }

        public void Part2()
        {
            int count = 0;

            foreach (string line in lines)
            {
                if (IsValidPasswordLine2(line))
                    count++;
            }

            Console.WriteLine("Day2 result 2: {0}", count);
        }

        public void Part2_Test()
        {
            string[] lines = new string[] { "1-3 a: abcde", "1-3 b: cdefg", "2-9 c: ccccccccc" };
            bool[] goodLines = new bool[] { true, false, false };

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                bool isValid = IsValidPasswordLine2(line);
                Debug.Assert(isValid == goodLines[i], "Mismatch");
            }


        }

    }
}
