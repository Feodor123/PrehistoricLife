using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace PrehistoricLife
{
    public class World
    {
        public static Point[] directions = new Point[]
        {
            new Point(-1,0),
            new Point(-1,1),
            new Point(0,1),
            new Point(1,1),
            new Point(1,0),
            new Point(1,-1),
            new Point(0,-1),
            new Point(-1,-1),
        };
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
        public static Dictionary<DayPart, Color> Colors = new Dictionary<DayPart, Color>()
        {
            {DayPart.Morning,new Color(200,200,180) },
            {DayPart.Daytime,new Color(250,250,250) },
            {DayPart.Evening,new Color(200,180,180) },
            {DayPart.Night,new Color(120,120,140) },
        };
        public static Dictionary<Item, int> throwDamage = new Dictionary<Item, int>()
        {
            {Item.Stick, 20 },
            {Item.Stone, 40 },
            {Item.Food, 5 },
            {Item.Spear, 80 },
        };
        public static readonly int dayTickCount;
        public static readonly int maxSight;
        public static readonly int sightCount;
        public const int WordCount = 10;
        public const int FoodNutrition = 60;
        public const double TreeGererationChanse = 0.2;
        public const int Width = 80;
        public const int Height = 80;
        public const int ThrowDistantion = 4;
        public const int Watering = 50;

        private List<ThrowedObject> throwed = new List<ThrowedObject>();
        private bool[] cryNow;
        private Tile[,] tiles;
        private Random rnd;
        private List<Human> humans;

        private List<Entity> animals;
        public int number;
        public List<Genom> Genoms => humans.Select(_ => _.genom).ToList();
        public int HumanCount => humans.Count;
        public bool[] cryBefore;
        public int tick = 0;
        public Tile this[int x,int y]
        {
            get
            {
                return tiles[((x % Width) + Width) % Width, ((y % Height) + Height) % Height];
            }
            private set
            {
                tiles[((x % Width) + Width) % Width, ((y % Height) + Height) % Height] = value;
            }
        }
        public Tile this[Point p]
        {
            get
            {
                return tiles[((p.X % Width) + Width) % Width, ((p.Y % Height) + Height) % Height];
            }
            private set
            {
                tiles[((p.X % Width) + Width) % Width, ((p.Y % Height) + Height) % Height] = value;
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
        public World(Random rnd,int number, List<Genom> genoms)
        {
            animals = new List<Entity>();
            humans = new List<Human>();
            this.number = number;
            this.rnd = rnd;
            cryNow = new bool[WordCount];
            cryBefore = new bool[WordCount];
            tiles = new Tile[Width, Height];
            Generate(rnd);
            AddHumans(genoms);
        }

        private void AddHumans(List<Genom> genoms)
        {
            foreach(var g in genoms)
            {
                humans.Add(new Human(new Point(rnd.Next(Width), rnd.Next(Height)), g, this));
            }
        }
        public void Generate(Random rnd)
        {
            for (int x = 0; x < Width;x++)
            {
                for (int y = 0; y < Height;y++)
                {
                    tiles[x, y] = new Tile(LandType.Grass,StaticObject.Nothing,new int[Enum.GetValues(typeof(Item)).Length - 1]);
                }
            }
            List<Point> points = new List<Point>
            {
                new Point(rnd.Next(Width), rnd.Next(Height))
            };
            do
            {
                Dictionary<int, Point> dict = new Dictionary<int, Point>()
                {
                    {0,new Point(-1,0)},
                    {1,new Point(0,1)},
                    {2,new Point(1,0)},
                };
                Point p;
                do
                {
                    p = dict[rnd.Next(3)];
                }
                while (p.X != 0 && ((points.Count >= 3 && points[points.Count - 1] + p + new Point(0,-1) == points[points.Count - 3])||(points.Count >= 2 && points[points.Count - 2] == points[points.Count - 1] + p)));
                Point pp = points.Last() + p;
                pp.X %= Width;
                pp.Y %= Height;
                points.Add(pp);
            }
            while (points.Count(_ => _ == points.Last()) <= 1);
            while (points.First() != points.Last()) points.RemoveAt(0);
            points.RemoveAt(0);
            foreach (var p in points)
            {
                this[p].landType = LandType.Water;
            }
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (tiles[x,y].landType == LandType.Grass && rnd.NextDouble() < TreeGererationChanse)
                    {
                        tiles[x, y].staticObject = StaticObject.TreeObject;
                    }
                }
            }
        }
        public void Update()
        {
            tick++;
            cryBefore = cryNow;
            cryNow = new bool[WordCount];
            UpdateThrows();
            for(int i = humans.Count; i > 0;i--)
            {
                humans[humans.Count - i].Update(this);
            }
            for (int i = animals.Count; i > 0; i--)
            {
                animals[animals.Count - i].Update(this);
            }

        }
        public void UpdateThrows()
        {
            foreach (var s in throwed)
            {
                Point p = s.position;
                for (int i = 1; i <= ThrowDistantion; i++)
                {
                    p += s.direction;
                    if (this[p].entities.Count(_ => !(_ is Human)) > 0)
                    {
                        this[p].entities.First(_ => !(_ is Human)).Hit(throwDamage[s.item]);
                    }
                }
            }
            throwed.Clear();
        }
        public void Kill(Entity entity)
        {
            if (entity is Human)
            {
                humans.Remove(entity as Human);
            }
            else
            {
                animals.Remove(entity);
            }
        }
        public void Go(Entity entity,int direction)
        {
            this[entity.position].entities.Remove(entity);
            this[entity.position + directions[direction]].entities.Add(entity);
        }
        public void Throw(Item item,Point position,int direction)
        {
            throwed.Add(new ThrowedObject(item, position, direction));
        }
        public void Cry(int word)
        {
            cryNow[word] = true;
        }
        public struct ThrowedObject
        {
            public Item item;
            public Point position;
            public Point direction;
            public ThrowedObject(Item item, Point position, int direction)
            {
                this.item = item;
                this.position = position;
                this.direction = directions[direction];
            }
        }

        public void Draw(SpriteBatch spriteBatch,Texture2D atlasTexture, Point sightPosition, int tileSize,Point ScreenSize)
        {
            for (int x = 0;x <= ScreenSize.X / tileSize; x++)
            {
                for (int y = 0; y <= ScreenSize.Y / tileSize; y++)
                {
                    this[sightPosition.X + x, sightPosition.Y + y].Draw(spriteBatch, atlasTexture, new Point(x * tileSize, y * tileSize), tileSize, Colors[DayPartNow]);
                }
            }
        }
    }
}
