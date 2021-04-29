using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class RecipeTechCard
    {
        public int RId { get; set; }

        public int TCId { get; set; }

        public virtual TechCard TechCard { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
