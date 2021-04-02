
using komm_rein.model;
using kommrein.ui.web.Services;
using kommrein.ui.web.Theme;
using kommrein.ui.web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

        protected List<Signed<Slot>> slots { get; set; }

        protected bool loaded = false;

        [Inject]
        protected IBookingService _service { get; set; }

        [Inject]
        protected IVisitService _visitService { get; set; }

        
        protected override async Task OnInitializedAsync()
        {
        
            slots = await _service.FindSlots(name, DateTime.Today, 2, null);
           
            loaded = true;
            StateHasChanged();
        }

        protected async Task BookSlot(Signed<Slot> slot)
        {
            await _visitService.BookForSlot(name, slot);
        }
             
    }
}
