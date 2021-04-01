
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


namespace komm_rein.ui.web.Pages
{
    [Authorize] 
    public partial class EditShop
    {
        [Parameter]
        public string ID { get; set; }

        protected EditContext editContext;
        protected Facility model = new() { MainAddress = new(), Settings = new(), OpeningHours = new List<OpeningHours>()};

        protected OpeningHoursViewModel newOpeningHours;

        protected bool loaded = false;

        [Inject]
        protected IFacilityService _service { get; set; }

        protected override void OnInitialized()
        {
            newOpeningHours = InitNewOpeningHours();
            editContext = new EditContext(model);
            editContext.SetFieldCssClassProvider(new BsFieldCssClassProvider());

         
        }

        private OpeningHoursViewModel InitNewOpeningHours()
        {
            return new()
            {
                From = "",
                To = "",
                Sunday = false,
                Monday = true,
                Tuesday = true,
                Wednesday = true,
                Thursday = true,
                Friday = true,
                Saturday = true,
            };
        }

        protected override async Task OnInitializedAsync()
        {
            model = await _service.GetWithSetting(new Guid(ID));
            if(model.OpeningHours == null)
            {
                model.OpeningHours = new List<OpeningHours>();
            }

            if (model.Settings == null)
            {
                model.Settings = new() { SlotStatusThreshold = .5};
            }

            loaded = true;
            StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            //await _service.Update(model);
            await _service.UpdateOpeningHours(model);
            await _service.UpdateSettings(model);
        }

        protected void AddOpeningHours()
        {
            OpeningHours newItem = new();

            bool valid = true;
            
            DateTime from, to;
            valid &= DateTime.TryParse(newOpeningHours.From, out from);
            valid &= DateTime.TryParse(newOpeningHours.From, out to);

            if (valid)
            {
                newItem.From = from;
                newItem.To = to;

                newItem.DayOfWeek |= newOpeningHours.Monday ? komm_rein.model.DayOfWeek.Monday : newItem.DayOfWeek;
                newItem.DayOfWeek |= newOpeningHours.Tuesday ? komm_rein.model.DayOfWeek.Tuesday : newItem.DayOfWeek;
                newItem.DayOfWeek |= newOpeningHours.Wednesday ? komm_rein.model.DayOfWeek.Wednesday : newItem.DayOfWeek;
                newItem.DayOfWeek |= newOpeningHours.Thursday ? komm_rein.model.DayOfWeek.Thursday : newItem.DayOfWeek;
                newItem.DayOfWeek |= newOpeningHours.Friday ? komm_rein.model.DayOfWeek.Friday : newItem.DayOfWeek;
                newItem.DayOfWeek |= newOpeningHours.Saturday ? komm_rein.model.DayOfWeek.Saturday : newItem.DayOfWeek;
                newItem.DayOfWeek |= newOpeningHours.Sunday ? komm_rein.model.DayOfWeek.Sunday : newItem.DayOfWeek;
            }

            model.OpeningHours.Add(newItem);
            newOpeningHours = InitNewOpeningHours();

            StateHasChanged();
        }
    }
}
