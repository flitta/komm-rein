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

        public async ValueTask<List<Slot>> FindSlots(string facilityName, DateTime day, int pax, int? kids)
        {
            return await _httpService.Get<List<Slot>>($"{_options.Path}/{facilityName}/{day.ToString("yyyy-MM-dd")}/{pax}/{kids.GetValueOrDefault()}");
        }

    }
}
