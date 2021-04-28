using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class ProductRecipe
    {
        public int PId { get; set; }

        public int RId { get; set; }

        public string Percent { get; set; }

        public virtual Product Product { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
