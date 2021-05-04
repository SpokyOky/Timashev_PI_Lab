using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class Recipe
    {
        public int? Id { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

        [DisplayName("Технология приготовления")]
        public string HowToCook { get; set; }

        [DisplayName("Описание качества")]
        public string Quality { get; set; }

        public bool? DeleteMark { get; set; }

        public virtual List<ProductRecipe> ProductRecipes { get; set; }

        public virtual List<TechCard> TechCards { get; set; }
    }
}
