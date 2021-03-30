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
    }
}
