using System;
using System.Collections.Generic;

namespace komm_rein.api.Models
{
    public class Visit
    {
        public Facility Facility { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public List<Household> Households { get; set; }
    }
}