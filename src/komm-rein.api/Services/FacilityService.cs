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
        readonly IProtectionService _protectionService;

        public FacilityService(IFacilityRepository repository)
        {
            _repository = repository;
        }


        public FacilityService(IFacilityRepository repository, IProtectionService protectionService)
        {
            _repository = repository;
            _protectionService = protectionService;
        }

        public async ValueTask<Slot[]> GetSlots(string name, DateTime day, int pax, int? kids)
        {
            Facility facility = await _repository.GetByNameWithAssociations(name);
            Visit visit = facility.Settings.CreateVisit(pax, kids);

            var slots = await GetSlotsForVisit(facility.ID, day, visit, DateTime.Now);

            return slots;
        }

        public async ValueTask<Facility> GetById(Guid id)
        {
            return await _repository.GetById(id);
        }

        public async ValueTask<Facility> GetByIdWithSettings(Guid id, string sid)
        {
            var facility = await _repository.GetByIdWithAssociations(id);
            if (facility.OwnerSid != sid)
            {
                throw new SecurityException();
            }

            return facility;
        }

        public async ValueTask<IEnumerable<Slot>> GetAvailableSlots(Guid facilityId, DateTime selectedDate, DateTime currentTime)
        {
            var facility = await _repository.GetByIdWithAssociations(facilityId);
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
                            FacilityId = facility.ID,
                        };
                    }
                );

                // because the list was build in reverse order, it will be reversed before insert
                result.AddRange(slots.Reverse());
            }

            return result;
        }
               
        public async ValueTask<Slot[]> GetSlotsForVisit(string name, DateTime day, Visit visit, DateTime currentTime)
        {
            Facility facility = await _repository.GetByName(name);
            var slots = await this.GetAvailableSlots(facility.ID, day, currentTime);
            await ApplySlotStatus(slots, facility, day.Date, day.Date.AddDays(1), visit);

            return slots.ToArray();
        }

        public async ValueTask<Slot[]> GetSlotsForVisit(string name, DateTime day, Visit visit)
        {
            return await GetSlotsForVisit(name, day, visit, DateTime.Now);
        }

        public async ValueTask<Slot[]> GetSlotsForVisit(Guid facilityId, DateTime day, Visit visit)
        {
            return await GetSlotsForVisit(facilityId, day, visit, DateTime.Now);
        }

        public async ValueTask<Slot[]> GetSlotsForVisit(Guid facilityId, DateTime day, Visit visit, DateTime currentTime)
        {
            Facility facility = await _repository.GetById(facilityId);
            var slots = await this.GetAvailableSlots(facilityId, day, currentTime);
            await ApplySlotStatus(slots, facility, day.Date, day.Date.AddDays(1), visit);

            return slots.ToArray();
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
            bool countWithPlannedVisit = newVisit != null;

            // load existing visits
            var visits = await _repository.GetVisits(facility.ID, from, to);
                        
            double crowdedAt = facility.Settings.CrowdedAt;

            int numberOfNewVistors = 0;
            if(newVisit != null)
            {
                numberOfNewVistors = facility.Settings.CountingMode switch
                {
                    CountingMode.EverySinglePerson => newVisit.Households.Sum(h => h.NumberOfPersons + h.NumberOfChildren),
                    CountingMode.SinglePersonWithoutChildren => newVisit.Households.Sum(h => h.NumberOfPersons),
                    CountingMode.HouseHolds => newVisit.Households.Count,
                    _ => throw new NotImplementedException(),
                };
            }


            foreach (var slot in slots)
            {
                List<Visit> visitsInSlot = visits.Where(visit =>
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

                if (countWithPlannedVisit && (paxCount + numberOfNewVistors) > facility.Settings.MaxNumberofVisitors)
                {
                    // full instead invalid, beacuase we count planned visit
                    slot.Status = SlotStatus.Full;
                }
                else if (paxCount > facility.Settings.MaxNumberofVisitors)
                {
                    slot.Status = SlotStatus.Invalid;
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

        public async ValueTask<Facility> Create(Facility newItem, string sid)
        {
            // create new item form input
            Facility facility = new() { 
                Name = newItem.Name,

                // lets use the dto here
                MainAddress = newItem.MainAddress
            };

            facility.AddCreatedInfo(sid);
            
            await _repository.Create(facility);
            return facility;
        }

        public async ValueTask<Facility> Update(Facility item, string sid)
        {
            var facility = await _repository.GetByIdWithAssociations(item.ID);
            if (facility.OwnerSid != sid)
            {
                throw new SecurityException();
            }

            // update properties and address
            facility.IsLive = item.IsLive;
            facility.Name = item.Name;
            facility.MainAddress.AdditionalInfo = item.MainAddress.AdditionalInfo;
            facility.MainAddress.City = item.MainAddress.City;
            facility.MainAddress.ContactEmail = item.MainAddress.ContactEmail;
            facility.MainAddress.ContactName = item.MainAddress.ContactName;
            facility.MainAddress.ContactPhone = item.MainAddress.ContactPhone;
            facility.MainAddress.Country = item.MainAddress.Country;
            facility.MainAddress.Region = item.MainAddress.Region;
            facility.MainAddress.Street_1 = item.MainAddress.Street_1;
            facility.MainAddress.Street_2 = item.MainAddress.Street_2;
            facility.MainAddress.Street_3 = item.MainAddress.Street_3;
            facility.MainAddress.ZipCode = item.MainAddress.ZipCode;

            facility.AddupdatedInfo(sid);

            await _repository.SaveItem(facility);

            return facility;
        }

        public async ValueTask<FacilitySettings> SetSettings(Guid facilityId, FacilitySettings value, string sid)
        {
            var facility = await _repository.GetWithSettings(facilityId);
            if(facility.OwnerSid != sid)
            {
                throw new SecurityException();
            }

            facility.Settings = value;
            facility.Settings.SlotSize = TimeSpan.FromMinutes(value.SlotSizeMinutes);

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


        public async ValueTask<IList<OpeningHours>> SetOpeningHours(Guid facilityId, OpeningHours[] value, string sid)
        {
            var facility = await _repository.GetWithOpeningHours(facilityId);
            if (facility.OwnerSid != sid)
            {
                throw new SecurityException();
            }

            // openinghours instance should be immutable

            if(facility.OpeningHours == null)
            {
                facility.OpeningHours = new List<OpeningHours>();
            }
                        
            foreach(OpeningHours item in value.Where(x => x.ID == Guid.Empty))
            {
                facility.OpeningHours.Add(item);
            }

            foreach (OpeningHours item in facility.OpeningHours.Where(x => !value.Any(y => y.ID == x.ID)).Reverse())
            {
                facility.OpeningHours.Remove(item);
            }

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

        public async ValueTask<Visit> Verify(Guid facilityId, Guid visitId, string signature, string sid)
        {
            var visit = await _repository.GetVisit(visitId);
            
            if (visit.Facility.OwnerSid != sid || visit.Facility.ID != facilityId)
            {
                throw new SecurityException();
            }

            if (_protectionService.Verify(signature, visit.ToDto()))
            {
                return visit;
            }
            else
            {
                return new Visit();
            }
        }

        public async ValueTask<Visit[]> GetVisits(Guid facilityId, string sid, int? pageCount, int? pageSize)
        {
            var facility = await _repository.GetById(facilityId);
            if (facility.OwnerSid != sid)
            {
                throw new SecurityException();
            }

            var result = await _repository.GetVisits(facilityId, DateTime.Today, pageCount, pageSize);

            return result.Select(v => v.ToDto()).ToArray();
        }

        public ValueTask<Visit[]> GetVisits(Guid facilityId, string sid)
        {
            return GetVisits(facilityId, sid, null, null);
        }

        public async ValueTask<Visit> GetVisit(Guid facilityId, Guid visitId, string sid)
        {

            var visit = await _repository.GetVisit(visitId);
            if (visit.Facility.OwnerSid != sid || visit.Facility.ID != facilityId)
            {
                throw new SecurityException();
            }

            return visit.ToDto();
        }
    }
}
