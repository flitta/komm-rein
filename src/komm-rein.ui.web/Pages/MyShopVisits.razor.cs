
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
    public partial class MyShopVisits
    {
        protected bool loaded = false;

        protected Visit[] visits = new Visit[0];

        [Parameter]
        public string FacilityId { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IFacilityService _service { get; set; }

        protected override async Task OnInitializedAsync()
        {
            visits = await _service.GetMyVisits(new Guid(FacilityId));

            loaded = true;
            StateHasChanged();
        }

        protected void GoTo(Visit item)
        {
            NavigationManager.NavigateTo($"/shop-termin/{new Guid(FacilityId)}/{item.ID}");
        }

    }
}
