﻿using komm_rein.model;
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

        public async ValueTask<IList<Visit>> GetAllForSid(string sid)
        {
            return await _dbContext.Visits.Where(x => x.OwnerSid == sid && x.From.Date >= DateTime.Today).ToListAsync();
        }

        public async ValueTask<Visit> GetById(Guid id)
        {
            return await _dbContext.Visits.FindAsync(id);
        }

    }
}
