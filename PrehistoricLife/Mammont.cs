using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public override Operation Update(World world)
        {
            throw new NotImplementedException();
        }
    }
}
