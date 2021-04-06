
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
    public partial class FindSlot
    {
        [Parameter]
        public string name { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected FindSlotsViewModel viewModel ;

        protected bool loaded = false;

        [Inject]
        protected IBookingService _service { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            viewModel = new() { Name = name, Day = DateTime.Today };
        }
        
        protected void BookSlot(Slot slot)
        {
            string uri = $"termin-buchen/{viewModel.Name}";

            uri = QueryHelpers.AddQueryString(uri, "from", slot.From.ToInvariantString());
            uri = QueryHelpers.AddQueryString(uri, "to", slot.To.ToInvariantString());
            uri = QueryHelpers.AddQueryString(uri, "pax", viewModel.PaxCount.ToString());
            uri = QueryHelpers.AddQueryString(uri, "kids", viewModel.ChildrenCount.GetValueOrDefault().ToString());

            NavigationManager.NavigateTo(uri);
        }

        protected async Task ExecuteSearch()
        {
            viewModel = new()
            {
                Day = viewModel.Day,
                Name = viewModel.Name,
                PaxCount = viewModel.PaxCount,
                ChildrenCount = viewModel.ChildrenCount,
                Slots = await _service.FindSlots(viewModel.Name, viewModel.Day, viewModel.PaxCount, viewModel.ChildrenCount)
            };


        }
             
    }
}
