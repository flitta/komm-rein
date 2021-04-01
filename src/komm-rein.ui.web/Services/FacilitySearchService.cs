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
    public class FacilitySearchService : ContextItemBaseService<Facility>, IFacilitySearchService
    {
        public FacilitySearchService(IHttpService httpService, IOptions<SearchApiConfig> options)
            : base(httpService, options.Value)
        {
        }

        public async ValueTask<List<Facility>> Search(string search)
        {
            return await _httpService.Get<List<Facility>>($"{_options.Path}/{search}");
        }

    }
}
