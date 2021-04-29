using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class ProductChemElement
    {
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Gram { get; set; }

        public int PId { get; set; }

        public int CEId { get; set; }

        public virtual Product Product { get; set; }

        public virtual ChemElement ChemElement { get; set; }
    }
}
