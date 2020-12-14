using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

namespace aoc2020
{
    class Day14
    {
        public static ulong Base2StrToNumber(char[] str)
        {
            return Convert.ToUInt64(new string(str), 2);
        }

        public static char[] ToBase2Str(ulong number)
        {
            return Convert.ToString((long)number, 2).PadLeft(36, '0').ToCharArray();
        }

        string[] lines;
        public Day14()
        {
            lines = File.ReadAllLines("day14.txt");
        }
        public void Part1()
        {
            string mask = "";
            Dictionary<int, char[]> memLocationAndVal = new Dictionary<int, char[]>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(" = ");
                if (parts[0].StartsWith("mask"))
                {
                    mask = parts[1];
                }
                else
                {
                    int location = int.Parse((parts[0].Replace(']', ' ').Split('['))[1]);
                    char[] val = ToBase2Str(ulong.Parse(parts[1]));
                    //apply mask
                    for (int i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] == '1' || mask[i] == '0')
                            val[i] = mask[i];
                    }

                    memLocationAndVal[location] = val;
                }
            }

            ulong sum = 0;
            foreach (char[] numStr in memLocationAndVal.Values)
            {
                ulong number = Base2StrToNumber(numStr);
                sum += number;

            }

            Console.WriteLine($"Day14 Part1 {sum}");
        }

        private void ExpandWildcard(ref Dictionary<ulong, ulong> memory, char[] wildcard, ulong val)
        {
            bool foundX = false;
            for (int i = 0; i < wildcard.Length; i++)
            {
                if (wildcard[i] == 'X')
                {
                    char[] adress1 = new char[wildcard.Length];
                    wildcard.CopyTo(adress1, 0);
                    adress1[i] = '1';
                    ExpandWildcard(ref memory, adress1, val);
                    char[] adress0 = new char[wildcard.Length];
                    wildcard.CopyTo(adress0, 0);
                    adress0[i] = '0';
                    ExpandWildcard(ref memory, adress0, val);
                    foundX = true;
                    break;                
                }
            }

            if (!foundX)
                memory[Base2StrToNumber(wildcard)] = val;
        }

        //brute force
        public void Part2()
        {
            Dictionary<ulong, ulong> memory = new Dictionary<ulong, ulong>();
            string mask = "";
            foreach (string line in lines)
            {
                string[] parts = line.Split(" = ");
                if (parts[0].StartsWith("mask"))
                {
                    mask = parts[1];
                }
                else
                {
                    ulong location = ulong.Parse((parts[0].Replace(']', ' ').Split('['))[1]);
                    char[] addressVal = ToBase2Str(location);
                    //apply mask
                    for (int i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] == '1' || mask[i] == 'X')
                            addressVal[i] = mask[i];
                    }

                    ExpandWildcard(ref memory, addressVal, ulong.Parse(parts[1]));
                }
            }

            BigInteger sum = 0;
            foreach (var entry in memory)
            {
                sum += entry.Value;
            }

            Console.WriteLine($"Day14 Part2  {sum}");

        }      
    }
}
