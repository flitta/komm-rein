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
        Task<List<Visit>> GetAllForSid(string sid);
        
        Task<Visit> GetByIdForOwner(Guid id, string sid);
    }
}
