using komm_rein.api.Repositories;
using komm_rein.model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public class VisitService : IVisitService
    {
        readonly IVisitRepository _repository;
        readonly IFacilityService _facilityService;


        public VisitService(IVisitRepository repository, IFacilityService facilityService)
        {
            _repository = repository;
            _facilityService = facilityService;
        }

        public async ValueTask<Visit> BookVisit(Visit visit, string sid)
        {
            // check if valid
            var facility = await _facilityService.GetById(visit.Facility.ID);
            var availableSlots = await _facilityService.GetSlotsForVisit(visit.Facility.ID, visit.From.Date, visit);

            var slotFound = availableSlots.FirstOrDefault(s => 
                    s.From == visit.From 
                &&  s.To == visit.To 
                &&  s.Status != SlotStatus.Full 
                &&  s.Status != SlotStatus.Invalid);

            if (slotFound == null)
            {
                throw new ArgumentException("No available slot found!");
            }

            // to do: we should implement a lock
            Visit newVisit = new()
            {
                Facility = facility,
                From = visit.From,
                To = visit.To,
                Households = new List<Household>()
            };

            foreach(Household household in visit.Households)
            {
                Household newHouseHold = new() {NumberOfChildren = household.NumberOfChildren, NumberOfPersons = household.NumberOfPersons };
                newHouseHold.AddCreatedInfo(sid);
                newVisit.Households.Add(newHouseHold);
            }

            newVisit.AddCreatedInfo(sid);

            await _repository.Create(newVisit);

            return newVisit;
        }

        public async ValueTask<Visit> Cancel(Guid id, string sid)
        {
            var item = await GetById(id, sid);

            item.IsCanceled = true;
            
            item.AddupdatedInfo(sid);

            await _repository.SaveItem(item);

            return item;
        }

        public async ValueTask<List<Visit>> GetAll(string sid)
        {
            throw new NotImplementedException();
        }

        public async ValueTask<Visit> GetById(Guid id, string sid)
        {
            var item = await _repository.GetById(id);
        
            if (item.OwnerSid != sid)
            {
                throw new SecurityException();
            }

            return item;
        }
    }
}
