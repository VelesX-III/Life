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
        public int Period { get; private set; }
        /// <summary>
        /// The player's current amount of health.
        /// </summary>
        public double Health { get => _Health; private set => _Health = value > 0 && _Health > 0 ? value : 0; }
        /// <summary>
        /// Backing for Health.
        /// </summary>
        private double _Health = 1;
        /// <summary>
        /// The amount of money the player has.
        /// </summary>
        public double Money { get => _Money; private set => _Money = value < 0 ? 0 : value; }
        /// <summary>
        /// Backing for Money.
        /// </summary>
        private double _Money = 0;
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
        /// The random number generator to use throughout the all instances of the game.
        /// </summary>
        private static readonly Random random;
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
        private readonly double v;
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
        /// Static constructor to make sure that all instances share the same RNG seed.
        /// </summary>
        static Game()
        {
            random = new Random();
        }
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
        /// <param name="choices">A list containing the choices to make in each turn.</param>
        public Game(
            uint periods = 8,
            float defaultHealth = 60,
            uint m = 10,
            uint n = 10,
            uint t = 10,
            int v = 3,
            double g = 1,
            double k = .007,
            double c = 700,
            double a = 45,
            List<Choice> choices = null)
        {
            Health = defaultHealth;
            MaxPeriods = periods;
            this.k = k;
            this.g = g;
            this.v = v;
            this.c = c;
            this.a = a;
            this.m = m;
            this.n = n;
            this.t = t;
            if ((choices is null) || choices?.Count == MaxPeriods) { Choices = choices; }
            else { throw new Exception("The number of choices doesn't match the number of turns."); }
        }
        /// <summary>
        /// Harvest the field.
        /// </summary>
        private void Harvest()
        {
            DataTable Field = new DataTable();
            for (int i = 0; i < n; i++)
            {
                Field.Columns.Add(new DataColumn(i.ToString(), typeof(bool)));
            }
            for (int i = 0; i < m; i++)
            {
                Field.Rows.Add(Field.NewRow());
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (random.NextDouble() <= ((double)t) / (((double)m) * ((double)n)))
                    {
                        Field.Rows[i][j] = true;
                    }
                    else { Field.Rows[i][j] = false; }
                }
            }

            int harvestRows = (int)(((float)Field.Columns.Count) * (1 - (g * (100 - Health) / 100)));
            double harvestTotal = 0;
            for (int i = 0; i < (harvestRows < Field.Rows.Count ? harvestRows : Field.Rows.Count); i++)
            {
                for (int j = 0; j < Field.Columns.Count; j++)
                {
                    harvestTotal += (bool)Field.Rows[i][j] == true ? v : 0;
                }
            }

            Money += harvestTotal;
            Health -= 15 + 4 * Period;
        }
        /// <summary>
        /// Regenerates health.
        /// </summary>
        /// <param name="I">The health investment.</param>
        private void RegenHealth(double I)
        {
            Health += Math.Floor(100 * (Math.Pow(Math.E, k * I) / (Math.Pow(Math.E, k * I) + ((100 - Health) / Health))) - Health);
            Money -= I;
        }
        /// <summary>
        /// Generate life enjoyment.
        /// </summary>
        /// <param name="L">The life enjoyment investment.</param>
        private void GenerateLifeEnjoyment(double L)
        {
            LifeEnjoyment += Health > 0 ? c * (Health / 100) * (L / (L + a)) : 0;
            Money -= L;
        }
        /// <summary>
        /// Play the current instance of the game.
        /// </summary>
        /// <returns>The amount of life enjoyment accrued.</returns>
        public Game Play()
        {
            if (Period == 0)
            {
                if (Choices is null)
                {
                    Choices = new List<Choice>();
                    for (Period = 0; Period < MaxPeriods; Period++)
                    {
                        Harvest();
                        double spendingMoney = random.NextDouble() * Money;
                        double healthInvestment = random.NextDouble() * spendingMoney;
                        Choices.Add(new Choice
                        {
                            MoneySpent = spendingMoney / Money,
                            HealthInvestment = healthInvestment / spendingMoney,
                            LifeInvestment = (spendingMoney - healthInvestment) / spendingMoney
                        });
                        RegenHealth(healthInvestment);
                        GenerateLifeEnjoyment(spendingMoney - healthInvestment);
                    }
                }
                else
                {
                    for (Period = 0; Period < MaxPeriods; Period++)
                    {
                        Harvest();
                        double spendingMoney = Choices[Period].MoneySpent * Money;
                        double healthInvestment = Choices[Period].HealthInvestment * spendingMoney;
                        double lifeInvestment = Choices[Period].LifeInvestment * spendingMoney;
                        RegenHealth(healthInvestment);
                        GenerateLifeEnjoyment(lifeInvestment);
                    }
                }
            }

            return this;
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
        /// <summary>
        /// The ratio of money invested to money owned.
        /// </summary>
        public double MoneySpent;
        /// <summary>
        /// The ratio of money spent on life enjoyment of money spent;
        /// </summary>
        public double LifeInvestment;
        /// <summary>
        /// The ratio of money spent on health improvement of money spent.
        /// </summary>
        public double HealthInvestment;
    }
}
