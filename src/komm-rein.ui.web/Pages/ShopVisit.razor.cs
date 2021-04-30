
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
    public partial class ShopVisit
    {
        protected bool loaded = false;

        protected Visit model = null;

        [Parameter]
        public string FacilityId { get; set; }

        [Parameter]
        public string VisitId { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IFacilityService _service { get; set; }

        protected override async Task OnInitializedAsync()
        {
            model = await _service.GetShopsVisit(new Guid(FacilityId), new Guid(VisitId));

            loaded = true;
            StateHasChanged();
        }
    }
}
