using System;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public abstract class Entity
    {
        protected Point position;
        protected double hp; 
        double HP
        {
            get
            {
                return hp;
            }  
            set
            {
                hp = Math.Min(value, MaxHp);
            }
        }
        protected virtual double MaxHp => 0;
        public abstract Action Update(World world);
    }
}
