using System;
namespace PrehistoricLife
{
    public class World
    {
        public int width;
        public int height;
        Tile[,] tiles;
        Random rnd;
        public World(int width,int height,Random rnd)
        {
            this.rnd = rnd;
            this.width = width;
            this.height = height;
            tiles = new Tile[width,height];
            Generate(rnd);
        }
        public void Generate(Random rnd)
        {
            for (int x = 0; x < width;x++)
            {
                for (int y = 0; y < height;y++)
                {
                    tiles[x, y] = new Tile(LandType.Grass,StaticObject.Nothing,new int[Enum.GetValues(typeof(Item)).Length]);
                }
            }
        }
    }
}
