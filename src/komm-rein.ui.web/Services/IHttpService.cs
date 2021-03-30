using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string path);
        Task<T> Post<T>(string path, T value);
        Task<T> Put<T>(string path, T value);
    }
}
