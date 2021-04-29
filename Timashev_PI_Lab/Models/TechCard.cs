using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class TechCard
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public virtual List<RecipeTechCard> RecipeTechCards { get; set; }
    }
}
