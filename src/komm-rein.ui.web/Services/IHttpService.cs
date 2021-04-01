﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string path);
        Task<T> Post<T>(string path, T value);

        Task<TR> Post<TR>(string path, object value);

        Task<T> Put<T>(string path, T value);
    }
}
