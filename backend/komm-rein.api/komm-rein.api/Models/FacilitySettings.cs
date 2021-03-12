using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Models
{
    public class FacilitySettings
    {
        public TimeSpan SlotSize { get; set; }

        public double SlotStatusThreshold { get; set; }
        
        public int MaxNumberofVisitors { get; set; }
        
        public bool CountHousehold { get; set; }


        // we dont need countchildren of we count-households!
        public bool CountChildren { get; set; }

        public int CrowdedAt { get => (int)(MaxNumberofVisitors * SlotStatusThreshold); }
    }
}
