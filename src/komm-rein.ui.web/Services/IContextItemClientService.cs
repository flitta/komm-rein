using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public interface IContextItemClientService<T>
        where T : ContextItem
    {
        ValueTask<T> Create(T item);

        ValueTask<T> Update(T item);

        ValueTask<T> Get(Guid id);
    }
}
