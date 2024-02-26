using System;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            //test();
            structureTest();
        }

        static void test()
        {
            //Test generator
            // Generator gen = new Generator();
            // Tile dirt = new Tile("dirt","#", 1, 15);
            // Console.WriteLine(dirt.maxDepth);
            // Tile stone = new Tile("stone","=", 5,50);
            // Tile air = new Tile("air"," ",0, 10);
            // Tile[] tiles = {dirt, stone, air};

            // Tile[,] stoneHouseTiles = {
            //     {dirt,stone,dirt},
            //     {stone, air, stone},
            //     {stone, dirt, stone}
            // };

            // Structure stoneHouse = new Structure(stoneHouseTiles, 5);
            // Structure[] structures = {stoneHouse};
            // Tile[,] terrain = gen.Generate(100,49, tiles, structures, 1, 5);

            // for (int i = 0; i < terrain.GetLength(0);i++)
            // {
            //     for (int j = 0; j < terrain.GetLength(1);j++)
            //     {
            //         Console.Write(terrain[i,j].ttype);
            //     }
            //     Console.WriteLine();
            // }
        }

        static void structureTest()
        {
            Tile dirt = new Tile("dirt","#", 1, 15);
            Tile stone = new Tile("stone","=", 5,50);
            Tile air = new Tile("air"," ",0, 10);
            Tile[,] stoneHouseTiles = {
                {dirt,stone,dirt},
                {stone, air, stone},
                {stone, dirt, stone}
            };

            StructureTile centerTile = new StructureTile("+", true, true, true, true, stoneHouseTiles);
            StructureTile vTile = new StructureTile("|", true, false, true, false, stoneHouseTiles);
            StructureTile hTile = new StructureTile("-", false, true, false, true, stoneHouseTiles);
            StructureTile[] structureTiles = {vTile,centerTile,hTile};
            Structure myStruct = new Structure(structureTiles,5);
            myStruct.Generate();
            foreach (StructureTile sTile in myStruct.generatedTiles)
            {
                Console.WriteLine($"Tile Type: {sTile.sTileID}");
                Console.WriteLine($"Tile XCoordinates: {sTile.tileCoordinates[0]}");
                Console.WriteLine($"Tile YCoordinates: {sTile.tileCoordinates[1]}");
            }
        }
    }

    public class Tile
    {
        public string ttype;
        public int depth;
        public int maxDepth;

        public string tileID; //If the tileID is "void" it will just be empty. This can be used in structures that have a unique shape.

        public Tile(string ttileID, string type, int tileDepth, int maxTileDepth)
        {
            ttype = type;
            depth = tileDepth;
            maxDepth = maxTileDepth;
            tileID = ttileID;
        }
    }

    public class StructureTile
    {
        public string sTileID;
        public bool ConnectsTop;
        public bool ConnectsLeft;
        public bool ConnectsDown;
        public bool ConnectsRight;
        public int[] tileCoordinates;

        public Tile[,] structureTiles;

        public StructureTile(bool connectsTop, bool connectsLeft, bool connectsDown, bool connectsRight, Tile[,] tiles)
        {
            ConnectsTop = connectsTop;
            ConnectsLeft = connectsLeft;
            ConnectsDown = connectsDown;
            ConnectsRight = connectsRight;
            structureTiles = tiles;
        }

        public StructureTile(string tileID, bool connectsTop, bool connectsLeft, bool connectsDown, bool connectsRight, Tile[,] tiles)
        {
            ConnectsTop = connectsTop;
            ConnectsLeft = connectsLeft;
            ConnectsDown = connectsDown;
            ConnectsRight = connectsRight;
            structureTiles = tiles;
            sTileID = tileID;
        }
    }

    public class Structure
    {
        public StructureTile[] structureTiles;
        public StructureTile[] generatedTiles;
        public Tile[] tiles;
        public int[] structureSize = new int[2];
        public int minDepth;
        public int maxDepth;
        public int tileLimit;

        public Structure(StructureTile[] tiles, int TileLimit)
        {
            structureTiles = tiles;
            tileLimit = TileLimit;
            generatedTiles = new StructureTile[tileLimit];
        }

        public void Generate() //Tile[,]
        {
            Random rand = new Random();
            generatedTiles[0] = structureTiles[rand.Next(0,structureTiles.Length)];
            generatedTiles[0].tileCoordinates = new int[2] {0,0};
            for (int i = 0; i < tileLimit; i++)
            {
                bool[] avaliableDirections = new bool[4] {false, false, false, false};
                Console.WriteLine("made bool");
                StructureTile selectedTile = generatedTiles[rand.Next(0, generatedTiles.Length)];
                Console.WriteLine("made 1st sel");
                while (selectedTile == null)
                {
                    selectedTile = generatedTiles[rand.Next(0, generatedTiles.Length)];
                }
                Console.WriteLine("selected a valid");
                //Disgusting Code
                if (selectedTile.ConnectsTop && checkSpot(generatedTiles,selectedTile, 1,1))
                {
                    avaliableDirections[0] = true;
                }
                if (selectedTile.ConnectsDown && checkSpot(generatedTiles,selectedTile, -1, 1))
                {
                    avaliableDirections[1] = true;
                }
                if  (selectedTile.ConnectsRight && checkSpot(generatedTiles,selectedTile, 1, 0))
                {
                    avaliableDirections[2] = true;
                }
                if  (selectedTile.ConnectsLeft && checkSpot(generatedTiles,selectedTile, -1, 0))
                {
                    avaliableDirections[3] = true;
                }

                if (!avaliableDirections[0] && !avaliableDirections[1] && !avaliableDirections[2] && !avaliableDirections[3])
                {
                    //Dead end tile, all spots taken
                    i--;
                    Console.WriteLine("Dead end!");
                }
                else
                {
                    //Generate new tile
                    //int selectedDirection = avaliableDirections[rand.Next(0, 4)];

                    //Not random but selecting a direction randomly would require more disgusting code.
                    for (int j = 0; j < 4; j++)
                    {
                        Console.WriteLine("In for loop");
                        if (avaliableDirections[j] == true)
                        {
                            int selectedTilebongbing = rand.Next(0,structureTiles.Length);
                            Console.WriteLine($"Selecting Tile{selectedTilebongbing}");
                            generatedTiles[i] = structureTiles[selectedTilebongbing];
                            switch(j)
                            {
                                case 0:
                                    while (generatedTiles[i].ConnectsDown == false)
                                    {
                                        generatedTiles[i] = structureTiles[rand.Next(0,structureTiles.Length)];
                                    }
                                    generatedTiles[i].tileCoordinates = new int[2] {selectedTile.tileCoordinates[0], selectedTile.tileCoordinates[1]+1};
                                    break;
                                case 1:
                                    while (generatedTiles[i].ConnectsTop == false)
                                    {
                                        generatedTiles[i] = structureTiles[rand.Next(0,structureTiles.Length)];
                                    }
                                    generatedTiles[i].tileCoordinates = new int[2] {selectedTile.tileCoordinates[0], selectedTile.tileCoordinates[1]-1};
                                    break;
                                case 2:
                                    while (generatedTiles[i].ConnectsLeft == false)
                                    {
                                        generatedTiles[i] = structureTiles[rand.Next(0,structureTiles.Length)];
                                    }
                                    generatedTiles[i].tileCoordinates = new int[2] {selectedTile.tileCoordinates[0]+1,selectedTile.tileCoordinates[1]};
                                    break;
                                case 3:
                                    while (generatedTiles[i].ConnectsRight == false)
                                    {
                                        generatedTiles[i] = structureTiles[rand.Next(0,structureTiles.Length)];
                                    }
                                    generatedTiles[i].tileCoordinates = new int[2] {selectedTile.tileCoordinates[0]-1,selectedTile.tileCoordinates[1]};
                                    break;
                            }
                        }
                    }
                    Console.WriteLine("end of for");
                }
            }
            //Finished making generating the structure tiles.
            //TODO generate the tiles in an array.
        }

        static bool checkSpot(StructureTile[] generatedTiles, StructureTile selectedTile, int direction, int axis)
        {
            Console.WriteLine("===start of checkspot===");
            bool empty = true;
            for (int j = 0; j < generatedTiles.Length; j++)
            {
                Console.WriteLine("in checksportloop");
                if (generatedTiles[j] != null)
                {
                    Console.WriteLine($"passed 1st if axis is {axis} and dir is {direction} \n tile coords1 is {generatedTiles[j].tileCoordinates[axis]}"); //tile coords anret set?
                    if (generatedTiles[j].tileCoordinates[axis] == selectedTile.tileCoordinates[axis]+direction)
                    {
                        Console.WriteLine("passed 2nd if");
                        empty = false;
                    }
                    Console.WriteLine("wasnt it");
                }
            }
            return empty;
        }
    }

    public class Generator
    {
        public Tile[,] Generate(int mapx, int mapy, Tile[] tiles ,Structure[] structures, int structureGenerationChance, int blend)
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
                        int structureGeneratePick = rand.Next(0, structures.Length);
                        for (int iS = 0; iS < structures[structureGeneratePick].structureTiles.GetLength(0); iS++)
                        {
                            Console.WriteLine("First For Loop");
                            for (int iJ = 0; iJ < structures[structureGeneratePick].structureTiles.GetLength(1); iJ++)
                            {
                                Console.WriteLine($"Second For Loop, the length is {structures[structureGeneratePick].structureTiles.GetLength(1)}");
                                if (i+iS < terrain.GetLength(0) && j+iJ < terrain.GetLength(1))
                                {
                                    Console.WriteLine("Generate!");
                                    // if (structures[structureGeneratePick].structureTiles[iS, iJ].tileID != "void")
                                    // {
                                    //     terrain[i+iS,j+iJ] = structures[structureGeneratePick].structureTiles[iS,iJ];
                                    // }
                                    // else
                                    // {
                                    //     terrain[i+iS,j+iJ] = GenerateTile(tiles, i, blend);
                                    // }
                                }
                            }
                        }
                    }

                    if (terrain[i,j] == null)
                    {
                        
                        Tile pickedTile = GenerateTile(tiles, i, blend);
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

        static Tile GenerateTile(Tile[] tiles, int currentDepth, int blend)
        {
            Random rand = new Random();
            Tile[] avaliableTiles = new Tile[tiles.Length];
            foreach (Tile tile in tiles)
            {
                //int generate = rand.Next(i,mapy);
                //Console.WriteLine($"Generate is: {generate} and tile is {tile.depth} and {tile.maxDepth}");
                if (currentDepth >= tile.depth - rand.Next(0, blend))//(generate >= tile.depth)
                {
                    if (currentDepth <= tile.maxDepth + rand.Next(0, blend))//(generate <= tile.maxDepth)
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
                                        if (avaliableTiles[p].tileID == tile.tileID)
                                        {
                                            hasTileBeenUsed = true;
                                        }
                                    }
                                }
                                if (!hasTileBeenUsed)
                                {
                                    avaliableTiles[x] = new Tile(tile.tileID,tile.ttype, tile.depth, tile.maxDepth);
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
                Console.WriteLine("Didnt find fitting tile out of: "+avaliableTiles);
            }
            return pickedTile;
        }
    }
}