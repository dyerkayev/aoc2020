using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace aoc2020
{
    class PocketDimension3d
    {
        public int offset;
        public int size;
        public bool[] cubes;
        public bool[] updateCubes;
        public (int x, int z, int l) activeBounds;
        public PocketDimension3d(int size)
        {
            this.size = size;
            offset = size / 2;
            cubes = new bool[size * size * size];
            updateCubes = new bool[cubes.Length];
        }

        //0,0,0 is in the middle of the grid
        private int GetIndex(int x, int z, int layer)
        {   
            return (x + offset) + (z+offset) * size + (layer+offset) * size * size;
        }

        public bool IsActiveAt(int x, int z, int layer)
        {
            int gridX = x + offset;
            int gridL = layer + offset;
            int gridZ = z + offset;
            bool isActive = false;

            if (gridX >= size || gridX < 0 || gridZ >= size || gridZ < 0 || gridL >= size || gridL < 0)
                isActive = false;
            else
                isActive = cubes[GetIndex(x, z, layer)];


            return isActive;
        }

        public void SetActiveAt(int x, int z , int layer, bool active)
        {
            activeBounds.x = Math.Max(Math.Abs(x), activeBounds.x);
            activeBounds.z = Math.Max(Math.Abs(z), activeBounds.z);
            activeBounds.l = Math.Max(Math.Abs(layer), activeBounds.l);

            updateCubes[GetIndex(x, z, layer)] = active;
        }

        public void ApplyUpdates()
        {
            for (int i = 0; i < cubes.Length; i++)
                cubes[i] = updateCubes[i];
        }

        public int CountActiveNeighbors(int x, int z, int y, bool skipMiddle = true)
        {
            int numActiveNeighbors = 0;

            for (int dy = -1; dy < 2; dy++)
                for (int dz = -1; dz < 2; dz++)
                    for (int dx = -1; dx < 2; dx++)
                    {
                        if (skipMiddle && dx == 0 && dy == 0 && dz == 0)
                            continue;
                        if (IsActiveAt(x + dx, z + dz, y + dy))
                            numActiveNeighbors++;
                    }
            return numActiveNeighbors;
        }

        public void ForEachCube(Func<int, int, int, bool> func)
        {
            int lbound = Math.Min(offset, activeBounds.l + 2);
            int xbound = Math.Min(offset, activeBounds.x + 2);
            int zbound = Math.Min(offset, activeBounds.z + 2);

            for (int l = -lbound; l <= lbound; l++)
                for (int z = -zbound; z <= zbound; z++)
                    for (int x = -xbound; x <= xbound; x++)
                        func(x, z, l);
        }

        public void DebugPrint()
        {
            StringBuilder output = new StringBuilder();
                        
            Console.Write("\n");

            int lbound = Math.Min(offset, activeBounds.l);
            int xbound = Math.Min(offset, activeBounds.x);
            int zbound = Math.Min(offset, activeBounds.z);

            for (int y = -lbound; y <= lbound; y++)
                Console.Write($"{y}\t\t");
            Console.WriteLine("");
            for (int z = -zbound; z <= zbound; z++)
            {
                for (int l = -lbound; l <= lbound; l++)
                {
                    for (int x = -xbound; x <= xbound; x++)
                    {
                        if (x == 0 && z == 0)
                            output.Append(IsActiveAt(x, z, l) ? '+' : '0');
                        else
                            output.Append(IsActiveAt(x, z, l) ? '#' : '.');
                    }
                    output.Append("\t");
                }
                output.Append('\n');
            }

            Console.Write(output);
        }
    }
    class PocketDimension4d
    {
        PocketDimension3d[] dims3d;
        int offset;
        int size;
        int wbound;
        public PocketDimension4d(int size)
        {
            dims3d = new PocketDimension3d[size];
           
            for (int i = 0; i < size; i++)
                dims3d[i] = new PocketDimension3d(size);
            
            offset = dims3d[0].offset;
            this.size = size;
        }

        public bool IsActiveAt(int x, int z, int layer, int w)
        {
            int gridW = w + offset;
            if (gridW >= size || gridW < 0)
                return false;
            return dims3d[gridW].IsActiveAt(x, z, layer);
        }

        public void SetActiveAt(int x, int z, int layer, int w, bool active)
        {
            int gridW = w + offset;
            wbound = Math.Max(Math.Abs(w), wbound);
            dims3d[gridW].SetActiveAt(x, z, layer, active);
        }

        public void ApplyUpdates()
        {
            int xbound = 0;
            int zbound = 0;
            int lbound = 0;
            for (int i = 0; i < size; i++)
            {
                xbound = Math.Max(xbound, dims3d[i].activeBounds.x);
                zbound = Math.Max(zbound, dims3d[i].activeBounds.z);
                lbound = Math.Max(lbound, dims3d[i].activeBounds.l);
            }

            for (int i = 0; i < size; i++)
            {
                dims3d[i].ApplyUpdates();
                dims3d[i].activeBounds.x = xbound;
                dims3d[i].activeBounds.z = zbound;
                dims3d[i].activeBounds.l = lbound;

            }
        }

        public int CountActiveNeighbors(int x, int z, int y, int w)
        {
            int numActiveNeighbors = 0;
            for (int dw = -1; dw < 2; dw++)
            {
                int gridW = w + dw + offset;
                if (gridW < 0 || gridW >= size)
                    continue;

                numActiveNeighbors += dims3d[gridW].CountActiveNeighbors(x, z, y, dw == 0);
            }
            return numActiveNeighbors;
        }

        public void ForEachCube(Func<int, int, int, int, bool> func)
        {
            int bound = wbound + 2;
            for (int w = -bound; w <= bound; w++)
                dims3d[w + offset].ForEachCube((x,y,z) => func(x,y,z,w));
        }

        public void DebugPrint()
        {
            for (int w = -wbound; w <= wbound; w++)
            {
                Console.WriteLine($"W:{w}");
                int gridW = w + offset;
                dims3d[gridW].DebugPrint();
            }
        }

    }
    class Day17
    {
        PocketDimension3d dim3d;
        PocketDimension4d dim4d;
        public Day17()
        {
            dim3d = new PocketDimension3d(45);
            dim4d = new PocketDimension4d(45);

            string[] layer0 = File.ReadAllLines("day17.txt");
            for(int z = 0; z < layer0.Length; z++)
            {
                for (int x = 0; x < layer0[z].Length; x++)
                {
                    if (layer0[z][x] == '#')
                    {
                        dim3d.SetActiveAt(x - layer0.Length / 2, z - layer0.Length / 2, 0, true);
                        dim4d.SetActiveAt(x - layer0.Length / 2, z - layer0.Length / 2, 0, 0, true);
                    }

                }
            }

            dim3d.ApplyUpdates();
            dim4d.ApplyUpdates();
        }

        public void Part1()
        {
           // dim.DebugPrint();
            for (int s = 0; s < 6; s++)
            {
                dim3d.ForEachCube((x, z, l) =>
                    {
                        int c = dim3d.CountActiveNeighbors(x, z, l);
                        if (dim3d.IsActiveAt(x, z, l) && (c != 2 && c != 3))
                            dim3d.SetActiveAt(x, z, l, false);
                        else if (!dim3d.IsActiveAt(x, z, l) && c == 3)
                            dim3d.SetActiveAt(x, z, l, true);
                        return true;
                    });

                Console.WriteLine($"Step {s + 1}");
                
                dim3d.ApplyUpdates();
                dim3d.DebugPrint();

            }
            int numActive = 0;
            dim3d.ForEachCube((x, z, l) => { if (dim3d.IsActiveAt(x, z, l)) { numActive++; } return true; });
            Console.WriteLine($"Day17 part 1 {numActive}");
        }

        public void Part2()
        {
            // dim.DebugPrint();
            for (int s = 0; s < 6; s++)
            {
                dim4d.ForEachCube((x, z, l, w) =>
                {
                    int c = dim4d.CountActiveNeighbors(x, z, l, w);
                    if (dim4d.IsActiveAt(x, z, l, w) && (c != 2 && c != 3))
                        dim4d.SetActiveAt(x, z, l, w, false);
                    else if (!dim4d.IsActiveAt(x, z, l, w) && c == 3)
                        dim4d.SetActiveAt(x, z, l, w, true);
                    return true;
                });

                Console.WriteLine($"Step {s + 1}");

                dim4d.ApplyUpdates();
                //dim4d.DebugPrint();
            }
            int numActive = 0;
            dim4d.ForEachCube((x, z, l, w ) => { if (dim4d.IsActiveAt(x, z, l, w)) { numActive++; } return true; });
            Console.WriteLine($"Day17 part 2 {numActive}");
        }
    }
}
