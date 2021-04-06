﻿using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public interface IBookingService
    {
        ValueTask<List<Slot>> FindSlots(string facilityName, DateTime day, int pax, int? kids);
    }
}
