using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class ChemElement
    {
        public int? Id { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

        public virtual List<ProductChemElement> ProductChemElements { get; set; }

        [Required]
        [Column(TypeName = "decimal(12, 2)")]
        public decimal Gram { get; set; }
    }
}
