using komm_rein.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public class FacilityRepository : ContextItemRepository<Facility>,  IFacilityRepository
    {
      
        public FacilityRepository(KraDbContext dbContext)
            :base(dbContext)
        {
        }

        public async ValueTask<Facility> GetById(Guid id)
        {
            return await _dbContext.Facilities.SingleAsync(x => x.ID == id);
        }

        public async ValueTask<Facility> GetByIdWithAssociations(Guid id)
        {
            return await _dbContext.Facilities
                  .Include(p => p.OpeningHours)
                  .Include(p => p.Settings)
                  .Include(p => p.MainAddress)
                  .Include(p => p.BillingAddress)
                .SingleAsync(x => x.ID == id);
        }

        public async ValueTask<IEnumerable<Visit>> GetVisits(Guid facilityId, DateTime from, DateTime to)
        {
            return await _dbContext.Visits.Where(v => v.Facility.ID == facilityId 
            && !v.IsCanceled
            && v.From >= from 
            && v.To <= to)
                .ToListAsync();
        }

        public async ValueTask<Facility> GetWithSettings(Guid id)
        {
            return await _dbContext.Facilities.Include(p => p.Settings).SingleAsync(x => x.ID == id);
        }

        public async ValueTask<Facility> GetWithOpeningHours(Guid id)
        {
            return await _dbContext.Facilities.Include(p => p.OpeningHours).SingleAsync(x => x.ID == id);
        }

        public async ValueTask<Facility> GetByName(string name)
        {
            return await _dbContext.Facilities
                .FirstAsync(x => x.Name == name);
        }

        public async ValueTask<List<Facility>> GetAll()
        {
            return await _dbContext.Facilities
                  .Take(100)  
              .ToListAsync();
        }
    }
}
