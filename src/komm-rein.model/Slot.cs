using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public enum SlotStatus
    {
        Free,
        Crowded,
        Full,
        Invalid,
    }

    public class Slot
    {
        public SlotStatus Status { get; set; }

        public OpeningHours OpeningHours { get; set; }

        public Guid FacilityId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public List<Visit> Visits { get; set; } = new List<Visit>();

        public override string ToString() => $"From: {From.TimeOfDay} to: {To.TimeOfDay}, Status: {Status}";
    }
}
