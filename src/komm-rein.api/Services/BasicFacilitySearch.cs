using komm_rein.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public class BasicFacilitySearch : ISearchService<Facility>
    {
        KraDbContext _context;

        public BasicFacilitySearch(KraDbContext dbContext)
        {
            _context = dbContext;
        }

        public async ValueTask<List<Facility>> Search(string search, int? take, int? skip)
        {
            var query = _context.Facilities.Where(x => x.Name.ToLower().Contains(search.ToLower()));
            
            if(take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            return await query.ToListAsync();
        }

        public async ValueTask<List<Facility>> Search(string search)
        {
            return await Search(search, null, null);
        }

        public async Task UpsertIntoIndex(Facility item)
        {
            // Method intentionally left empty.
        }
    }
}
