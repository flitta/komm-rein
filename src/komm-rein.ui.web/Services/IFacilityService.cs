﻿using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public interface IFacilityService : IContextItemClientService<Facility>
    {
        ValueTask<Facility> GetWithSetting(Guid id);

    }
}
