using komm_rein.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public interface IFacilityService
    {
        IEnumerable<Slot> GetAvailableSlots(Facility facility, DateTime selectedDate, DateTime currentTime);

        IEnumerable<Slot> GetAvailableSlots(Guid facilityId, DateTime selectedDate, DateTime currentTime);
       
        void ApplySlotStatus(IEnumerable<Slot> slots, Facility facility, DateTime from, DateTime to, Visit newVisit);

        void ApplySlotStatus(IEnumerable<Slot> slots, Facility facility, DateTime from, DateTime to);

        void ApplySlotStatus(Slot slot, Facility facility, DateTime from, DateTime to);

        IEnumerable<Slot> GetSlotsForVisit(Guid facilityId, DateTime day, Visit visit, DateTime currentTime);
    }
}
