using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
namespace PrehistoricLife
{
    public class Simulation
    {
        const int StartHumanCount = 20;
        const int EndHumanCount = 5;
        public TimeSpan speed = TimeSpan.FromMilliseconds(100);
        public TimeSpan lastUpdate;
        public World world;
        public List<int> worldsLifeTimes;

        private Random rnd;
        private List<Genom> genoms;

        public Simulation()
        {
            worldsLifeTimes = new List<int>();
            rnd = new Random();
            genoms = new List<Genom>();
            while (genoms.Count < StartHumanCount)
            {
                genoms.Add(new Genom(rnd));                
            }
            world = new World(rnd, 0,genoms);
        }
        public void Go(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > lastUpdate + speed)
            {
                lastUpdate = gameTime.TotalGameTime;
                world.Update();
                if (world.HumanCount <= EndHumanCount)
                {
                    worldsLifeTimes.Add(world.tick);
                    genoms = world.Genoms;
                    while(genoms.Count < EndHumanCount)
                    {
                        genoms.Add(new Genom(rnd));
                    }
                    for (int i = EndHumanCount;i < StartHumanCount; i++)
                    {
                        genoms.Add(genoms[i % EndHumanCount].Mutate());
                    }
                    world = new World(rnd,world.number + 1, genoms);
                }
            }
        }
    }
}
