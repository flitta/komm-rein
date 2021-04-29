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
        ValueTask<Signed<Visit>> BookForSlot(string name, DateTime from, DateTime to, int pax, int? kids);

        Task Cancel(Visit visit);

        ValueTask<Visit[]> GetMyVisits();

        ValueTask<Signed<Visit>> GetSigned(Guid id);

    }
}
