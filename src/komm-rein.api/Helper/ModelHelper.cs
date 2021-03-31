using komm_rein.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.model
{
    public static class ModelHelper
    {
        public static Facility ToDto(this Facility input, bool withMainAddress = false, bool withSettings = false, bool withOpeningHours = false)
        {
            Func<bool, IList<OpeningHours>, IList<OpeningHours>> openingHours = (wo, inp) => wo ? (inp?.Select(x => x.ToDto()).ToList()) : null;

            var result = new Facility 
            {
                ID = input.ID,
                Name = input.Name,
                IsActive = input.IsActive,
                IsLive = input.IsLive,

                MainAddress = withMainAddress ? input.MainAddress?.ToDto() : null,
                Settings = withSettings ? input.Settings?.ToDto() : null,
                OpeningHours = openingHours(withOpeningHours, input.OpeningHours),
            };

            return result;
        }

        public static Visit ToDto(this Visit input)
        {
            throw new NotImplementedException();
        }

        public static Slot ToDto(this Slot input)
        {
            var result = new Slot
            {
                Facility = input.Facility?.ToDto(),
                From = input.From,
                To = input.To,
                Status = input.Status,
            };

            return result;
        }

        public static Address ToDto(this Address input)
        {
            var result = new Address
            {
                ID = input.ID,
                City = input.City,
                AdditionalInfo = input.AdditionalInfo,
                ContactEmail = input.ContactEmail,
                ContactName = input.ContactName,
                ContactPhone = input.ContactPhone,
                Country = input.Country,
                Region = input.Region,
                Street_1 = input.Street_1,
                Street_2 = input.Street_2,
                Street_3 = input.Street_3,
                ZipCode = input.ZipCode,
            };

            return result;
        }

        public static FacilitySettings ToDto(this FacilitySettings input)
        {
            var result = new FacilitySettings
            {
                ID = input.ID,
                CountingMode = input.CountingMode,
                MaxNumberofVisitors = input.MaxNumberofVisitors,
                SlotSize = input.SlotSize,
                SlotStatusThreshold = input.SlotStatusThreshold,
            };

            return result;
        }

        public static OpeningHours ToDto(this OpeningHours input)
        {
            var result = new OpeningHours
            {
                ID = input.ID,
                From = input.From,
                To = input.To,
                DayOfWeek = input.DayOfWeek,
              
            };

            return result;
        }
    }
}
