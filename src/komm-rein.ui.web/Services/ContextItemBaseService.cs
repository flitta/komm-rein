﻿using komm_rein.model;
using kommrein.ui.web.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public class ContextItemBaseService<T> : IContextItemBaseService<T>
        where T : ContextItem
    {
        protected readonly IHttpService _httpService;

        protected readonly ApiConfig _options;

        public ContextItemBaseService(IHttpService httpService, ApiConfig options)
        {
            _httpService = httpService;
            _options = options;

            _httpService.Init(options.ApiName);
        }
      
        public virtual async ValueTask<T> Get(Guid id)
        {
            return await _httpService.Get<T>($"{_options.Path}/{id}");
        }
    }
}
