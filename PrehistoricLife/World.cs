using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class World
    {
        public static Dictionary<DayPart, int> DayPartsTickCount = new Dictionary<DayPart, int>()
        {
            {DayPart.Morning,50},
            {DayPart.Daytime,70},
            {DayPart.Evening,50},
            {DayPart.Night,50},
        };
        public static Dictionary<DayPart, int> DayPartsSight = new Dictionary<DayPart, int>()
        {
            {DayPart.Morning,2},
            {DayPart.Daytime,3},
            {DayPart.Evening,2},
            {DayPart.Night,1},
        };
        public static readonly int dayTickCount;
        public static readonly int maxSight;
        public static readonly int sightCount;
        public const int WordCount = 10;
        public bool[] cry;
        public int tick = 0;
        public int width;
        public int height;
        Tile[,] tiles;
        Random rnd;
        public Tile this[int x,int y]
        {
            get
            {
                return tiles[((x % width) + width) % width, ((y % height) + height) % height];
            }
        }
        public Tile this[Point p]
        {
            get
            {
                return tiles[((p.X % width) + width) % width, ((p.Y % height) + height) % height];
            }
        }
        public DayPart DayPartNow
        {
            get
            {
                int n = tick % dayTickCount;
                int k = 0;
                int i = 0;
                while (n >= k + DayPartsTickCount[(DayPart)i]) { k += DayPartsTickCount[(DayPart)i]; i++; }
                return (DayPart)i;
            }
        }
        static World()
        {
            maxSight = DayPartsSight.Values.Max();
            sightCount = (int)Math.Pow(maxSight + 1, 2);
            dayTickCount = DayPartsTickCount.Values.Sum();
        }
        public World(int width,int height,Random rnd)
        {
            this.rnd = rnd;
            this.width = width;
            this.height = height;
            cry = new bool[WordCount];
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
        public void Update()
        {

        }
    }
}
