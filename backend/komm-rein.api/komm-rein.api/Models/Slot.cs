using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Models
{
  
    public class Slot
    {
        public enum SlotStatus
        {
            Free,
            Crowded,
            Full,
            Invalid,
        }

        public SlotStatus Status { get; set; }

        public OpeningHours OpeningHours { get; internal set; }

        public Facility Facility { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public List<Visit> Visits { get; set; } = new List<Visit>();
    }
}
