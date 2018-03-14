using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class Mammont : Entity
    {
        public override double MeleeDamage
        {
            get
            {
                return 40;
            }
        }
        protected override double MaxHp => 500;
        private static readonly int[] drop;
        public override void Die()
        {
            base.Die();
            world[position].Drop(drop);
        }
        static Mammont()
        {
            drop = new int[Enum.GetValues(typeof(Item)).Length - 1];
            drop[(int)Item.Food - 1] += 6;
        }
        public Mammont(World world, Point position) : base(world, position)
        {

        }
        public override void Update(World world)
        {
            base.Update(world);
        }
        public override void Hunger()
        {
            HP++;
        }
    }
}
