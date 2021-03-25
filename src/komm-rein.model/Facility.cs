using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public class Facility : ContextItem
    {
        public FacilitySettings Settings { get; set; }
                
        public IList<OpeningHours> OpeningHours { get; set; }

        public String Name{ get; set; }

    }
}
