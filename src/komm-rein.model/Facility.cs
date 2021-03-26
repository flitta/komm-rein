using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public class Facility : ContextItem
    {
        [JsonIgnore]
        public FacilitySettings Settings { get; set; }

        [JsonIgnore]
        public IList<OpeningHours> OpeningHours { get; set; }

        public String Name{ get; set; }

    }
}
