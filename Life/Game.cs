using System;
using System.Data;
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
        /// The amount of money the player has.
        /// </summary>
        public int Money { get; private set; }
        /// <summary>
        /// The amount of the player's life enjoyment. You want to optimize this.
        /// </summary>
        public double LifeEnjoyment { get; private set; }
        /// <summary>
        /// Represents the choices made by the player.
        /// </summary>
        public List<Choice> Choices { get; private set; }
        /// <summary>
        /// The maximum number of periods in the game.
        /// </summary>
        private readonly uint MaxPeriods;
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
        /// The dimensions of the field.
        /// </summary>
        private readonly uint m, n;
        /// <summary>
        /// The number of successful harvesting points over the field.
        /// </summary>
        private readonly uint t;
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
        public Game(uint periods = 10, float defaultHealth = 70, uint m = 10, uint n = 10, uint t = 100, int v = 1, double g = 1, double k = 0.01021, double c = 464.53, double a = 32, List<Choice> choices = null)
        {
            Health = defaultHealth;
            MaxPeriods = periods;
            Period = 0;
            this.k = k;
            this.g = g;
            this.v = v;
            this.c = c;
            this.a = a;
            this.Choices = choices;
        }
        /// <summary>
        /// Regenerates health.
        /// </summary>
        /// <param name="I">The health investment.</param>
        /// <param name="H">The health after harvesting.</param>
        private void RegenHealth(double I) => Health += 100 * (Math.Pow(Math.E, k * I) / (Math.Pow(Math.E, k * I) + ((100 - Health) / Health))) - Health;
        /// <summary>
        /// Decrement health at the start of each turn.
        /// </summary>
        private void DecrementHealth() => Health -= 10 + Period;
        /// <summary>
        /// Harvest the field.
        /// </summary>
        /// <returns>
        /// An integer representing currency gleaned.
        /// </returns>
        private int Harvest()
        {
            uint t = this.t;
            DataTable Field = new DataTable();
            for (int i = 0; i < n; i++)
            {
                Field.Columns.Add(new DataColumn(i.ToString(), typeof(bool)));
            }
            for (int i = 0; i < m; i++)
            {
                Field.Rows.Add(Field.NewRow());
            }
            Random random = new Random();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (t > 0 && random.Next(0, 2) == 1)
                    {
                        Field.Rows[i][j] = true;
                        t--;
                    }
                    else { Field.Rows[i][j] = false; }
                }
            }

            int harvestRows = (int)(((float)Field.Columns.Count) * (1 - (g * (100 - Health) / 100)));
            int harvestTotal = 0;
            for (int i = 0; i < (harvestRows < Field.Rows.Count ? harvestRows : Field.Rows.Count); i++)
            {
                for (int j = 0; j < Field.Columns.Count; j++)
                {
                    harvestTotal += (bool)Field.Rows[i][j] == true ? v : 0;
                }
            }

            return harvestTotal;
        }
    }

    /// <summary>
    /// Represents the amounts invested into life and health investments at each turn.
    /// </summary>
    /// <remarks>
    /// A collection of these choices forms the chromosomes.
    /// </remarks>
    public class Choice
    {
        public uint LifeInvestment;
        public uint HealthInvestment;
    }
}
