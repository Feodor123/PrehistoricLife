using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class Genom
    {
        Random rnd;
        static int OperationsTypeCount;
        static int handCount = Enum.GetNames(typeof(Hand)).Length;
        const int maxActionCount = 50;
        const int Height = 30;
        const int Width = 50;
        int angle = 0;
        int x = 0;
        int y = 0;
        int[,] code = new int[Height,Width];

        public static Dictionary<ActionType, int> actionCount = new Dictionary<ActionType, int>()
        {
            {ActionType.Nothing,1},
            {ActionType.Go,8},
            {ActionType.Take,handCount * (Enum.GetValues(typeof(Item)).Length - 1)},//брать ничто нельзя
            {ActionType.Put,handCount},
            {ActionType.Throw,handCount},
            {ActionType.Eat,handCount},
            {ActionType.Sleep,1},
            {ActionType.UseStaticObject,1},
            {ActionType.Build,1},//пока только шалаш
            {ActionType.Fight,1},
            {ActionType.Drink,1},
            {ActionType.Shout,World.WordCount},
            {ActionType.SeeStaticObject,World.sightCount},//под себя тоже можно
            {ActionType.SeeLand,World.sightCount},//под себя тоже можно
            {ActionType.SeeItem,World.sightCount * (Enum.GetValues(typeof(Item)).Length)},//проверка на соответствующий item (или на полное отстствие)
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
            int k = n - h * h - h;//-h,...,0,...,h
            Point v0 = Simulation.directions[angle];
            Point v1 = new Point();
            v0 = new Point(v0.X * h, v0.Y * h);
            if (k > 0)
            {
                v1 = Simulation.directions[(angle + 1) % 8];
                v1 = new Point(v0.X * h, v0.Y * h);
            }
            else
            {
                v1 = Simulation.directions[(angle + 7) % 8];
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
        public void Mutate()
        {
            int x = rnd.Next(Width);
            int y = rnd.Next(Height);
            code[y, x] = Rnd();
        }

        public Operation Update(World world,Human human)
        {
            bool end = false;
            for (int i = 0;i < maxActionCount && !end; i++)
            {
                int j = Enum.GetValues(typeof(ActionType)).Length - 1;
                while (actionBias[(ActionType)j] > code[y, x]) { j--; }
                int n = code[y, x] - actionBias[(ActionType)j];
                switch ((ActionType)j)
                {
                    case ActionType.Nothing:
                        end = true;
                        x++;
                        return new Operation(OperationType.Nothing);
                    case ActionType.Go:
                        end = true;
                        x++;//нафиг перескоки
                        return new Operation(OperationType.Go,n);
                    case ActionType.Take:
                        end = true;
                        if (world[human.position].CanTake((Item)((n % handCount) + 1)) && human.CanAdd((Hand)(n/handCount), (Item)((n % handCount) + 1)))
                            x += 1;
                        else
                            x += 2;
                        return new Operation(OperationType.Take, n);
                    case ActionType.Put:
                        end = true;
                        if (!human.stuff[n].IsEmpty)
                            x += 1;
                        else
                            x += 2;
                        return new Operation(OperationType.Put, n);
                    case ActionType.Throw:
                        end = true;
                        if (!human.stuff[n].IsEmpty)
                            x += 1;
                        else
                            x += 2;
                        return new Operation(OperationType.Throw, n);
                    case ActionType.Eat:
                        end = true;
                        if (!human.stuff[n].IsEmpty && human.stuff[n].IsFood)
                            x += 1;
                        else
                            x += 2;
                        return new Operation(OperationType.Eat, n);
                    case ActionType.Sleep:
                        end = true;
                        x++;
                        return new Operation(OperationType.Sleep);
                    case ActionType.UseStaticObject:
                        end = true;
                        switch (world[human.position].staticObject)
                        {
                            case StaticObject.Nothing:
                                x += 1;
                                break;
                            case StaticObject.TreeObject:
                                x += 2;
                                break;
                            case StaticObject.Bush:
                                x += 3;
                                break;
                            case StaticObject.Hut:
                                x += 4;
                                break;
                        }
                        return new Operation(OperationType.UseStaticObject);
                    case ActionType.Build:
                        if (human.CanBuild())
                            x += 1;
                        else
                            x += 2;
                        end = true;
                        return new Operation(OperationType.Build);
                    case ActionType.Fight:
                        if (world[human.position].ContainsEnemy())
                            x += 1;
                        else
                            x += 2;
                        end = true;
                        return new Operation(OperationType.Fight);
                    case ActionType.Drink:
                        if (world[human.position].landType == LandType.Water)
                            x += 1;
                        else
                            x += 2;
                        end = true;
                        return new Operation(OperationType.Drink);
                    case ActionType.Shout:
                        x += 1;
                        end = true;
                        return new Operation(OperationType.Shout,n);
                    case ActionType.SeeLand:
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
                    case ActionType.SeeStaticObject:
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
                                case StaticObject.Bush:
                                    x += 4;
                                    break;
                                case StaticObject.Hut:
                                    x += 5;
                                    break;
                                default:
                                    throw new Exception();
                            }
                        }
                        break;
                    case ActionType.SeeItem:
                        Item item = (Item)(n / World.sightCount);
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
                                if (tile.itemCount[(int)item] != 0)
                                    x += 2;
                                else
                                    x += 3;
                            }
                        }
                        break;
                    case ActionType.SeeEntity:
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
                                    x += 1 + tile.entities.Count(_ => _ is Human);
                                    break;
                                case EntityType.Tiger:
                                    x += 1 + tile.entities.Count(_ => _ is Tiger);
                                    break;
                                case EntityType.Mammont:
                                    x += 1 + tile.entities.Count(_ => _ is Mammont);
                                    break;
                            }
                        }
                        break;
                    case ActionType.Listen:
                        if (world.cry[n])
                            x += 1;
                        else
                            x += 2;
                            break;
                    case ActionType.GetHandType:
                        if (human.stuff[n].IsEmpty)
                            x += 1;
                        else
                            x += (int)human.stuff[n].item + 1;
                        break;
                    case ActionType.GetHandCount:
                        x += human.stuff[n].count;
                        break;
                    case ActionType.GetDayPart:
                        x += (int)world.DayPartNow + 1;
                        break;
                    case ActionType.Turn:
                        x++;
                        angle = (angle + (n + 1)) % 8;
                        break;
                    case ActionType.Goto:
                        x = 0;
                        y = n;
                        break;
                    default:
                        throw new Exception();
                }
            }
            return new Operation(OperationType.Nothing);
        }
        Dictionary<ActionType,Action> Do = new Dictionary<ActionType, Action>()
        {
            {ActionType.Build,_ => _},
        }
    }
}
