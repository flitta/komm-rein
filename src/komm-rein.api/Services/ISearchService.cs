using komm_rein.model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public interface ISearchService<T>
    {
        ValueTask<List<T>> Search(string search, int? take, int? skip);

        ValueTask<List<T>> Search(string search);

        Task UpsertIntoIndex(T item);
    }
}
