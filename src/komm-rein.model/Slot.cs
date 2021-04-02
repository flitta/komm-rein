using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{
    
    public enum SlotStatus
    {
        None, 
        Free,
        Crowded,
        Full ,
        Invalid,
    }

    public class Slot
    {
        public SlotStatus Status { get; set; }
    
        public Guid FacilityId { get; set; }
        
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        
        public override string ToString() => $"From: {From.TimeOfDay} to: {To.TimeOfDay}, Status: {Status}";
    }
}
