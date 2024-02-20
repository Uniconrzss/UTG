using System;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test generator
            Generator gen = new Generator();
            Tile dirt = new Tile("#", 1, 15);
            Console.WriteLine(dirt.maxDepth);
            Tile stone = new Tile("=", 5,50);
            Tile air = new Tile(" ",0, 10);
            Tile[] tiles = {dirt, stone, air};

            Tile[,] stoneHouseTiles = {
                {dirt,stone,dirt},
                {stone, air, stone},
                {stone, dirt, stone}
            };

            Structure stoneHouse = new Structure(stoneHouseTiles);
            Structure[] structures = {stoneHouse};
            Tile[,] terrain = gen.Generate(100,49, tiles, structures, 1);

            for (int i = 0; i < terrain.GetLength(0);i++)
            {
                for (int j = 0; j < terrain.GetLength(1);j++)
                {
                    Console.Write(terrain[i,j].ttype);
                }
                Console.WriteLine();
            }
        }
    }

    public class Tile
    {
        public string ttype;
        public int depth;
        public int maxDepth;

        public Tile(string type, int tileDepth, int maxTileDepth)
        {
            ttype = type;
            depth = tileDepth;
            maxDepth = maxTileDepth;
        }
    }

    public class Structure
    {
        public Tile[,] structureTiles;
        public int[] structureSize = new int[2];

        public Structure(Tile[,] tiles)
        {
            structureTiles = tiles;
            structureSize[0] = structureTiles.GetLength(0);
            structureSize[1] = structureTiles.GetLength(1);
        }
    }

    public class Generator
    {
        public Tile[,] Generate(int mapx, int mapy, Tile[] tiles ,Structure[] structures, int structureGenerationChance)
        {
            Tile[,] terrain = new Tile[mapy,mapx];
            Random rand = new Random();

            for (int i = 0; i < mapy; i++)
            {
                for (int j = 0; j < mapx; j++)
                {
                    int structureGenerate = rand.Next(0, 100);
                    if (structureGenerate < structureGenerationChance)
                    {
                        //Pick random structure
                        int structureGeneratePick = rand.Next(0, structures.Length-1);
                        for (int iS = 0; iS < structures[structureGeneratePick].structureTiles.GetLength(0); iS++)
                        {
                            Console.WriteLine("First For Loop");
                            for (int iJ = 0; iJ < structures[structureGeneratePick].structureTiles.GetLength(1); iJ++)
                            {
                                Console.WriteLine($"Second For Loop, the length is {structures[structureGeneratePick].structureTiles.GetLength(1)}");
                                if (i+iS < terrain.GetLength(0) && j+iJ < terrain.GetLength(1))
                                {
                                    Console.WriteLine("Generate!");
                                    terrain[i+iS,j+iJ] = structures[structureGeneratePick].structureTiles[iS,iJ]; //I have honestly no clue if this will work
                                }
                            }
                        }
                    }

                    if (terrain[i,j] == null)
                    {
                        Tile[] avaliableTiles = new Tile[tiles.Length];
                        foreach (Tile tile in tiles)
                        {
                            //int generate = rand.Next(i,mapy);
                            //Console.WriteLine($"Generate is: {generate} and tile is {tile.depth} and {tile.maxDepth}");
                            if (i >= tile.depth)//(generate >= tile.depth)
                            {
                                if (i <= tile.maxDepth)//(generate <= tile.maxDepth)
                                {
                                    for (int x = 0; x < avaliableTiles.Length; x++)
                                    {
                                        if (avaliableTiles[x] == null)
                                        {
                                            //Console.WriteLine($"Length of array is now: {avaliableTiles.Length}");
                                            bool hasTileBeenUsed = false;
                                            for (int p = 0; p <avaliableTiles.Length; p++)
                                            {
                                                if (avaliableTiles[p] != null)
                                                {
                                                    if (avaliableTiles[p].ttype == tile.ttype)
                                                    {
                                                        hasTileBeenUsed = true;
                                                    }
                                                }
                                            }
                                            if (!hasTileBeenUsed)
                                            {
                                                avaliableTiles[x] = new Tile(tile.ttype, tile.depth, tile.maxDepth);
                                            }
                                            //DEBUG
                                            // Console.WriteLine($"Tiles array is now: ");
                                            // for (int p = 0; p < avaliableTiles.Length; p++)
                                            // {
                                            //     if (avaliableTiles[p] != null)
                                            //     {
                                            //         Console.Write(avaliableTiles[p].ttype+" ");
                                            //     }
                                            // }
                                        }
                                    }
                                }
                            }
                        }
                        Tile pickedTile = null;
                        while (pickedTile == null)
                        {
                            int pickTile = rand.Next(0, avaliableTiles.Length-1);
                            pickedTile = avaliableTiles[pickTile];
                            //Console.WriteLine("Didnt find fitting tile out of: "+avaliableTiles);
                        }
                        //Put tile in array
                        if (pickedTile != null)
                        {
                            terrain[i,j] = pickedTile;
                        }
                    }
                }
            }

            return terrain;
        }
    }
}