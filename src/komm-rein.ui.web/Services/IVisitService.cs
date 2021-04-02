using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public interface IVisitService : IContextItemClientService<Visit>
    {
        ValueTask<Visit> BookForSlot(Signed<Slot> slot, int pax, int? kids);
    }
}
