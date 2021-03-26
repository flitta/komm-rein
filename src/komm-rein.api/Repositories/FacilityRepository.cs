using komm_rein.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        readonly KraDbContext _dbContext;

        public FacilityRepository(KraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Facility item)
        {
            _dbContext.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<Facility> SaveItem(Facility item)
        {
            _dbContext.Update(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async ValueTask<Facility> GetById(Guid id)
        {
            return await _dbContext.Facilities.FindAsync(id);
        }

        public async ValueTask<IEnumerable<Visit>> GetVisits(Guid facilityId, DateTime from, DateTime to)
        {
            return await _dbContext.Visits.Where(v => v.Facility.ID == facilityId && v.From >= from && v.To <= to).ToListAsync();
        }

        public async ValueTask<Facility> GetWithSettings(Guid id)
        {
            return await _dbContext.Facilities.Include(p => p.Settings).SingleAsync(x => x.ID == id);
        }

        public async ValueTask<Facility> GetWithOpeningHours(Guid id)
        {
            return await _dbContext.Facilities.Include(p => p.OpeningHours).SingleAsync(x => x.ID == id);
        }
    }
}
