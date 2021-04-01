
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
    public partial class Search
    {
        [Parameter]
        public string search { get; set; }

        protected List<Facility> facilities { get; set; } = new List<Facility>();

        protected bool loaded = false;

        [Inject]
        protected IFacilitySearchService _service { get; set; }

     
        protected override async Task OnInitializedAsync()
        {

            facilities = await _service.Search(search);
           
            loaded = true;
            StateHasChanged();
        }
     
    }
}
