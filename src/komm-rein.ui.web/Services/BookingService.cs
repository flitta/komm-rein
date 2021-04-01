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
    public class BookingService : ItemBaseService<Slot>, IBookingService
    {
        public BookingService(IHttpService httpService, IOptions<BookingApiConfig> options)
            : base(httpService, options.Value)
        {
        }

        public async ValueTask<List<Slot>> FindSlots(Guid facilityId, DateTime day, Visit requestedVisit)
        {
            return await _httpService.Post<List<Slot>>($"{_options.Path}/{facilityId}/{day.ToString("yyyy-MM-dd")}", requestedVisit);
        }

    }
}
