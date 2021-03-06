using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.ViewModel
{
    public class FindSlotsViewModel : BookSlotViewModel
    {
        public string Name { get; set; }

        public DateTime Day { get; set; }

        public int PaxCount { get; set; }

        public int? ChildrenCount { get; set; }

        public List<Slot> Slots { get; set; } = new List<Slot>();

    }
}
