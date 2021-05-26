using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Numerics;

namespace aoc2020
{
    

    class Tile
    {
        public Tile(string firstLine)
        {
            edges = new string[4];
            id = int.Parse(firstLine.Split(' ')[1].TrimEnd(':'));
            data = new char[10,10];
        }

        public int id;
        public string[] edges;

        public char[,] data;
    }

    public enum EdgeMatch
    {
        None,
        Normal,
        Flipped
    }

    //struct TileMatch
    //{
    //    public EdgeMatch edgeMatch;
    //    public int e1;
    //    public int e2;
    //}

    class TileAndTransform
    {
        TileAndTransform(Tile tile)
        {
            this.tile = tile;
            rotate = 0;
            flip = Flip.None;
        }
        
        public Tile tile;
        public int rotate; //0 none, 1 - 90 and so on, 2 -180, 3 - 270
        public enum Flip
        {
            None,
            Horizontal,
            Vertical
        }
        public Flip flip; //1 - horizontal flip, 2 vertical flip, 0 no flip
    }


    class Day20
    {


        List<Tile> tiles;
        public Day20()
        {
            tiles = new List<Tile>();

            Tile tile = new Tile("Tile 0:");
            int tileLines = 0;
            int tileSize = 10;
            StringBuilder edge3builder = new StringBuilder(10);
            StringBuilder edge1builder = new StringBuilder(10);
            int numTiles = 0;
            foreach (string line in File.ReadAllLines("day20.txt"))
            {
                if (line.Length == 0)
                    continue;
                if (line.StartsWith("Tile"))
                {
                    tile = new Tile(line);
                    tileLines = 0;
                    edge3builder = new StringBuilder(10);
                    edge1builder = new StringBuilder(10);
                    continue;
                }
                for (int i = 0; i < line.Length; i++)
                    tile.data[i, tileLines] = line[i];

                if (tileLines == 0)
                {
                    tile.edges[0] = line;
                }

                edge3builder.Append(line[0]);
                edge1builder.Append(line[tileSize-1]);

                if (tileLines == tileSize - 1 )
                {
                    tile.edges[2] = ReverseString(line);
                    tile.edges[3] = ReverseString(edge3builder.ToString());
                    tile.edges[1] = edge1builder.ToString();
                    tiles.Add(tile);
                    numTiles++;
                }

                tileLines++;
            }
        }

        static string ReverseString(string s)
        {
            char[] c = s.ToCharArray();
            Array.Reverse(c);
            return new string(c);
        }

        public (EdgeMatch, int) CheckTileHasEdge(Tile tile1, string edge)
        {
            for (int i = 0; i < tile1.edges.Length; i++)
            {
                if (edge.StartsWith(tile1.edges[i]))
                {
                    return (EdgeMatch.Normal, i);
                }
            }

            for (int i = 0; i < tile1.edges.Length; i++)
            {
                if (ReverseString(edge).StartsWith(tile1.edges[i]))
                {
                    return (EdgeMatch.Flipped, i);
                }
            }
            return (EdgeMatch.None, -1);
        }

        public (EdgeMatch m, int eidx1, int eidx2) CheckTwoTiles(Tile tile1, Tile tile2)
        {
            //tiles can be rotated and flipped
            for (int eidx1 = 0; eidx1 < tile1.edges.Length; eidx1++)
            {
                string edge = tile1.edges[eidx1];
                (EdgeMatch m, int eidx2) = CheckTileHasEdge(tile2, edge);
                if (m != EdgeMatch.None)
                    return (m, eidx1, eidx2);
            }

            return (EdgeMatch.None, -1, -1);
        }

        public void Part1()
        {
            BigInteger result = 1;
            //corner tile is the one that has 2 of its edges unconnected
            //match the edges by putting them into dictionary
            //list tile indices int the list 
            //if an edge is reversed, the tile is flipped
            Dictionary<int, List<int>> tileNeigbors = new Dictionary<int, List<int>>();
            foreach (Tile tile in tiles)
            {
                tileNeigbors[tile.id] = new List<int>();
                foreach (Tile tile1 in tiles)
                {
                    if(tile1.id != tile.id)
                    {
                        (EdgeMatch m, _, _) = CheckTwoTiles(tile1, tile);
                        if (m != EdgeMatch.None)
                            tileNeigbors[tile.id].Add(tile1.id);
                    }
                }
            }

            foreach (var entry in tileNeigbors)
            {
                if (entry.Value.Count == 2)
                    result *= entry.Key;
            }

            Console.WriteLine($"Day20 Part1 result {result}");
        }



        public void Part2()
        {
            //int imageSize = (int)Math.Sqrt(tiles.Count);
            //char[,] image = new char[imageSize * 3, imageSize * 3];
            //for (int i = 0; i < imageSize; i++)
            //    for (int j = 0; j < imageSize; j++)
            //        image[i,j] = ' ';

            Dictionary<int, List<int>> tileNeigbors = new Dictionary<int, List<int>>();
            foreach (Tile tile in tiles)
            {
                tileNeigbors[tile.id] = new List<int>();
                foreach (Tile tile1 in tiles)
                {
                    if (tile1.id != tile.id)
                    {
                        (EdgeMatch m, _, _) = CheckTwoTiles(tile1, tile);
                        if (m != EdgeMatch.None)
                            tileNeigbors[tile.id].Add(tile1.id);
                    }
                }
            }
            
            int cornerTileId = 0;
            foreach (var entry in tileNeigbors)
            {
                if (entry.Value.Count == 2)
                    cornerTileId = entry.Key;
            }

            List<int> allTileIds = new List<int>();
            foreach (Tile tile in tiles)
                allTileIds.Add(tile.id);

            while (allTileIds.Count > 0)
            {

            }







            //Dictionary<int, List<int>> tileNeigbors = new Dictionary<int, List<int>>();
            foreach (Tile tile1 in tiles)
            {
                tileNeigbors[tile1.id] = new List<int>();
                foreach (Tile tile2 in tiles)
                {
                    if (tile2.id != tile1.id)
                    {
                        (EdgeMatch m, int e1, int e2) = CheckTwoTiles(tile1, tile2);                       
                        //flipped match means that tile1 is edge e1 is flipped
                        if (m == EdgeMatch.Flipped)
                        {

                        }    

                    }
                }
            }

            //foreach (Tile tile in tiles)
            //{
            //    foreach (Tile tile1 in tiles)
            //    {
            //        if (tile1.id != tile.id)
            //        {
            //            for (int e = 0; e < tile.edges.Length; e++)
            //            {
            //                string edge = tile.edges[e];
            //                (Match m, int eidx) = CheckTileHasEdge(tile1, edge);
            //                if (m == Match.Flipped)
            //                {

            //                }
            //            }
            //        }
            //    }
            //}

        }
    }
}
