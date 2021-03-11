using komm_rein.api.Models;
using komm_rein.api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public class FacilityService : IFacilityService
    {
        IFacilityRepository _repository;

        public FacilityService(IFacilityRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Slot> GetAvailableSlots(Guid facilityId, DateTime day, int numberOfPax = 1)
        {
            if(day.ToUniversalTime() < DateTime.Now.ToUniversalTime())
            {
                throw new ArgumentException("Day must not be in the past!");
            }

            throw new NotImplementedException();

        }
    }
}
