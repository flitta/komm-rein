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

        public IEnumerable<Slot> GetAvailableSlots(Guid facilityId, DateTime selectedDay, DateTime now, int numberOfPax = 1)
        {
            var startofSelectedDay = selectedDay.Date;
            if (startofSelectedDay.Date < now.Date)
            {
                throw new ArgumentException("Selected day must not be in the past!");
            }

            var facility = _repository.GetById(facilityId);
           
            var startForSlots = now < startofSelectedDay ? startofSelectedDay : now;
            var endForSlots = startofSelectedDay.AddDays(1);

            // create list of slots for timespan
            TimeSpan timeSpan = endForSlots - startForSlots;
            int numberOfSlots = (int)(timeSpan.TotalMinutes / facility.Settings.SlotSize.TotalMinutes);
                        
            var slots = Enumerable.Range(1, numberOfSlots).Select((i) => new Slot { });

            return slots;
        }

        public OpeningHours FindOpeningHours(Guid facilityId, DateTime time)
        {
            var facility = _repository.GetById(facilityId);
            var dayOfWeek = time.DayOfWeek.FromSystem();

            var result = facility.OpeningHours.FirstOrDefault(o =>
                (o.DayOfWeek & dayOfWeek) == dayOfWeek
                && o.From.TimeOfDay <= time.TimeOfDay && o.To.TimeOfDay >= time.TimeOfDay);

            return result;
        }
    }
}
