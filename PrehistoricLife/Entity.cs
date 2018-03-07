using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public abstract class Entity
    {      
        public Point position;
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
        public abstract void Update(World world);
    }
}
