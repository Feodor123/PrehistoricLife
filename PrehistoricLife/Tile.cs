using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PrehistoricLife
{
    public class Tile
    {       
        public LandType landType;
        public int[] itemCount;
        public StaticObject staticObject;
        public List<Entity> entities;
        public Tile(LandType landType,StaticObject staticObject,int[] itemCount)
        {
            entities = new List<Entity>();
            itemCount = new int[Enum.GetValues(typeof(Item)).Length - 1];
            this.landType = landType;
            this.itemCount = itemCount;
            this.staticObject = staticObject;
        }
        public bool CanTake(Item item)
        {
            if (itemCount[(int)item] == 0)
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
        public void Draw(SpriteBatch spriteBatch, Texture2D atlasTexture, Point position, int tileSize,Color color)
        {
            Rectangle r;
            switch (landType)
            {
                case LandType.Grass:
                    r = new Rectangle(0, 0, 64, 64);
                    break;
                case LandType.Water:
                    r = new Rectangle(64, 0, 64, 64);
                    break;
                default:
                    throw new Exception();
            }
            spriteBatch.Draw(atlasTexture,new Rectangle(position,new Point(tileSize, tileSize)),r, color);
            if (staticObject == StaticObject.TreeObject)
            {
                spriteBatch.Draw(atlasTexture,new Rectangle(position,new Point(tileSize, tileSize)),new Rectangle(64,64,64,64), color);
            }
        }
        public void Drop(int[] item)
        {
            for (int i = 0;i < Enum.GetValues(typeof(Item)).Length - 1; i++)
            {
                itemCount[i] += item[i];
            }
        }
    }
}
