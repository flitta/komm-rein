using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public interface IContextItemRepository<T>
        where T : ContextItem
    {
        public Task Create(T item);

        ValueTask<T> SaveItem(T item);

        Task<T> GetById(Guid id);
    }
}
