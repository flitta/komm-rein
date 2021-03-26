using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public abstract class ContextItem
    {
        public Guid ID { get; set; }

        [JsonIgnore]
        public string OwnerSid { get; set; }

        [JsonIgnore]
        public string CreatedBySid { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public string UpdatedBySid { get; set; }

        [JsonIgnore]
        public DateTime UpdatedDate { get; set; }

        [JsonIgnore]
        public string DeletedBySid { get; set; }

        [JsonIgnore]
        public DateTime? DeleteddDate { get; set; }

    }
}
