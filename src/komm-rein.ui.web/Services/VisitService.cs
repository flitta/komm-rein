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
    public class VisitService : ContextItemClientService<Visit>, IVisitService
    {
        public VisitService(IHttpService httpService, IOptions<VisitApiConfig> options)
            : base(httpService, options.Value)
        {
        }

        public async ValueTask<Signed<Visit>> BookForSlot(string name, DateTime from, DateTime to, int pax, int? kids)
        {
            return await _httpService.Post<Signed<Visit>>($"{_options.Path}/book/{name}/{from.ToInvariantString()}/{to.ToInvariantString()}/{pax}/{kids.GetValueOrDefault()}", new object());
        }

        public async Task Cancel(Visit visit)
        {
            await _httpService.Put<Visit>($"{_options.Path}/cancel", visit);
        }

        public async ValueTask<Visit[]> GetMyVisits()
        {
            return await _httpService.Get<Visit[]>(_options.Path);
        }

        public async ValueTask<Signed<Visit>> GetSigned(Guid id)
        {
            return await _httpService.Get<Signed<Visit>>($"{_options.Path}/{id}");
        }
    }
}
