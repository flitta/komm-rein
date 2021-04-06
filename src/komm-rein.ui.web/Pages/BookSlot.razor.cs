
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
    public partial class BookSlot
    {
        protected BookSlotViewModel viewModel = new();

        protected bool loaded = false;

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IVisitService _service { get; set; }

        [Parameter]
        public string Name { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            viewModel.Name = Name;
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("from", out var from))
            {
                viewModel.From = Convert.ToDateTime(from);
            }

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("to", out var to))
            {
                viewModel.To = Convert.ToDateTime(to);
            }

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("pax", out var pax))
            {
                viewModel.PaxCount = Convert.ToInt32(pax);
            }

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("kids", out var kids))
            {
                viewModel.ChildrenCount = Convert.ToInt32(kids);
            }

        }

        protected async Task Execute()
        {
            viewModel.BookedVisit = await _service.BookForSlot(viewModel.Name, viewModel.From, viewModel.To, viewModel.PaxCount, viewModel.ChildrenCount);
        }

    }
}
