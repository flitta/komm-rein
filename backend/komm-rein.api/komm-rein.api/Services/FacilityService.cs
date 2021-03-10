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

        public IEnumerable<Slot> GetAvailableSlots(Guid id, DateTime from, DateTime to, int numberOfPax = 1)
        {
            // from should be after to
            if(to <= from)
            {
                throw new ArgumentException("To date must be after from date!");
            }

            // load facility
            var facility = _repository.GetById(id);
                                    
            // validate from
            if(from <= DateTime.Now + facility.Settings.TimeToPlanAhead)
            {
                throw new ArgumentException($"From date must not be before {(DateTime.Now + facility.Settings.TimeToPlanAhead).ToString()}!");
            }

            throw new NotImplementedException();

        }
    }
}
