using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace aoc2020
{
    class Day4
    {
        private List<Dictionary<string,string>> passports;
        public Day4()
        {
            passports = new List<Dictionary<string, string>>();
            Dictionary<string, string> currentPassport = new Dictionary<string, string>();

            foreach(string line in File.ReadAllLines("day4.txt"))
            {
                if (line.Length == 0)
                {
                    passports.Add(currentPassport);
                    currentPassport = new Dictionary<string, string>();
                }
                else
                {
                    string[] keyValues = line.Split(' ');
                    foreach (string keyVal in keyValues)
                    {
                        string[] keyValue = keyVal.Split(':');
                        if (keyValue[0].Length > 0)
                            currentPassport.Add(keyValue[0], keyValue[1]);
                    }
                }                 
            }
            passports.Add(currentPassport);
        }


        public void Part1()
        {
            string[] requiredKeys = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            int validCount = 0;
            foreach(var passport in passports)
            {
                bool hasAllKeys = true;
                foreach (string requiredKey in requiredKeys)
                {
                    if (!passport.ContainsKey(requiredKey))
                        hasAllKeys = false;
                }

                if (hasAllKeys)
                    validCount++;
            }

            Console.WriteLine("Day4 result 1:{0}", validCount);
        }

        public void Part2()
        {
            var validationRules = new Dictionary<string, string>
            {
                {"byr", "(19[2-9][0-9])|(200[0-2])"},
                {"iyr", "(201[0-9])|2020" },
                {"eyr", "(202[0-9])|2030" },
                {"hgt", "(1[5-8][0-9]cm)|(19[0-3]cm)|(59in)|(6[0-9]in)|(7[0-6]in)"},
                {"hcl", "#[0-9a-f]{6}"},
                {"ecl", "amb|blu|brn|gry|grn|hzl|oth"},
                {"pid", "^[0-9]{9}$"}
            };

            //test validation rules
            //string[] testStrings = { "byr:1921", "byr:1915", "byr:2001", "byr:2012", "iyr:2018", "iyr:1920", "iyr:2020", "eyr:2019", "eyr:2020", "eyr:2029", "hgt:142cm", "hgt:181", "hgt:192cm", "hgt:171cm", "hgt:59in", "hgt:600in", "hgt:62in", "hgt:72in", "hgt:77in", "hcl:#a", "hcl:#1d3f5c7", "hcl:1234", "ecl:aaa", "ecl:brn", "pid:32748", "pid:000000000", "pid:0001234567" };
            //bool[] testResulst = { true, false, true, false, true, false, true, false, true, true, false, false, true, true, true, false, true, true, false, false, true, false, false, true, false, true, true };

            //Debug.Assert(testStrings.Length == testResulst.Length);

            //for (int i = 0; i < testStrings.Length; i++)
            //{
            //    string[] keyVal = testStrings[i].Split(':');
            //    bool regexpCheck = Regex.IsMatch(keyVal[1], validationRules[keyVal[0]]);
            //    Debug.Assert(regexpCheck == testResulst[i], "No match");
            //}
            int validCount = 0;
            foreach (var passport in passports)
            {
                bool hasAllKeys = true;
                foreach (string requiredKey in validationRules.Keys)
                {
                    if (passport.ContainsKey(requiredKey))
                    {
                        if (!Regex.IsMatch(passport[requiredKey], validationRules[requiredKey]))
                        {
                            hasAllKeys = false;
                            break;
                        }
                    }
                    else
                    {
                        hasAllKeys = false;
                        break;
                    }
                }

                if (hasAllKeys)
                {
                    validCount++;
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", passport["byr"], passport["iyr"], passport["eyr"], passport["hgt"], passport["hcl"], passport["ecl"], passport["pid"]);
                }
            }

            Console.WriteLine("Day4 result 2:{0}", validCount);

        }
    }
}
