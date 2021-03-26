using komm_rein.model;
using komm_rein.api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using Microsoft.AspNetCore.Mvc;

namespace komm_rein.api.Services
{
    public class FacilityService : IFacilityService
    {
        readonly IFacilityRepository _repository;

        public FacilityService(IFacilityRepository repository)
        {
            _repository = repository;
        }
               
        public async ValueTask<IEnumerable<Slot>> GetAvailableSlots(Guid facilityId, DateTime selectedDate, DateTime currentTime)
        {
            var facility = await _repository.GetById(facilityId);
            return GetAvailableSlots(facility, selectedDate, currentTime);
        }

        public IEnumerable<Slot> GetAvailableSlots(Facility facility, DateTime selectedDate, DateTime currentTime)
        {
            var seletecedDayDate = selectedDate.Date;
            var currentTimeTime = (new DateTime() + currentTime.TimeOfDay);

            if (seletecedDayDate < currentTime.Date)
            {
                throw new ArgumentException("Selected day must not be in the past!");
            }

            var result = new List<Slot>();

            foreach (var openingHours in facility.OpeningHours.RemainingForDay(seletecedDayDate, currentTime))
            {
                // if current time is after open-from , use current time
                var startForSlots = currentTimeTime > openingHours.From ? currentTimeTime : openingHours.From;
                var endForSlots = openingHours.To;

                TimeSpan timeSpan = endForSlots - startForSlots;
                int numberOfSlots = (int)(timeSpan / facility.Settings.SlotSize);

                // to leave the gap at the beginning of a timespan (an already started unusable slot), the list is build up form the end
                var slots = Enumerable.Range(1, numberOfSlots).Select((i) =>
                {
                    var from = (endForSlots - facility.Settings.SlotSize * i);
                    var to = from + facility.Settings.SlotSize;

                    // if we have to add a day:
                    var deltaDays = to.Date - from.Date;

                    return new Slot
                    {
                        From = seletecedDayDate + from.TimeOfDay,
                        To = seletecedDayDate + to.TimeOfDay + deltaDays,
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

        public async ValueTask<IEnumerable<Slot>> GetSlotsForVisit(Guid facilityId, DateTime day, Visit visit, DateTime currentTime)
        {
            Facility facility = await _repository.GetById(facilityId);
            var slots = await this.GetAvailableSlots(facilityId, day, currentTime);
            await ApplySlotStatus(slots, facility, day.Date, day.Date.AddDays(1), visit);

            return slots;
        }

        public async Task ApplySlotStatus(Slot slot, Facility facility, DateTime from, DateTime to)
        {
            await ApplySlotStatus(new[] { slot }, facility, from, to);
        }

        public async Task ApplySlotStatus(IEnumerable<Slot> slots, Facility facility, DateTime from, DateTime to)
        {
            await ApplySlotStatus(slots, facility, from, to, null);
        }

        public async Task ApplySlotStatus(IEnumerable<Slot> slots, Facility facility, DateTime from, DateTime to, Visit newVisit)
        {
            // different status when counting new (transient) visit
            bool crowdedIfFull = newVisit != null;

            // load existing visits
            var visits = await _repository.GetVisits(facility.ID, from, to);

            // add newVisit (transient) for evaluation
            if(newVisit != null)
            {
                visits = visits.Concat(new[] { newVisit });
            }

            double crowdedAt = facility.Settings.CrowdedAt;

            foreach (var slot in slots)
            {
                var visitsInSlot = visits.Where(visit =>
                (slot.From == visit.From && slot.To == visit.To)
                ||
                (slot.From > visit.From && slot.To < visit.To)
                ||
                (slot.From <= visit.From && slot.To > visit.From && slot.To < visit.To)
                ||
                (slot.From < visit.To && slot.To >= visit.To)
                )
                    .ToList();

                int paxCount = facility.Settings.CountingMode switch
                {
                    CountingMode.EverySinglePerson => visitsInSlot.SelectMany(v => v.Households).Sum(h => h.NumberOfPersons + h.NumberOfChildren),
                    CountingMode.SinglePersonWithoutChildren => visitsInSlot.SelectMany(v => v.Households).Sum(h => h.NumberOfPersons),
                    CountingMode.HouseHolds => visitsInSlot.SelectMany(v => v.Households).Count(),
                    _ => throw new NotImplementedException(),
                };



                if (crowdedIfFull && paxCount > facility.Settings.MaxNumberofVisitors)
                {
                    slot.Status = SlotStatus.Full;
                }
                else if (paxCount > facility.Settings.MaxNumberofVisitors)
                {
                    slot.Status = SlotStatus.Invalid;
                }
                else if (crowdedIfFull && paxCount == facility.Settings.MaxNumberofVisitors)
                {
                    // the new (planned and transient visit) matches exactly the allowd visitors
                    // we show crowded, as we are only full after the visit would be booked
                    slot.Status = SlotStatus.Crowded;
                }
                else if (paxCount == facility.Settings.MaxNumberofVisitors)
                {
                    slot.Status = SlotStatus.Full;
                }
                else if (paxCount >= crowdedAt)
                {
                    slot.Status = SlotStatus.Crowded;
                }
                else
                {
                    slot.Status = SlotStatus.Free;
                }
            }
        }

        public async ValueTask<Facility> Create(Facility newItem, string ownerSid)
        {
            newItem.AddCreatedInfo(ownerSid);
            await _repository.Create(newItem);
            return newItem;
        }

        public async ValueTask<FacilitySettings> SetSettings(Guid facilityId, FacilitySettings value, string ownerSid)
        {
            var facility = await _repository.GetWithSettings(facilityId);
            if(facility.OwnerSid != ownerSid)
            {
                throw new SecurityException();
            }

            facility.Settings = value;
            
            var result = await _repository.SaveItem(facility);

            return result.Settings;
        }

        public async ValueTask<FacilitySettings> GetSettings(Guid facilityId, string ownerSid)
        {

            var facility = await _repository.GetWithSettings(facilityId);

            if (facility.OwnerSid != ownerSid)
            {
                throw new SecurityException();
            }

            return facility.Settings;
        }

        public async ValueTask<IList<OpeningHours>> GetOpeningHours(Guid facilityId)
        {
            var facility = await _repository.GetWithOpeningHours(facilityId);
            return facility.OpeningHours;
        }

        public async ValueTask<IList<OpeningHours>> SetOpeningHours(Guid facilityId, OpeningHours[] value, string ownerSid)
        {
            var facility = await _repository.GetWithOpeningHours(facilityId);
            if (facility.OwnerSid != ownerSid)
            {
                throw new SecurityException();
            }

            facility.OpeningHours = value;

            var result = await _repository.SaveItem(facility);

            return result.OpeningHours;
        }

        public async ValueTask<Facility> GetByName(string name)
        {
            return await _repository.GetByName(name);
        }

        public async ValueTask<List<Facility>> GetAll()
        {

            return await _repository.GetAll();
        }
    }
}
