using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public abstract class Entity
    {
        public abstract double MeleeDamage
        {
            get;
        }
        public Point position;
        protected double hp;
        protected double fp;
        protected double HP
        {
            get
            {
                return hp;
            }
            set
            {
                hp = Math.Min(value, MaxHp);
                if (hp <= 0)
                {
                    Die();
                }
            }
        }
        protected double FP
        {
            get
            {
                return fp;
            }
            set
            {
                fp = Math.Min(Math.Max(value,0), MaxFp);
            }
        }
        public World world;
        protected virtual double MaxHp => 0;
        protected virtual double MaxFp => 0;

        public Entity(World world,Point position)
        {
            this.world = world;
            this.position = position;
            HP = MaxHp;
            FP = MaxFp;
        }
        public virtual void Update(World world)
        {
            Hunger();
        }
        public virtual void Hit(double damage)
        {
            HP -= damage;
        }
        public virtual void Die()
        {
            world.Kill(this);
            world[position].entities.Remove(this);
        }
        public abstract void Hunger();
    }
}
