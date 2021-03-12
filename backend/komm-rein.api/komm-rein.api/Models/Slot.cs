using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Models
{
    public class Slot
    {
        public Facility Facility { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public OpeningHours OpeningHours { get; internal set; }
    }
}
