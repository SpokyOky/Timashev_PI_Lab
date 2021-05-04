using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class Product
    {
        public int? Id { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

        public int Gram { get; set; }

        public bool? DeleteMark { get; set; }

        public virtual List<ProductChemElement> ProductChemElements { get; set; }

        public virtual List<ProductRecipe> ProductRecipes { get; set; }
    }
}
