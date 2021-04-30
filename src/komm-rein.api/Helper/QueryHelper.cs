using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public static class QueryHelper
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, int? page, int? pageSize)
        {
            var result = query;
            if (page.HasValue && pageSize.HasValue)
            {
                result = query.Skip(page.Value * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return result;
        }

    }
}
