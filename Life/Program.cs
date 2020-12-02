using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Game> individuals = new List<Game>();
            int population = 10;
            int iterations = 100000;
            Random random = new Random();
            for (int i = 0; i < population; i++)
            {
                individuals.Add(new Game().Play());
            }
            individuals = individuals.OrderByDescending(g => g.LifeEnjoyment).ToList();

            for (int i = 0; i < iterations; i++)
            {
                List<Game> children = new List<Game>();
                for (int j = 0; j < individuals.Count - 1; j += 2)
                {
                    //Create a child from two parents.
                    Game child = new Game(choices: individuals[j].Choices.GetRange(0, 5).Union(individuals[j + 1].Choices.GetRange(5, 5)).ToList());
                    //Roll for mutation.
                    if (random.NextDouble() < .5)
                    {
                        int randomChromosome = random.Next(0, child.Choices.Count);
                        child.Choices[randomChromosome].MoneySpent += child.Choices[randomChromosome].MoneySpent * random.Next(-1, 2) * random.NextDouble();
                        child.Choices[randomChromosome].HealthInvestment += child.Choices[randomChromosome].HealthInvestment * random.Next(-1, 2) * random.NextDouble();
                        child.Choices[randomChromosome].LifeInvestment += child.Choices[randomChromosome].LifeInvestment * random.Next(-1, 2) * random.NextDouble();
                    }
                    children.Add(child.Play());
                }
                individuals = individuals.Union(children).OrderByDescending(g => g.LifeEnjoyment).Take(population).ToList();
                Console.WriteLine(individuals.First().LifeEnjoyment);
            }

            //individuals.ForEach(g => Console.WriteLine(g.LifeEnjoyment));
            Console.ReadLine();
        }
    }
}
