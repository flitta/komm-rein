﻿using komm_rein.api.Repositories;
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

        //public VisitService(IVisitRepository repository, IFacilityService facilityService)
        //{
        //    _repository = repository;
        //    _facilityService = facilityService;
        //}
        public VisitService(IVisitRepository repository, IFacilityService facilityService, IFacilityRepository facilityRepository , IProtectionService protectionService)
        {
            _protectionService = protectionService;
            _repository = repository;
            _facilityService = facilityService;
            _facilityRepository = facilityRepository;
        }

        public async ValueTask<Signed<Visit>> BookVisit(Signed<Slot> signedSlot, int pax, int kids, string sid)
        {
            // check if valid
            Slot slot;
            
            if (_protectionService.Verify(signedSlot.Signature, signedSlot.Payload))
            {
                slot = signedSlot.Payload;
            }
            else
            {
                throw new ArgumentException("Wrong signature for slot");
            }

            var facility = await _facilityRepository.GetWithSettings(slot.FacilityId);

            Visit visit = facility.Settings.CreateVisit(pax, kids);
            visit.From = slot.From;
            visit.To = slot.To;
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
            var result = new Signed<Visit>(dto, _protectionService.Sign(dto)); ;
            
            return result;
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
