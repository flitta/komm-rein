using komm_rein.model;
using kommrein.ui.web.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public class ContextItemClientService<T> : IContextItemClientService<T>
        where T : ContextItem
    {
        protected readonly IHttpService _httpService;

        protected readonly ApiConfig _options;

        public ContextItemClientService(IHttpService httpService, ApiConfig options)
        {
            _httpService = httpService;
            _options = options;
        }
        
        public virtual async ValueTask<T> Create(T item)
        {
            return await _httpService.Post(_options.Path, item);
        }

        public virtual async ValueTask<T> Get(Guid id)
        {
            return await _httpService.Get<T>($"{_options.Path}/{id}");
        }

        public virtual async ValueTask<T> Update(T item)
        {
            return await _httpService.Put($"{_options.Path}/{item.ID}", item);
        }
    }
}
