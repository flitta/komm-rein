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
        readonly IFacilityRepository _repository;

        public FacilityService(IFacilityRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Slot> GetAvailableSlots(Guid facilityId, DateTime selectedDate, DateTime currentTime)
        {
            return GetAvailableSlots(facilityId, selectedDate, currentTime, 1);
        }

        public IEnumerable<Slot> GetAvailableSlots(Guid facilityId, DateTime selectedDate, DateTime currentTime, int numberOfPax)
        {
            var seletecedDayDate = selectedDate.Date;
            var currentTimeTime = (new DateTime() + currentTime.TimeOfDay);

            if (seletecedDayDate < currentTime.Date)
            {
                throw new ArgumentException("Selected day must not be in the past!");
            }

            var facility = _repository.GetById(facilityId);

            var result = new List<Slot>();

            foreach (var openingHours in facility.OpeningHours.RemainingForDay(seletecedDayDate, currentTime))
            {
                // if current time is after open-from , use current time
                var startForSlots = currentTimeTime > openingHours.From ? currentTimeTime : openingHours.From;
                var endForSlots = openingHours.To;

                TimeSpan timeSpan = endForSlots - startForSlots;
                int numberOfSlots = (int)(timeSpan/ facility.Settings.SlotSize);

                // to leave the gap at the beginning of a timespan (an already started unusable slot), the list is build up form the end
                var slots = Enumerable.Range(1, numberOfSlots).Select((i) => {
                        var from = (endForSlots - facility.Settings.SlotSize * i);
                        var to = from + facility.Settings.SlotSize;
                        return new Slot
                        {
                            From = from,
                            To = to,
                            OpeningHours = openingHours,
                            Facility = facility,
                        };
                    }
                );

                // because the list was build in reverse order, it will be reversed before insert
                result.AddRange(slots.Reverse());
            }

            return result;
        }
             
    }
}
