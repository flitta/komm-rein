using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Models
{
    public enum CountingMode
    {
        EverySinglePerson,
        HouseHolds,
        SinglePersonWithoutChildren
    }

    public class FacilitySettings
    {
    
        public TimeSpan SlotSize { get; set; }

        public double SlotStatusThreshold { get; set; }
        
        public int MaxNumberofVisitors { get; set; }
        
        public CountingMode CountingMode { get; set; }

        public int CrowdedAt { get => (int)(MaxNumberofVisitors * SlotStatusThreshold); }
    }
}
