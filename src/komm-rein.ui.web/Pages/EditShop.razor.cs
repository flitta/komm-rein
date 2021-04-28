
using komm_rein.model;
using kommrein.ui.web.Services;
using kommrein.ui.web.Theme;
using kommrein.ui.web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using kommrein.ui.web.Helper;

namespace komm_rein.ui.web.Pages
{
    [Authorize] 
    public partial class EditShop
    {
        [Parameter]
        public string ID { get; set; }

        protected EditContext editContext;
        protected EditFacilityViewModel model = new() { OpeningHours = new List<OpeningHours>()};

        protected bool loaded = false;

        [Inject]
        protected IFacilityService _service { get; set; }

        protected override void OnInitialized()
        {
            editContext = new EditContext(model);
            editContext.SetFieldCssClassProvider(new BsFieldCssClassProvider());
        }

        protected override async Task OnInitializedAsync()
        {
            var item = await _service.GetWithSetting(new Guid(ID));

            model = item.ToEditViewModel();
            loaded = true;
            StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            var item = model.ToModel();
            await _service.Update(item);
            await _service.UpdateOpeningHours(item);
            await _service.UpdateSettings(item);
        }

        protected void AddOpeningHours()
        {
            OpeningHours newItem = new();

            bool valid = true;
            
            DateTime from, to;

            valid &= DateTime.TryParse(model.NewOpeningHours.From, out from);
            valid &= DateTime.TryParse(model.NewOpeningHours.To, out to);

            if (valid)
            {
                newItem.From = from;
                newItem.To = to;

                newItem.DayOfWeek |= model.NewOpeningHours.Monday ? komm_rein.model.DayOfWeek.Monday : newItem.DayOfWeek;
                newItem.DayOfWeek |= model.NewOpeningHours.Tuesday ? komm_rein.model.DayOfWeek.Tuesday : newItem.DayOfWeek;
                newItem.DayOfWeek |= model.NewOpeningHours.Wednesday ? komm_rein.model.DayOfWeek.Wednesday : newItem.DayOfWeek;
                newItem.DayOfWeek |= model.NewOpeningHours.Thursday ? komm_rein.model.DayOfWeek.Thursday : newItem.DayOfWeek;
                newItem.DayOfWeek |= model.NewOpeningHours.Friday ? komm_rein.model.DayOfWeek.Friday : newItem.DayOfWeek;
                newItem.DayOfWeek |= model.NewOpeningHours.Saturday ? komm_rein.model.DayOfWeek.Saturday : newItem.DayOfWeek;
                newItem.DayOfWeek |= model.NewOpeningHours.Sunday ? komm_rein.model.DayOfWeek.Sunday : newItem.DayOfWeek;
            }

            model.OpeningHours.Add(newItem);
            model.NewOpeningHours = OpeningHoursViewModel.Create();

            StateHasChanged();
        }
    }
}
