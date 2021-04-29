using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public interface IFacilityService : IContextItemClientService<Facility>
    {
        ValueTask<Visit> VerifyVisit(Guid facilityId, Guid visitId, string signature);

        ValueTask<Facility> GetWithSetting(Guid id);

        ValueTask<Facility[]> GetMyFacilities();

        ValueTask<Slot[]> GetSlots(string name, DateTime day, Visit visit);

        ValueTask<FacilitySettings> UpdateSettings(Facility item);

        ValueTask<OpeningHours[]> UpdateOpeningHours(Facility item);
    }
}
