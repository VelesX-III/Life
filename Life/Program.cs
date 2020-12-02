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
            List<Game> Individuals = new List<Game>();
            for (int i = 0; i < 100; i++)
            {
                Individuals.Add(new Game().Play());
            }

            Individuals.ForEach(g => Console.WriteLine(g.LifeEnjoyment));
            Console.ReadLine();
        }
    }
}
