
using komm_rein.model;
using kommrein.ui.web.Services;
using kommrein.ui.web.Theme;
using kommrein.ui.web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;



namespace komm_rein.ui.web.Pages
{
    [Authorize]
    public partial class ViewVisit
    {
        protected bool loaded = false;

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IVisitService _service { get; set; }

        [Parameter]
        public string ID { get; set; }

        protected VisitViewModel model = new VisitViewModel();

        protected override async Task OnInitializedAsync()
        {
            var signed = await _service.GetSigned(new Guid(ID));

            model = new()
            {
                Signature = signed.Signature,
                ID = signed.Payload.ID,
                FacilityID = signed.Payload.Facility.ID,
                Name = signed.Payload.Facility.Name,
                From = signed.Payload.From,
                To = signed.Payload.To,
                PaxCount = signed.Payload.Households.Sum(h => h.NumberOfPersons + h.NumberOfChildren),
                VerificationUri = $"https://terminshopping.app/verifizieren/{signed.Payload.Facility.ID}/{signed.Payload.ID}/{signed.Signature}",
            };

            loaded = true;
            StateHasChanged();
        }

        protected async Task Cancel()
        {
            await _service.Cancel(new komm_rein.model.Visit() { ID = model.ID });
            NavigationManager.NavigateTo("/meine-termine");
        }

    }
}
