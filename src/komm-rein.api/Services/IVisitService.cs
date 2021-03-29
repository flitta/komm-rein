using komm_rein.model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public interface IVisitService
    {
        ValueTask<List<Visit>> GetAll(string sid);

        ValueTask<Visit> BookVisit(Visit visit, string sid);

        ValueTask<Visit> GetById(Guid id, string sid);

        ValueTask<Visit> Cancel(Guid id, string sid);
    }
}
