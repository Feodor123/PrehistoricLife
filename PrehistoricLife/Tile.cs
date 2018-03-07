using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public struct Tile
    {       
        public LandType landType;
        public int[] itemCount;
        public StaticObject staticObject;
        public List<Entity> entities;
        public Tile(LandType landType,StaticObject staticObject,int[] itemCount)
        {
            entities = new List<Entity>();
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
        public bool ContainsEnemy()
        {
            return entities.Exists(_ => _ is Tiger || _ is Mammont);
        }
    }
}
