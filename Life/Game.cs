using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    /// <summary>
    /// Represets a single game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The current period of the game.
        /// </summary>
        public uint Period { get; private set; }
        /// <summary>
        /// The player's current amount of health.
        /// </summary>
        public double Health { get; private set; }
        /// <summary>
        /// The amount of the player's life enjoyment. You want to optimize this.
        /// </summary>
        public double LifeEnjoyment { get; private set; }
        /// <summary>
        /// The constant <i>k</i>, which affects health regeneration.
        /// </summary>
        private readonly double k;
        /// <summary>
        /// The constant <i>γ</i>, which affects harvesting.
        /// </summary>
        private readonly double g;
        /// <summary>
        /// The constant <i>v</i>, which is the value of a successful harvesting attempt.
        /// </summary>
        private readonly int v;
        /// <summary>
        /// The constant <i>c</i>, which affects life enjoyment.
        /// </summary>
        private readonly double c;
        /// <summary>
        /// The constant <i>α</i>, which affects life enjoyment.
        /// </summary>
        private readonly double a;
        /// <summary>
        /// The harvesting field, represented as a multidimensional array.
        /// You get <i>v</i> points of life enjoyment for a successful harvest,
        /// at one of the <i>t</i> points scattered across the field.
        /// </summary>
        public bool[,] Field { get; private set; }
        /// <summary>
        /// Constructs a new game with the default parameters. You can override these.
        /// </summary>
        /// <param name="periods">Number of periods to begin the game with.</param>
        /// <param name="defaultHealth">Starting amount of health to begin the game with.</param>
        /// <param name="m">Number of rows of the field.</param>
        /// <param name="n">Number of columns in the field.</param>
        /// <param name="t">Number of successful harvest points dispersed across the field.</param>
        /// <param name="v">Value of life enjoyment for a successful harvest.</param>
        /// <param name="g">Affects harvesting.</param>
        /// <param name="k">Affects health regen.</param>
        /// <param name="c">Affects life enjoyment.</param>
        /// <param name="a">Affects life enjoyment.</param>
        public Game(uint periods = 10, float defaultHealth = 70, int m = 10, int n = 10, int t = 100, int v = 1, double g = 1, double k = 0.01021, double c = 464.53, double a = 32)
        {
            Health = defaultHealth;
            Period = periods;
            this.k = k;
            this.g = g;
            this.v = v;
            this.c = c;
            this.a = a;
            Field = new bool[m, n];
            Random random = new Random();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (t > 0 && random.Next(0, 2) == 1)
                    {
                        Field[i, j] = true;
                        t--;
                    }
                }
            }
        }
        /// <summary>
        /// Regenerates health.
        /// </summary>
        /// <param name="I">The health investment.</param>
        /// <param name="H">The health after harvesting.</param>
        private void HealthRegen(double I, double H) => Health += 100 * (Math.Pow(Math.E, k * I) / (Math.Pow(Math.E, k * I) + ((100 - H) / H))) - H;
    }
}
