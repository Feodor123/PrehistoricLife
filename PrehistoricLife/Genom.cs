using System;
namespace PrehistoricLife
{
    public class Genom
    {
        Random rnd;
        const int OperationsTypeCount = 100;
        const int Height = 10;
        const int Width = 50;
        int[,] code = new int[Height,Width];
        public Genom(Random rnd)
        {
            this.rnd = rnd;
            GenerateRandom();
        }
        void GenerateRandom()
        {
            for (int x = 0; x < Width;x++)
            {
                for (int y = 0; y < Height;y++)
                {
                    code[x, y] = rnd.Next(OperationsTypeCount);
                }
            }
        }
    }
}
