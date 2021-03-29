using komm_rein.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public class ContextItemRepository<T>
        where T : ContextItem
    {
        protected readonly KraDbContext _dbContext;

        public ContextItemRepository(KraDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(T item)
        {
            _dbContext.Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<T> SaveItem(T item)
        {
            _dbContext.Update(item);
            await _dbContext.SaveChangesAsync();
            
            return item;
        }

    }
}
