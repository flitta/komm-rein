
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
using System.Linq;
using System.Threading.Tasks;



namespace komm_rein.ui.web.Pages
{
    [Authorize]
    public partial class VerifyVisit
    {
        protected bool loaded = false;

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IFacilityService _service { get; set; }

        [Parameter] 
        public string FacilityID { get; set; }

        [Parameter]
        public string VisitID { get; set; }

        [Parameter]
        public string Signature { get; set; }


        protected VisitViewModel model = null;


        protected override async Task OnInitializedAsync()
        {
            var visit = await _service.VerifyVisit(new Guid(FacilityID), new Guid(VisitID), Signature);

            if(visit != null)
            {
                model = new() { ID = visit.ID, Name = visit.Facility.Name, From = visit.From, To = visit.To, PaxCount = visit.Households.Sum(h => h.NumberOfPersons + h.NumberOfChildren) };
            }

            loaded = true;
            StateHasChanged();
        }

    }
}
