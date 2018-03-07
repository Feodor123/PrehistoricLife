using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class Simulation
    {
        public static Point[] directions = new Point[]
        {
            new Point(-1,0),
            new Point(-1,1),
            new Point(0,1),
            new Point(1,1),
            new Point(1,0),
            new Point(1,-1),
            new Point(0,-1),
            new Point(-1,-1),
        };
        const int startHumanCount = 5;
        Random rnd = new Random();
        World world;
        List<Genom> genoms;
        List<Human> humans;
        public Simulation()
        {
            world = new World(100,100,rnd);
            while (genoms.Count < startHumanCount)
            {
                genoms.Add(new Genom(rnd));
            }
        }
    }
}
