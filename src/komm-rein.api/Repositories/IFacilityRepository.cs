using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public interface IFacilityRepository : IContextItemRepository<Facility>
    {
        ValueTask<Facility> GetByNameWithAssociations(string name);

        ValueTask<Facility> GetByIdWithAssociations(Guid id);

        public ValueTask<IEnumerable<Visit>> GetVisits(Guid facilityId, DateTime from, DateTime to);

            
        ValueTask<Facility> GetWithSettings(Guid facilityId);

        ValueTask<Facility> GetWithOpeningHours(Guid id);
        
        ValueTask<Facility> GetByName(string name);

        ValueTask<List<Facility>> GetAll();
    }
}
