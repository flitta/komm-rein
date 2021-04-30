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
        Task<Facility> GetByNameWithAssociations(string name);

        Task<Facility> GetByIdWithAssociations(Guid id);

        Task<List <Visit>> GetVisits(Guid facilityId, DateTime from, DateTime to);

        Task<List<Visit>> GetVisits(Guid facilityId, DateTime from, int? page, int? pageSize);


        Task<Facility> GetWithSettings(Guid facilityId);

        Task<Facility> GetWithOpeningHours(Guid id);

        Task<Facility> GetByName(string name);

        Task<List<Facility>> GetAll();
        
        Task<Visit> GetVisit(Guid visitId);
    }
}
