
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
        public string ID { get; set; }

        protected List<Slot> slots { get; set; }

        protected bool loaded = false;

        [Inject]
        protected IBookingService _service { get; set; }

     
        protected override async Task OnInitializedAsync()
        {
            var visit = new Visit() { Households = new List<Household> { new Household() { NumberOfPersons = 4 } } };

            slots = await _service.FindSlots(new Guid(ID), DateTime.Today, visit);
           
            loaded = true;
            StateHasChanged();
        }
     
    }
}
