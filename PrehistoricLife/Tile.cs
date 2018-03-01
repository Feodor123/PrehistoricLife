using System;
namespace PrehistoricLife
{
    public struct Tile
    {       
        public LandType landType;
        public int[] itemCount;
        public StaticObject staticObject;
        public Tile(LandType landType,StaticObject staticObject,int[] itemCount)
        {
            this.landType = landType;
            this.itemCount = itemCount;
            this.staticObject = staticObject;
        }
        public bool CanTake(Item item)
        {
            if (item == 0)
            {
                return false;
            }
            return true;
        }
        public void Take(Item item)
        {
            if (itemCount[(int)item] == 0)
            {
                throw new Exception();
            }
            itemCount[(int)item]--;
        }
        public void Put(Item item)
        {
            itemCount[(int)item]++;
        }
    }
}
