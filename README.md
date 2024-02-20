UTG Is a custom terrain generator i built from scratch. It is pretty customizeable with custom blocks and structures that you can add. 
Features:
  - Tile Class
  - Structure Class
  - Generator Class

#Tile Class
The Tile Class is for creating tiles that will be generated.
Usage: ```Tile dirt = new Tile("#", 1, 15);```
  *  Tile(tile type, minimum depth, maximum depth).
  *  The tile type does not need to be a single character since the generator returns an array of tiles, but in my current code to test the generator it is used as a "tile texture".

#Structure Class
The Structure Class is for creating structures that will be generated on the map (Currently very primitive)
Usage: ```Structure house = new Structure({
  {air, air, wood, air, air},
  {air, wood, air, wood, air},
  {wood, air, air, air, wood},
  {wood, air, air, air, wood},
  {wood, air, air, air, wood},
});```
  *  The "wood" and "air" tiles are used as examples, you would have to create your own tiles.
  *  Structure(2D Array of the structure in tiles[]).

#Generator Class
This is the class that does the heavy lifting.
Usage: ```
Generator gen = new Generator();
Tile[,] terrain = gen.Generate(100,49, tiles, structures, 1, 5);```
Returns: 2D Tile Array.
  *  Generate(int mapx, int mapy, Tile[] tiles ,Structure[] structures, int structureGenerationChance, int blend)
  -  `mapx` - Map Size on the X axis.
  -  `mapy` - Map Size on the Y axis.
  -  `tiles` - An array of tiles that will be used in the map.
  -  `structures` - Structures that could be generated in the map.
  -  `structureGenerationChance` - Chance of a structure generating (in percentage)
  -  `blend` - How much the maximum and minimum depth of a tile can deviate (Often helps the map look more "natural").

#Tips for generating (Since the generator is not perfect yet)
  *  You can create many tiles that operate in the same area if you want a higher percentage chance of them spawning in that specific area.
     Example:
     ```
     Generator gen = new Generator();
     Tile dirt = new Tile("#", 1, 15);
     Tile stone = new Tile("=", 5,50);

     //Three air Tile objects here
     Tile air = new Tile(" ",0, 10);
     Tile air1 = new Tile(" ",0, 10); 
     Tile air2 = new Tile(" ",0, 10);
     
     Tile[] tiles = {dirt, stone, air, air1, air2};

     Tile[,] terrain = gen.Generate(100,49, tiles, {}, 1, 5);
     ```
     Result: There will be more air than any other tile from 1 to 10.
