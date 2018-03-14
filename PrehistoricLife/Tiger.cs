using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class Tiger : Entity
    {
        public override double MeleeDamage
        {
            get
            {
                return 20;
            }
        }
        protected override double MaxHp => 150;
        private static readonly int[] drop;
        public override void Die()
        {
            base.Die();
            world[position].Drop(drop);
        }
        static Tiger()
        {
            drop = new int[Enum.GetValues(typeof(Item)).Length - 1];
            drop[(int)Item.Food - 1] += 1;
        }
        public Tiger(World world,Point position) : base(world, position)
        {

        }
        public override void Update(World world)
        {
            base.Update(world);
        }
        public override void Hunger()
        {
            FP--;
            if (FP > MaxFp / 2)
            {
                HP += 2;
            }
            else if (FP == 0)
            {
                HP -= 2;
            }
        }
    }
}
