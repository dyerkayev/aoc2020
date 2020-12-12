using System;   
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace aoc2020
{
    class GridOfPlaces
    {
        public GridOfPlaces(int width, int height)
        {
            places = new char[width * height];
            for (int i = 0; i < width * height; i++)
                places[i] = '.';
        }
        public GridOfPlaces(string[] lines)
        {
            width = lines[0].Length + 2;
            height = lines.Length + 2;
            places = new char[width * height];

            for (int l = 1; l < height - 1; l++)
            {
                for (int c = 1; c < width - 1; c++)
                    SetSeat(c, l, lines[l - 1][c - 1]);
            }

            //fill borders
            for (int c = 0; c < width; c++)
            {
                SetSeat(c, 0, '.');
                SetSeat(c, height - 1, '.');
            }

            for (int l = 0; l < height; l++)
            {
                SetSeat(0, l, '.');
                SetSeat(width - 1, l, '.');
            }
        }
        public char GetSeat(int x, int y)
        {
            return places[x + y * width];
        }
        public void SetSeat(int x, int y, char c)
        {
            places[x + y * width] = c;
        }

        public void Print()
        {
            for (int l = 0; l < height; l++)
            {
                string line = "";
                for (int c = 0; c < width; c++)
                {
                    line += GetSeat(c, l);
                }
                Console.WriteLine(line);
            }

        }

        public int NumOccupiedNeighbours(int x, int y)
        {
            int numOccupied = 0;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    if (GetSeat(x + dx, y + dy) == '#')
                        numOccupied++;
                }
            }
            return numOccupied;
        }

        public int NumOccupiedVisible(int x, int y)
        {
            (int dx, int dy)[] directions = new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1), (1, 1), (-1, -1), (-1, 1), (1, -1) };
            int numOccupied = 0;

            foreach ((int dx, int dy) d in directions)
            {
                int px = x;
                int py = y;

                while (px < width -1 && py < height -1 && px > 0 && py > 0)
                {
                    px += d.dx;
                    py += d.dy;
                    if (GetSeat(px, py) == '#')
                    {
                        numOccupied++;
                        break;
                    }
                    else if (GetSeat(px, py) == 'L')
                        break;
                }

            }
 
            return numOccupied;
        }
        public void StepRound1(GridOfPlaces inGrid)
        {
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {

                    //occupied seats
                    int numOccupied = inGrid.NumOccupiedNeighbours(x, y);

                    if (inGrid.GetSeat(x, y) == 'L' && numOccupied == 0)
                    {
                        SetSeat(x, y, '#');
                    }
                    else if (inGrid.GetSeat(x, y) == '#' && numOccupied >= 4)
                    {
                        SetSeat(x, y, 'L');
                    }
                    else
                    {
                        SetSeat(x, y, inGrid.GetSeat(x, y));
                    }
                }
            }

        }

        public void StepRound2(GridOfPlaces inGrid)
        {
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    //occupied seats
                    int numOccupied = inGrid.NumOccupiedVisible(x, y);

                    if (inGrid.GetSeat(x, y) == 'L' && numOccupied == 0)
                    {
                        SetSeat(x, y, '#');
                    }
                    else if (inGrid.GetSeat(x, y) == '#' && numOccupied >= 5)
                    {
                        SetSeat(x, y, 'L');
                    }
                    else
                    {
                        SetSeat(x, y, inGrid.GetSeat(x, y));
                    }
                }
            }

        }

        public int CountOccupied()
        {
            int numOccupied = 0;
            for (int i = 0; i < width * height; i++)
                if (places[i] == '#')
                    numOccupied++;

            return numOccupied;
        }

        public bool IsSame(GridOfPlaces grid)
        {
            for (int i = 0; i < width * height; i++)
                if (grid.places[i] != places[i]) 
                    return false;
            return true;
        }
        char[] places;
        int width;
        int height;
    }
    class Day11
    {
        private GridOfPlaces grid1;
        private GridOfPlaces grid2;
        public Day11()
        {
            string[] lines = File.ReadAllLines("day11.txt");
            grid1 = new GridOfPlaces(lines);
            grid2 = new GridOfPlaces(lines);
        }
        
        public void Part1()
        {            
            do
            {
                GridOfPlaces temp = grid1;
                grid1 = grid2;
                grid2 = temp;

                grid2.StepRound1(grid1);
            }
            while (!grid1.IsSame(grid2));

            Console.WriteLine($"Num occupied: {grid2.CountOccupied()}");

        }

        public void Part2()
        {
            do
            {
                GridOfPlaces temp = grid1;
                grid1 = grid2;
                grid2 = temp;

                grid2.StepRound2(grid1);
            }
            while (!grid1.IsSame(grid2));

            Console.WriteLine($"Num occupied: {grid2.CountOccupied()}");

        }

    }
}
