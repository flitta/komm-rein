using komm_rein.model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.api.Services
{
    public interface IFacilityService
    {
        ValueTask<Facility> GetByIdWithSettings(Guid id, string sid);

        ValueTask<Facility> GetById(Guid id);

        ValueTask<Facility> Create(Facility newItem, string sid);

        ValueTask<Facility> Update(Facility item, string sid);

        ValueTask<FacilitySettings> SetSettings(Guid facilityId, FacilitySettings value, string sid);

        ValueTask<FacilitySettings> GetSettings(Guid facilityId, string ownerSid);

        ValueTask<IList<OpeningHours>> GetOpeningHours(Guid facilityId);

        ValueTask<IList<OpeningHours>> SetOpeningHours(Guid facilityId, OpeningHours[] value, string sid);

        ValueTask<Facility> GetByName(string name);

        ValueTask<List<Facility>> GetAll();

        ValueTask<Slot[]> GetSlotsForVisit(Guid facilityId, DateTime day, Visit visit);

        ValueTask<Slot[]> GetSlotsForVisit(string facilitNname, DateTime day, Visit visit);

        ValueTask<Slot[]> GetSlots(string name, DateTime day, int pax, int? kids);

        ValueTask<Visit> Verify(Guid facilityId, Guid visitId, string signature, string sid);
    }
}
