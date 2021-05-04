using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Timashev_PI_Lab.Models
{
    public class TechCard
    {
        public int? Id { get; set; }

        [DisplayName("Название")]
        public string Name { get; set; }

        public bool? DeleteMark { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
