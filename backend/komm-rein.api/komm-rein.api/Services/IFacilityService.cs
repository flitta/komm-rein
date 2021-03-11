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
        IEnumerable<Slot> GetAvailableSlots(Guid facilityId, DateTime selectedDay, DateTime now, int numberOfPax = 1);
    }
}
