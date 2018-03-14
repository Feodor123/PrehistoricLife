using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public sealed class Human : Entity
    {
        public override double MeleeDamage
        {
            get
            {
                return 15;
            }
        }
        protected override double MaxHp => 100;
        protected override double MaxFp => 100;
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
        public Human(Point position,Genom genom,World world) : base(world,position)
        {
            stuff = new HandStuff[Enum.GetValues(typeof(Hand)).Length];
            this.genom = genom;
            hp = MaxHp;            
        }
        public bool sleep;
        private int water;
        public int Water
        {
            get
            {
                return water;
            }
            set
            {
                if (value < 0)
                {
                    water = 0;
                }
                water = value;
            }
        }

        public override void Update(World world)
        {
            base.Update(world);
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
            public void Remove()
            {
                if (count == 0)
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
            public void Remove(int count)
            {
                if (this.count < count)
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
        public void Remove(Hand hand, int count)
        {
            stuff[(int)hand].Remove(count);
        }
        public void Add(Hand hand, Item item)
        {
            stuff[(int)hand].Add(item);
        }
        public void Remove(Hand hand)
        {
            stuff[(int)hand].Remove();
        }
        
        public bool CanEat(int hand)
        {
            return !stuff[hand].IsEmpty && stuff[hand].IsFood;
        }
        public void Eat(int hand)
        {
            stuff[hand].Remove();
            HP += World.FoodNutrition;
        }
        public override void Die()
        {
            base.Die();
            world[position].Drop(AllItem);
        }
        public override void Hunger()
        {
            FP--;
            Water--;
            if (FP > MaxFp/2)
            {
                HP++;
                if (Water > 0)
                {
                    HP++;
                }
            }
            else if (FP == 0)
            {
                HP--;
            }
        }
    }
}
