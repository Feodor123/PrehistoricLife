using System;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public sealed class Human : Entity
    {
        protected override double MaxHp => 100;
        public Genom genom;
        public HandStuff[] stuff;
        public Human(Point position,Genom genom)
        {
            stuff = new HandStuff[Enum.GetValues(typeof(Hand)).Length];
            this.genom = genom;
            hp = MaxHp;
            this.position = position;
        }
        public override Action Update(World world)
        {
            throw new NotImplementedException();
        }
        public struct HandStuff
        {
            private Item item;
            private int count;
            public bool Add(Item item)
            {
                if (this.item != item && count != 0)
                {
                    return false;
                }
                count++;
                this.item = item;
                return true;
            }
            public bool Remove(Item item)
            {
                if (this.item != item || count == 0)
                {
                    return false;
                }
                count--;
                return true;
            }
            public bool Add(Item item,int count)
            {
                if (this.item != item && this.count != 0)
                {
                    return false;
                }
                this.count += count;
                this.item = item;
                return true;
            }
            public bool Remove(Item item,int count)
            {
                if (this.item != item || this.count < count)
                {
                    return false;
                }
                this.count -= count;
                return true;
            }
        }
        public bool Add(Hand hand,Item item,int count)
        {
            return stuff[(int)hand].Add(item, count);
        }
        public bool Remove(Hand hand, Item item, int count)
        {
            return stuff[(int)hand].Remove(item, count);
        }
        public bool Add(Hand hand, Item item)
        {
            return stuff[(int)hand].Add(item);
        }
        public bool Remove(Hand hand, Item item)
        {
            return stuff[(int)hand].Remove(item);
        }
    }
}
