using komm_rein.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.ViewModel
{
    public class EditFacilityViewModel : CreateFacilityViewModel
    {
        public IList<OpeningHours> OpeningHours { get; set; }

        public OpeningHoursViewModel NewOpeningHours { get; set; }
        
        [Required]
        public int MaxNumberofVisitors { get; set; }

        [Required]
        public CountingMode CountingMode { get; set; }

        [Required]
        public double SlotStatusThreshold { get; set; }

        [Required]
        public int SlotMinutes { get; set; }

        public Guid ID { get; set; }
    }
}
