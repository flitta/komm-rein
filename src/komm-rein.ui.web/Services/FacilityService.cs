using komm_rein.model;
using kommrein.ui.web.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public class FacilityService : ContextItemClientService<Facility>, IFacilityService
    {
        public FacilityService(IHttpService httpService, IOptions<FacilityApiConfig> options)
            : base(httpService, options.Value)
        {
        }

        public Task<Facility[]> GetMyFacilities()
        {
            return _httpService.Get<Facility[]>(_options.Path);
        }

        public Task<Facility> GetWithSetting(Guid id)
        {
            return _httpService.Get<Facility>($"{_options.Path}/withsettings/{id}");
        }

        public Task<FacilitySettings> UpdateSettings(Facility item)
        {
            return _httpService.Put($"{_options.Path}/{item.ID}/settings", item.Settings);
        }

        public Task<OpeningHours[]> UpdateOpeningHours(Facility item)
        {
            return _httpService.Put($"{_options.Path}/{item.ID}/openinghours", item.OpeningHours.ToArray());
        }

        public Task<Slot[]> GetSlots(string name, DateTime day, Visit visit)
        {
            return _httpService.Post<Slot[]>($"{_options.Path}/{name}/slots/{day}", visit);
        }

        public  Task<Visit> VerifyVisit(Guid facilityId, Guid visitId, string signature)
        {
            return  _httpService.Get<Visit>($"{_options.Path}/{facilityId}/{visitId}/{signature}/verify");
        }

        public Task<Visit[]> GetMyVisits(Guid facilityId)
        {
            return _httpService.Get<Visit[]>($"{_options.Path}/{facilityId}/visits");
        }

        public Task<Visit> GetShopsVisit(Guid facilityId, Guid visitId)
        {
            return _httpService.Get<Visit>($"{_options.Path}/{facilityId}/visit/{visitId}");
        }
    }
}
