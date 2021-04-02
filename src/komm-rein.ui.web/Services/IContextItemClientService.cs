using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public interface IContextItemClientService<T> : IItemBaseService<T>
        where T : ContextItem, new()
    {
        ValueTask<T> Create(T item);

        ValueTask<T> Update(T item);
    }
}
