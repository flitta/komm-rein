using komm_rein.model;
using kommrein.ui.web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kommrein.ui.web.Helper
{

    public static class ModelHelper
    {
        public static Facility ToModel(this EditFacilityViewModel viewModel)
        {
            Facility result = new Facility()
            {
                ID = viewModel.ID,
                Name = viewModel.Name,
                MainAddress = new Address()
                {
                    Street_1 = viewModel.Street,
                    Street_2 = viewModel.Street_2,
                    ZipCode = viewModel.ZipCode,
                    ContactEmail = viewModel.Email,
                    ContactPhone = viewModel.Phone,
                    City = viewModel.City
                },
                Settings = new FacilitySettings
                {
                    MaxNumberofVisitors = viewModel.MaxNumberofVisitors,
                    CountingMode = viewModel.CountingMode,
                    SlotStatusThreshold = viewModel.SlotStatusThreshold,
                    SlotSizeMinutes = viewModel.SlotMinutes,
                },
                OpeningHours = viewModel.OpeningHours
            };

            return result;
        }

        public static EditFacilityViewModel ToEditViewModel(this Facility item)
        {
            EditFacilityViewModel result = new EditFacilityViewModel()
            {
                ID = item.ID,
                Name = item.Name,
                City = item.MainAddress.City,
                Street = item.MainAddress.Street_1,
                Street_2 = item.MainAddress.Street_2,
                ZipCode = item.MainAddress.ZipCode,
                Phone = item.MainAddress.ContactPhone,
                Email = item.MainAddress.ContactEmail,
                MaxNumberofVisitors = item.Settings.MaxNumberofVisitors,
                CountingMode = item.Settings.CountingMode,
                SlotMinutes = item.Settings.SlotSizeMinutes,
                SlotStatusThreshold = item.Settings.SlotStatusThreshold,
                OpeningHours = item.OpeningHours ?? new List<OpeningHours>(),
                NewOpeningHours = OpeningHoursViewModel.Create(),
            };

            return result;
        }
    }
}
