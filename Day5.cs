using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Linq;

namespace aoc2020
{
    class Day5
    {
        (int, int)[] seats;
        public Day5()
        {
            string[] boardingPasses = File.ReadAllLines("day5.txt");
            seats = boardingPasses.Select(bp => CalculateSeat(bp)).ToArray();
        }

        private static (int value, int pos) FindValue(string boardingPass, int stringPos, int rangeStart, int rangeEnd, char lowerChar)
        {
            int val = 0;
            int pos = 0;

            int mid = rangeStart + (rangeEnd - rangeStart) / 2;
            if (boardingPass[stringPos] == lowerChar)
                rangeEnd = mid;
            else
                rangeStart = mid + 1;

            if (rangeEnd != rangeStart)
                (val, pos) = FindValue(boardingPass, stringPos + 1, rangeStart, rangeEnd, lowerChar);
            else
                (val, pos) = (rangeStart, stringPos);

            return (val, pos);
        }
        private static (int rowNum, int colNum) CalculateSeat(string boardingPass)
        {
            (int row, int pos) = FindValue(boardingPass, 0, 0, 127, 'F');
            (int col, int pos2) = FindValue(boardingPass, pos+1, 0, 7, 'L');
            return (row, col);
        }

        private static int CalculateSeatId((int, int) rowCol)
        {
            return rowCol.Item1 * 8 + rowCol.Item2;
        }
        public static void TestSeatIds()
        {
            //Test first test case
            {
                int seatId = CalculateSeatId(CalculateSeat("FBFBBFFRLR"));
                Debug.Assert(seatId == 357);
            }

            {
                int seatId = CalculateSeatId(CalculateSeat("BFFFBBFRRR"));
                Debug.Assert(seatId == 567);
            }

            {
                int seatId = CalculateSeatId(CalculateSeat("FFFBBBFRRR"));
                Debug.Assert(seatId == 119);
            }

            {
                int seatId = CalculateSeatId(CalculateSeat("BBFFBBFRLL"));
                Debug.Assert(seatId == 820);
            }
        }
        public void Part1()
        {
            int[] seatIds = seats.Select(seat => CalculateSeatId(seat)).ToArray();
            Console.WriteLine("Day5 Part1 : {0}", seatIds.Max());
        }
        public void Part2()
        {
            int[] seatIds = seats.Select(seat => CalculateSeatId(seat)).ToArray();

            for (int row = 1; row < 127; row++)
                for (int col = 0; col < 8; col++)
                {
                    int seatId = CalculateSeatId((row, col));

                    if (Array.IndexOf(seatIds, seatId) == -1)
                    {
                        int idx1 = Array.IndexOf(seatIds, seatId - 1);
                        int idx2 = Array.IndexOf(seatIds, seatId + 1);
                        if (idx1 != -1 && idx2 != 0)
                        {
                            Console.WriteLine("Day5 part2: {0}", seatId);
                            return;
                        }
                    }
                }
            
        }
    }
}
