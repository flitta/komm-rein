using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Repositories
{
    public interface IFacilityRepository
    {
        public Facility GetById(Guid id);

        public IEnumerable<Visit> GetVisits(Guid facilityId, DateTime from, DateTime to);

        public void Create(Facility item);
    }
}
