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
            const int population = 100;
            const int iterations = 1000;
            const double mutationRate = .1;
            const double permutationRate = .5;
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
                    //Create children from two parents.
                    List<Choice>[] childChoices = { new List<Choice>(), new List<Choice>() };
                    childChoices[0].AddRange(individuals[j].Choices.GetRange(0, 5));
                    childChoices[0].AddRange(individuals[j + 1].Choices.GetRange(5, 5));
                    childChoices[1].AddRange(individuals[j].Choices.GetRange(5, 5));
                    childChoices[1].AddRange(individuals[j + 1].Choices.GetRange(0, 5));
                    //Roll for mutation.
                    if (random.NextDouble() < mutationRate)
                    {
                        foreach (Choice choice in childChoices[0])
                        {
                            if (random.NextDouble() < permutationRate)
                            {
                                double healthInvestment = choice.HealthInvestment;
                                double lifeInvestment = choice.LifeInvestment;
                                choice.HealthInvestment = lifeInvestment;
                                choice.LifeInvestment = healthInvestment;
                            }
                        }
                    }
                    if (random.NextDouble() < mutationRate)
                    {
                        foreach (Choice choice in childChoices[1])
                        {
                            if (random.NextDouble() < permutationRate)
                            {
                                double healthInvestment = choice.HealthInvestment;
                                double lifeInvestment = choice.LifeInvestment;
                                choice.HealthInvestment = lifeInvestment;
                                choice.LifeInvestment = healthInvestment;
                            }
                        }
                    }
                    children.Add(new Game(choices: childChoices[0]).Play());
                    children.Add(new Game(choices: childChoices[1]).Play());
                }
                individuals.AddRange(children);
                individuals = individuals.OrderByDescending(g => g.LifeEnjoyment).Take(population).ToList();
                Console.WriteLine(individuals.First().LifeEnjoyment);
            }
            Console.ReadLine();
        }
    }
}
