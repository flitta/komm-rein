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
        ValueTask<Visit[]> GetAll(string sid);
        
        ValueTask<Signed<Visit>> BookVisit(string name, DateTime from, DateTime to, int pax, int kids, string sid);

        ValueTask<Signed<Visit>> GetByIdForOwner(Guid id, string sid);

        ValueTask<Visit> Cancel(Guid id, string sid);
    }
}
