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

        public async ValueTask<Facility[]> GetMyFacilities()
        {
            return await _httpService.Get<Facility[]>(_options.Path);
        }

        public async ValueTask<Facility> GetWithSetting(Guid id)
        {
            return await _httpService.Get<Facility>($"{_options.Path}/withsettings/{id}");
        }

        public async ValueTask<FacilitySettings> UpdateSettings(Facility item)
        {
            return await _httpService.Put($"{_options.Path}/{item.ID}/settings", item.Settings);
        }

        public async ValueTask<OpeningHours[]> UpdateOpeningHours(Facility item)
        {
            return await _httpService.Put($"{_options.Path}/{item.ID}/openinghours", item.OpeningHours.ToArray());
        }

        public async ValueTask<Slot[]> GetSlots(string name, DateTime day, Visit visit)
        {
            return await _httpService.Post<Slot[]>($"{_options.Path}/{name}/slots/{day}", visit);
        }
    }
}
