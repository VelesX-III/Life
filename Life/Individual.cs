using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Life
{
    abstract class Individual
    {
        public IList<int> Chromosomes { get; protected set; }
        public abstract float FitnessScore();
        public abstract Individual Cross(Individual otherParent);
        public abstract Individual Mutate();
    }
}
