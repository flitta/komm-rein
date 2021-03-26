using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public interface IFacilityService
    {
        Task Create(Facility newItem, string ownerSid);

        //Task<IEnumerable<Slot>> GetAvailableSlots(Facility facility, DateTime selectedDate, DateTime currentTime);

        //Task<IEnumerable<Slot>> GetAvailableSlots(Guid facilityId, DateTime selectedDate, DateTime currentTime);

        //Task ApplySlotStatus(IEnumerable<Slot> slots, Facility facility, DateTime from, DateTime to, Visit newVisit);

        //Task ApplySlotStatus(IEnumerable<Slot> slots, Facility facility, DateTime from, DateTime to);

        //Task ApplySlotStatus(Slot slot, Facility facility, DateTime from, DateTime to);

        //Task<IEnumerable<Slot>> GetSlotsForVisit(Guid facilityId, DateTime day, Visit visit, DateTime currentTime);
    }
}
