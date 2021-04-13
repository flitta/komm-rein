
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
    public partial class MyVisit
    {
        //protected BookSlotViewModel viewModel = new();

        protected bool loaded = false;

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IVisitService _service { get; set; }

        [Parameter]
        public string ID { get; set; }

        protected Visit model = new Visit();


        protected override async Task OnInitializedAsync()
        {
            model = await _service.Get(new Guid(ID));
            loaded = true;
            StateHasChanged();
        }

        protected async Task Cancel()
        {
            //viewModel.BookedVisit = await _service.BookForSlot(viewModel.Name, viewModel.From, viewModel.To, viewModel.PaxCount, viewModel.ChildrenCount);
        }

    }
}
