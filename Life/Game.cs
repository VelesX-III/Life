using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    public class Game
    {
        /// <summary>
        /// The current period of the game.
        /// </summary>
        public int Period { get; private set; }
        /// <summary>
        /// The player's current amount of health.
        /// </summary>
        public int Health { get; private set; }
        /// <summary>
        /// The constant k, which affects health regeneration.
        /// </summary>
        private readonly float k;
        /// <summary>
        /// The constant gamma, which affects harvesting.
        /// </summary>
        private readonly float g;
        /// <summary>
        /// The constant v, which is the value of a successful harvesting attempt.
        /// </summary>
        private readonly int v;
        public bool[,] Field { get; private set; }
        public Game(int periods, int defaultHealth,int m, int n, int t, int v, float g, float k)
        {
            Health = defaultHealth;
            Period = periods;
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
