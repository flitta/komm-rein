using komm_rein.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public class FacilityRepository : ContextItemRepository<Facility>, IFacilityRepository
    {

        public FacilityRepository(KraDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<Facility> GetById(Guid id)
        {
            return _dbContext.Facilities.SingleAsync(x => x.ID == id);
        }

        public Task<Facility> GetByIdWithAssociations(Guid id)
        {
            return _dbContext.Facilities
                  .Include(p => p.OpeningHours)
                  .Include(p => p.Settings)
                  .Include(p => p.MainAddress)
                  .Include(p => p.BillingAddress)
                .SingleAsync(x => x.ID == id);
        }

        public Task<Facility> GetByNameWithAssociations(string name)
        {
            return _dbContext.Facilities
                  .Include(p => p.OpeningHours)
                  .Include(p => p.Settings)
                  .Include(p => p.MainAddress)
                  .Include(p => p.BillingAddress)
                .SingleAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public Task<List<Visit>> GetVisits(Guid facilityId, DateTime from, DateTime to)
        {
            return _dbContext.Visits.Include(v => v.Households).Where(v => v.Facility.ID == facilityId
            && !v.IsCanceled
            && v.From >= from
            && v.To <= to)
                .ToListAsync();
        }

        public Task<List<Visit>> GetVisits(Guid facilityId, DateTime from, int? page, int? pageSize)
        {
            var query = _dbContext.Visits.Include(v => v.Households).Where(v => v.Facility.ID == facilityId
                    && !v.IsCanceled
                    && v.From >= from)
                .Paging(page, pageSize);

            return query.ToListAsync();
        }

        public Task<Facility> GetWithSettings(Guid id)
        {
            return _dbContext.Facilities.Include(p => p.Settings).SingleAsync(x => x.ID == id);
        }

        public Task<Facility> GetWithOpeningHours(Guid id)
        {
            return _dbContext.Facilities.Include(p => p.OpeningHours).SingleAsync(x => x.ID == id);
        }

        public Task<Facility> GetByName(string name)
        {
            return _dbContext.Facilities
                .FirstAsync(x => x.Name == name);
        }

        public Task<List<Facility>> GetAll()
        {
            return _dbContext.Facilities
                  .Take(100)
              .ToListAsync();
        }

        public Task<Visit> GetVisit(Guid visitId)
        {
            return _dbContext.Visits
                .Include(v => v.Facility)
                .Include(v => v.Households)
                .Where(v => v.ID == visitId)
                .SingleAsync();
        }
    }
}
