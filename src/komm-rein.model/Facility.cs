using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public class Facility : ContextItem
    {
        public FacilitySettings Settings { get; set; }

        public IList<OpeningHours> OpeningHours { get; set; }

        [Required]
        public String Name{ get; set; }

        [JsonIgnore]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public bool IsLive{ get; set; }

        [Required]
        public Address MainAddress { get; set; }

        public Address BillingAddress { get; set; }

    }
}
