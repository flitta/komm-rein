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
        Task<Visit> VerifyVisit(Guid facilityId, Guid visitId, string signature);

        Task<Facility> GetWithSetting(Guid id);

        Task<Facility[]> GetMyFacilities();

        Task<Slot[]> GetSlots(string name, DateTime day, Visit visit);

        Task<FacilitySettings> UpdateSettings(Facility item);

        Task<OpeningHours[]> UpdateOpeningHours(Facility item);

        Task<Visit[]> GetMyVisits(Guid facilityId);

        Task<Visit> GetShopsVisit(Guid facilityId, Guid visitId);
    }
}
