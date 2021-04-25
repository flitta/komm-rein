
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
    public partial class MyVisits
    {
        //protected BookSlotViewModel viewModel = new();

        protected bool loaded = false;

        protected Visit[] visits = new Visit[0];

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IVisitService _service { get; set; }

        protected override async Task OnInitializedAsync()
        {
            visits = await _service.GetMyVisits();

            loaded = true;
            StateHasChanged();
        }

        protected async Task GoTo(Visit visit)
        {
         
            NavigationManager.NavigateTo($"/termin/{visit.ID}");
        }

    }
}
