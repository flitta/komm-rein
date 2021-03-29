using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public interface IFacilityRepository
    {
        public ValueTask<Facility> GetById(Guid id);

        public ValueTask<IEnumerable<Visit>> GetVisits(Guid facilityId, DateTime from, DateTime to);

        public Task Create(Facility item);
            
        ValueTask<Facility> GetWithSettings(Guid facilityId);

        ValueTask<Facility> SaveItem(Facility item);

        ValueTask<Facility> GetWithOpeningHours(Guid id);
        
        ValueTask<Facility> GetByName(string name);

        ValueTask<List<Facility>> GetAll();
    }
}
