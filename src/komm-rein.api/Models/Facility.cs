using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Models
{
    public class Facility
    {
        public Guid ID { get; set; }

        public FacilitySettings Settings { get; set; }
                
        public IList<OpeningHours> OpeningHours { get; set; }



    }
}
