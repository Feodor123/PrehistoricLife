using System;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class Tiger : Entity
    {
        protected override double MaxHp => 150;
        public Tiger(Point position)
        {
            this.position = position;
        }
        public override Action Update(World world)
        {
            throw new NotImplementedException();
        }
    }
}
