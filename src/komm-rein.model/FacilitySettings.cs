using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public enum CountingMode
    {
        EverySinglePerson,
        HouseHolds,
        SinglePersonWithoutChildren
    }

    public class FacilitySettings : ContextItem
    {

        [NotMapped]
        public int SlotSizeMinutes { get; set; }

        [JsonIgnore]
        public TimeSpan SlotSize { get; set; }

        public double SlotStatusThreshold { get; set; }
        
        public int MaxNumberofVisitors { get; set; }
        
        public CountingMode CountingMode { get; set; }

        public int CrowdedAt { get => (int)(MaxNumberofVisitors * SlotStatusThreshold); }
    }
}
