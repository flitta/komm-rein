
using komm_rein.model;
using kommrein.ui.web.Services;
using kommrein.ui.web.Theme;
using kommrein.ui.web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

        protected FindSlotsViewModel viewModel ;

        protected bool loaded = false;

        [Inject]
        protected IBookingService _service { get; set; }

      
        protected IVisitService _visitService { get; set; }

        [Inject]
        protected IServiceProvider _serviceProvider { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            viewModel = new() { Name = name, Day = DateTime.Today };
        }
        
        protected async Task BookSlot(Signed<Slot> slot)
        {
            if (_visitService == null)
            {
                _visitService = _serviceProvider.GetService<IVisitService>();
            }

            await _visitService.BookForSlot(slot, viewModel.PaxCount, viewModel.ChildrenCount);
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
