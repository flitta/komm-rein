using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public interface IVisitRepository : IContextItemRepository<Visit>
    {
        ValueTask<IList<Visit>> GetAllForSid(string sid);
        ValueTask<Visit> GetByIdForOwner(Guid id, string sid);
    }
}
