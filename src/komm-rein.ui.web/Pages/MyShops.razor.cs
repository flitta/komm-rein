
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
    public partial class MyShops
    {
        protected bool loaded = false;

        protected Facility[] facilities = new Facility[0];

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IFacilityService _service { get; set; }

        protected override async Task OnInitializedAsync()
        {
            facilities = await _service.GetMyFacilities();

            loaded = true;
            StateHasChanged();
        }

        protected async Task GoTo(Facility item)
        {
         
            NavigationManager.NavigateTo($"/shop-verwalten/{item.ID}");
        }

    }
}
