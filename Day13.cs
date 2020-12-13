using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

namespace aoc2020
{
    class Day13
    {
        int timestamp;
        List<int> busIds;
        List<(int, int)> busIdAndPos;
        public Day13()
        {
            string[] allLines = File.ReadAllLines("day13.txt");
            timestamp = int.Parse(allLines[0]);
            busIds = new List<int>();
            busIdAndPos = new List<(int, int)>();
            int pos = 0;
            foreach (string busId in allLines[1].Split(','))
            {
                if (busId[0] != 'x')
                {
                    busIds.Add(int.Parse(busId));
                    busIdAndPos.Add((int.Parse(busId), pos));
                }
                pos++;
            }
        }

        public void Part1()
        {
            int minDeparture = int.MaxValue;
            int minBusId = int.MaxValue;
            foreach (int busId in busIds)
            {
                int departure = (timestamp % busId == 0) ? 0 : ((timestamp / busId + 1) * busId - timestamp);
                if (departure < minDeparture)
                {
                    minDeparture = departure;
                    minBusId = busId;
                }
            }

            Console.WriteLine($"Day13 Part 1: {minDeparture * minBusId}");
        }

        public static (BigInteger match, BigInteger p) FirstMatchWithPeriod((int bi, int p) bp1, (int bi, int p) bp2, (BigInteger s, BigInteger period) sp)
        {
            BigInteger t = sp.s;
                        
            while (true)
            {
                if (((t + bp1.p) % bp1.bi == 0) && ((t + bp2.p) % bp2.bi) == 0)
                {
                    BigInteger nextT = t + sp.period;
                    BigInteger period = 0;
                    while (true)
                    {
                        if (((nextT + bp1.p) % bp1.bi == 0) && ((nextT + bp2.p) % bp2.bi) == 0)
                        {
                            period = nextT - t;
                            break;
                        }
                        nextT += sp.period;
                    }
                    return (t, period);
                }

                t += sp.period;
            }
        }
        public void Part2()
        {

            BigInteger t = 0;
            BigInteger p = 1;
            for (int i = 0; i < busIdAndPos.Count -1 ; i++)
                (t, p) = FirstMatchWithPeriod(busIdAndPos[i], busIdAndPos[i + 1], (t, p));

            Console.WriteLine($"Day13 Part2 {t}");

        }
    }
}
