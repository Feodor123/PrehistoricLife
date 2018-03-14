using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class Genom
    {
        Random rnd;
        static int OperationsTypeCount;
        static int handCount = Enum.GetNames(typeof(Hand)).Length;
        static int itemCount = Enum.GetValues(typeof(Item)).Length;
        const int maxActionCount = 50;
        const int Height = 30;
        const int Width = 300;
        int angle = 0;
        int x = 0;
        int y = 0;
        int[,] code = new int[Width, Height];
        StaticObject usedStaticObject = StaticObject.Nothing;

        public static Dictionary<ActionType, int> actionCount = new Dictionary<ActionType, int>()
        {
            {ActionType.Nothing,1},
            {ActionType.Go,8},
            {ActionType.Take,handCount * (itemCount - 1)},//брать ничто нельзя
            {ActionType.Put,handCount},
            {ActionType.Throw,handCount * 8},
            {ActionType.Eat,handCount},
            {ActionType.Sleep,1},
            {ActionType.UseStaticObject,2},//начать/закончить
            {ActionType.Fight,1},
            {ActionType.Drink,1},
            {ActionType.Shout,World.WordCount},
            {ActionType.SeeStaticObject,World.sightCount},//под себя тоже можно
            {ActionType.SeeLand,World.sightCount},//под себя тоже можно
            {ActionType.SeeItem,World.sightCount * (itemCount)},//проверка на соответствующий item (или на полное отстствие)
            {ActionType.SeeEntity,World.sightCount * (Enum.GetValues(typeof(EntityType)).Length)},
            {ActionType.Listen,World.WordCount},
            {ActionType.GetHandType,handCount},
            {ActionType.GetHandCount,handCount},
            {ActionType.GetDayPart,1},
            {ActionType.Turn,7},
            {ActionType.Goto,Height},
        };
        public static Dictionary<ActionType, int> actionBias = new Dictionary<ActionType, int>();

        static Genom()
        {
            OperationsTypeCount = actionCount.Values.Sum();
            actionBias.Add(0, 0);
            for (int i = 1; i < Enum.GetValues(typeof(ActionType)).Length; i++)
            {
                actionBias.Add((ActionType)i, actionBias[(ActionType)(i - 1)] + actionCount[(ActionType)(i - 1)]);
            }
        }

        public Point See(Human human,int n)
        {
            int h = (int)Math.Sqrt(n);//0,1,2,...
            if (h == 0)
            {
                return human.position;
            }
            int k = n - h * h - h;//-h,...,0,...,h
            Point v0 = World.directions[angle];
            Point v1 = new Point();
            v0 = new Point(v0.X * h, v0.Y * h);
            if (k > 0)
            {
                v1 = World.directions[(angle + 1) % 8];
                v1 = new Point(v0.X * h, v0.Y * h);
            }
            else
            {
                v1 = World.directions[(angle + 7) % 8];
                v1 = new Point(v0.X * h, v0.Y * h);
                k = -k;
            }
            Point p = v1 - v0;
            p = human.position + v0 + new Point(p.X / h * k, p.Y / h * k);
            return p;
        }

        public Genom(Random rnd)
        {
            this.rnd = rnd;
            GenerateRandom();
        }
        public Genom(int[,] code,Random rnd)
        {
            this.rnd = rnd;
            this.code = (int[,])code.Clone();
        }
        void GenerateRandom()
        {
            for (int x = 0; x < Width;x++)
            {
                for (int y = 0; y < Height;y++)
                {
                    code[x, y] = Rnd();
                }
            }
        }
        int Rnd()
        {
            return rnd.Next(OperationsTypeCount);
        }
        public Genom Mutate()
        {
            Genom genom = new Genom(code, rnd);
            int x = rnd.Next(Width);
            int y = rnd.Next(Height);
            genom.code[x, y] = Rnd();
            return genom;
        }
        public void Drink(Human human)
        {
            human.Water = World.Watering;
        }
        public void Update(World world,Human human)//OK
        {
            Item item;
            Hand hand;
            bool end = false;
            for (int i = 0;i < maxActionCount && !end; i++)
            {
                int j = Enum.GetValues(typeof(ActionType)).Length - 1;
                while (actionBias[(ActionType)j] > code[x, y]) { j--; }
                int n = code[x, y] - actionBias[(ActionType)j];
                switch ((ActionType)j)
                {
                    case ActionType.Nothing://OK
                        end = true;
                        x++;
                        break;
                    case ActionType.Go://OK
                        end = true;
                        x++;
                        world.Go(human,(angle + n) % 8);
                        break;
                    case ActionType.Take://OK
                        end = true;
                        item = (Item)(n % (itemCount - 1));
                        hand = (Hand)(n / (itemCount - 1));
                        if (world[human.position].CanTake(item) && human.CanAdd(hand, item))
                        {
                            x += 1;
                            human.Add(hand, item);
                            world[human.position].Take(item);
                        }
                        else
                            x += 2;
                        break;
                    case ActionType.Put://OK
                        end = true;
                        if (!human.stuff[n].IsEmpty)
                        {
                            x += 1;
                            world[human.position].Put(human.stuff[n].item);
                            human.Remove((Hand)n);
                        }
                        else
                            x += 2;
                        break;
                    case ActionType.Throw://OK
                        end = true;
                        hand = (Hand)(n / 8);
                        if (!human.stuff[(int)hand].IsEmpty)
                        {
                            x += 1;
                            world.Throw(human.stuff[n].item,human.position,(n + angle) % 8);
                            human.Remove(hand);
                        }
                        else
                            x += 2;
                        break;
                    case ActionType.Eat://OK
                        end = true;
                        if (human.CanEat(n))
                        {
                            human.Eat(n);
                            x++;
                        }
                        else
                            x += 2;
                        break;
                    case ActionType.Sleep://OK
                        end = true;
                        x++;
                        human.sleep = true;
                        break;
                    case ActionType.UseStaticObject://OK
                        end = true;
                        switch (world[human.position].staticObject)
                        {
                            case StaticObject.Nothing:
                                x += 1;
                                break;
                            default:
                                if (n == 0)
                                {
                                    if (usedStaticObject == StaticObject.Nothing)
                                    {
                                        usedStaticObject = world[human.position].staticObject;
                                        x += 2;
                                    }
                                    else
                                    {
                                        x += 3;
                                    }
                                }
                                else
                                {
                                    if (usedStaticObject == StaticObject.Nothing)
                                    {
                                        x += 2;
                                    }
                                    else
                                    {
                                        usedStaticObject = StaticObject.Nothing;
                                        x += 3;
                                    }
                                }
                                break;
                        }
                        break;
                    case ActionType.Fight://OK
                        if (world[human.position].ContainsEnemy())
                        {
                            world[human.position].entities.First(_ => _ is Tiger || _ is Mammont).Hit(human.MeleeDamage);
                            x += 1;
                        }
                        else
                            x += 2;
                        end = true;
                        break;
                    case ActionType.Drink://OK
                        if (world[human.position].landType == LandType.Water)
                        {
                            x += 1;
                            Drink(human);
                        }
                        else
                            x += 2;
                        end = true;
                        break;
                    case ActionType.Shout://OK
                        x += 1;
                        world.Cry(n);
                        end = true;
                        break;
                    case ActionType.SeeLand://OK
                        if (n >= Math.Pow(World.DayPartsSight[world.DayPartNow] + 1,2))
                        {
                            x += 1;
                        }
                        else
                        {
                            Tile tile = world[See(human,n)];
                            switch (tile.landType)
                            {
                                case LandType.Water:
                                    x += 2;
                                    break;
                                case LandType.Grass:
                                    x += 3;
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                        break;
                    case ActionType.SeeStaticObject://OK
                        if (n >= Math.Pow(World.DayPartsSight[world.DayPartNow] + 1, 2))
                        {
                            x += 1;
                        }
                        else
                        {
                            Tile tile = world[See(human, n)];
                            switch (tile.staticObject)
                            {
                                case StaticObject.Nothing:
                                    x += 2;
                                    break;
                                case StaticObject.TreeObject:
                                    x += 3;
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                        break;
                    case ActionType.SeeItem://OK
                        item = (Item)(n / World.sightCount);
                        n %= World.sightCount;
                        if (n >= Math.Pow(World.DayPartsSight[world.DayPartNow] + 1, 2))
                        {
                            x += 1;
                        }
                        else
                        {
                            Tile tile = world[See(human, n)];
                            if (item == Item.Nothing)
                            {
                                if (tile.itemCount.All(_ => _ == 0))
                                    x += 2;
                                else
                                    x += 3;
                            }
                            else
                            {
                                if (tile.itemCount[(int)item - 1] != 0)
                                    x += 2;
                                else
                                    x += 3;
                            }
                        }
                        break;
                    case ActionType.SeeEntity://OK
                        EntityType entity = (EntityType)(n / World.sightCount);
                        n %= World.sightCount;
                        if (n >= Math.Pow(World.DayPartsSight[world.DayPartNow] + 1, 2))
                        {
                            x += 1;
                        }
                        else
                        {
                            Tile tile = world[See(human, n)];
                            switch (entity)
                            {
                                case EntityType.Human:
                                    x += 2 + tile.entities.Count(_ => _ is Human);
                                    break;
                                case EntityType.Tiger:
                                    x += 2 + tile.entities.Count(_ => _ is Tiger);
                                    break;
                                case EntityType.Mammont:
                                    x += 2 + tile.entities.Count(_ => _ is Mammont);
                                    break;
                            }
                        }
                        break;
                    case ActionType.Listen://OK
                        if (world.cryBefore[n])
                            x += 1;
                        else
                            x += 2;
                            break;
                    case ActionType.GetHandType://OK
                        if (human.stuff[n].IsEmpty)
                            x += 1;
                        else
                            x += (int)human.stuff[n].item + 1;
                        break;
                    case ActionType.GetHandCount://OK
                        x += human.stuff[n].count + 1;
                        break;
                    case ActionType.GetDayPart://OK
                        x += (int)world.DayPartNow + 1;
                        break;
                    case ActionType.Turn://OK
                        x++;
                        angle = (angle + (n + 1)) % 8;
                        break;
                    case ActionType.Goto://OK
                        x = 0;
                        y = n;
                        break;
                    default:
                        throw new Exception();
                }
            }
        }
    }
}
