using komm_rein.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public class VisitRepository : ContextItemRepository<Visit>, IVisitRepository
    {

        public VisitRepository(KraDbContext dbContext)
           : base(dbContext)
        {
        }

        public Task<List<Visit>> GetAllForSid(string sid)
        {
            return _dbContext.Visits
                .Include(v => v.Facility)
                .Where(x => x.OwnerSid == sid && x.From.Date >= DateTime.Today && !x.IsCanceled)
                .ToListAsync();
        }

        public async Task<Visit> GetById(Guid id)
        {
            return await _dbContext.Visits.FindAsync(id);
        }

        public Task<Visit> GetByIdForOwner(Guid id, string sid)
        {
            return _dbContext.Visits
                .Include(v => v.Facility)
                .Include(v => v.Households)
                .Where(x => x.ID == id && x.OwnerSid == sid )
                .SingleAsync();
        }

    }
}
