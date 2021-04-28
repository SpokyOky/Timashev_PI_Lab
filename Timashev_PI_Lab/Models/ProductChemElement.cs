using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class ProductChemElement
    {
        public string Percent { get; set; }

        public int PId { get; set; }

        public int CEId { get; set; }

        public virtual Product Product { get; set; }

        public virtual ChemElement ChemElement { get; set; }
    }
}
