using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public sealed class Human : Entity
    {
        protected override double MaxHp => 100;
        public Genom genom;
        public HandStuff[] stuff;
        public int[] AllItem
        {
            get
            {
                int[] item = new int[Enum.GetValues(typeof(Item)).Length - 1];
                foreach (var hs in stuff)
                {
                    if (!hs.IsEmpty)
                    {
                        item[(int)hs.item - 1] += hs.count;
                    }
                }
                return item;
            }
        }
        static int[] HutBuildItem = new int[] {1,0,0,1};
        public Human(Point position,Genom genom)
        {
            stuff = new HandStuff[Enum.GetValues(typeof(Hand)).Length];
            this.genom = genom;
            hp = MaxHp;
            this.position = position;
        }
        public override void Update(World world)
        {
            genom.Update(world, this);
        }
        public struct HandStuff
        {
            public Item item;
            public int count;
            public bool IsEmpty
            {
                get
                {
                    return count == 0;
                }
            }
            public bool IsFood
            {
                get
                {
                    return item == Item.Food;
                }
            }

            public void Add(Item item)
            {
                if (this.item != item && count != 0)
                {
                    throw new Exception();
                }
                count++;
            }
            public void Remove(Item item)
            {
                if (this.item != item || count == 0)
                {
                    throw new Exception();
                }
                count--;
            }
            public void Add(Item item,int count)
            {
                if (this.item != item && this.count != 0)
                {
                    throw new Exception();
                }
                this.count += count;
            }
            public void Remove(Item item,int count)
            {
                if (this.item != item || this.count < count)
                {
                    throw new Exception();
                }
                this.count -= count;
            }

            public bool CanAdd(Item item)
            {
                if (this.item != item && count != 0)
                {
                    return false;
                }
                return true;
            }
            public bool CanRemove(Item item)
            {
                if (this.item != item || count == 0)
                {
                    return false;
                }
                return true;
            }
            public bool CanRemove(Item item, int count)
            {
                if (this.item != item || this.count < count)
                {
                    return false;
                }
                return true;
            }
        }
        public bool CanRemove(Hand hand, Item item,int count)
        {
            return stuff[(int)hand].CanRemove(item, count);
        }
        public bool CanAdd(Hand hand, Item item)
        {
            return stuff[(int)hand].CanAdd(item);
        }
        public bool CanRemove(Hand hand, Item item)
        {
            return stuff[(int)hand].CanRemove(item);
        }

        public void Add(Hand hand,Item item,int count)
        {
            stuff[(int)hand].Add(item, count);
        }
        public void Remove(Hand hand, Item item, int count)
        {
            stuff[(int)hand].Remove(item, count);
        }
        public void Add(Hand hand, Item item)
        {
            stuff[(int)hand].Add(item);
        }
        public void Remove(Hand hand, Item item)
        {
            stuff[(int)hand].Remove(item);
        }

        public bool CanBuild()//пока только шалаш
        {
            int[] all = AllItem;
            for (int i = 0;i < all.Length; i++)
            {
                if(all[i] < HutBuildItem[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
