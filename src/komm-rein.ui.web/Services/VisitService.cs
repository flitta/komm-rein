﻿using komm_rein.model;
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

        public async ValueTask<Visit> BookForSlot(Signed<Slot> slot, int pax, int? kids)
        {
            return await _httpService.Post<Visit>($"{_options.Path}/{pax}/{kids.GetValueOrDefault()}", slot);
        }

    }
}
