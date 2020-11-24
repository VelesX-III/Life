using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    public class Game
    {
        public int Period { get; private set; }
        public int Health { get; private set; }
        private readonly float k;
        private readonly float g; //Gamma
        private readonly int v;
        public bool[,] Field { get; private set; }

        public Game(int m, int n, int t, int v, float g, float k)
        {
            Field = new bool[m, n];
            Random random = new Random();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Field[i, j] = random.Next(0, 2) == 1 ? true : false;
                }
            }
            this.k = k;
            this.g = g;
            this.v = v;
        }
    }
}
