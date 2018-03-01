using System;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class Mammont : Entity
    {
        protected override double MaxHp => 500;
        public Mammont(Point position)
        {
            this.position = position;
        }
        public override Action Update(World world)
        {
            throw new NotImplementedException();
        }
    }
}
