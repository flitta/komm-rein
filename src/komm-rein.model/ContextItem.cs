using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public abstract class ContextItem
    {
        public Guid ID { get; set; }

        public string OwnerSid { get; set; }

        public string CreatedBySid { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBySid { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string DeletedBySid { get; set; }

        public DateTime? DeleteddDate { get; set; }

    }
}
