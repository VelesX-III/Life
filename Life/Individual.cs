using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    abstract class Individual
    {
        /// <summary>
        /// Presumably the choices made by the player?
        /// </summary>
        public IEnumerable<int> Chromosomes { get; protected set; }
        /// <summary>
        /// The fitness score function of the GA.
        /// </summary>
        /// <returns>A real number measuring the fitness of the GA. Presumably the same as total life enjoyment.</returns>
        public abstract float FitnessScore();
        /// <summary>
        /// Gets the child of the current individual and the individual specified by the parameter.
        /// </summary>
        /// <param name="otherParent">The individual to cross with.</param>
        /// <returns>The child individual.</returns>
        public abstract Individual Cross(Individual otherParent);
        /// <summary>
        /// Mutates the current individual, shuffling its chromosomes.
        /// </summary>
        public abstract void Mutate();
    }
}
