using System;
using System.Collections.Generic;
namespace PrehistoricLife
{
    public class Simulation
    {
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
