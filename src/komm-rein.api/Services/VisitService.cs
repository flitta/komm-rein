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
        readonly IFacilityRepository _facilityRepository;
        readonly IFacilityService _facilityService;
        readonly IProtectionService _protectionService;

   
        public VisitService(IVisitRepository repository, IFacilityService facilityService, IFacilityRepository facilityRepository , IProtectionService protectionService)
        {
            _protectionService = protectionService;
            _repository = repository;
            _facilityService = facilityService;
            _facilityRepository = facilityRepository;
        }

        public async ValueTask<Signed<Visit>> BookVisit(string name, DateTime from, DateTime to, int pax, int kids, string sid)
        {
            var facility = await _facilityRepository.GetByNameWithAssociations(name);

            Visit visit = facility.Settings.CreateVisit(pax, kids);
            visit.From = from;
            visit.To = to;
            visit.Facility = facility;

            var availableSlots = await _facilityService.GetSlotsForVisit(visit.Facility.ID, visit.From.Date, visit);

            var slotFound = availableSlots.FirstOrDefault(s =>
                    s.From == visit.From
                && s.To == visit.To
                && s.Status != SlotStatus.Invalid
                && s.Status != SlotStatus.Full);

            if (slotFound == null)
            {
                throw new ArgumentException("No available slot found!");
            }
                     
            foreach (Household household in visit.Households)
            {
                household.AddCreatedInfo(sid);
            }

            visit.AddCreatedInfo(sid);

            await _repository.Create(visit);

            var dto = visit.ToDto();
            var result = new Signed<Visit>(dto, _protectionService.Sign(dto));
            
            return result;
        }

        public async ValueTask<Visit> Cancel(Guid id, string sid)
        {
            var item = await _repository.GetByIdForOwner(id, sid);

            if (item == null || item.OwnerSid != sid)
            {
                throw new SecurityException();
            }

            item.IsCanceled = true;

            item.AddupdatedInfo(sid);

            await _repository.SaveItem(item);

            return item;
        }

        public async ValueTask<Visit[]> GetAll(string sid)
        {
            var list = await _repository.GetAllForSid(sid);
            return list.ToArray();
        }

        public async ValueTask<Signed<Visit>> GetByIdForOwner(Guid id, string sid)
        {
            var item = await _repository.GetByIdForOwner(id, sid);

            if (item == null || item.OwnerSid != sid)
            {
                throw new SecurityException();
            }

            Visit visit = item.ToDto();

            return new Signed<Visit>(visit, _protectionService.Sign(visit));
        }
    }
}
