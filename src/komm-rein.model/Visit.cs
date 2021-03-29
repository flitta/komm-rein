using System;
using System.Collections.Generic;

namespace komm_rein.model
{
    public class Visit : ContextItem
    {
        public Facility Facility { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public List<Household> Households { get; set; }

        public bool IsCanceled { get; set; }
    }
}