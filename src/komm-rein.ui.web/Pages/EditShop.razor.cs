
using komm_rein.model;
using kommrein.ui.web.Services;
using kommrein.ui.web.Theme;
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
    [Authorize] 
    public partial class EditShop
    {
        [Parameter]
        public string ID { get; set; }

        protected EditContext editContext;
        protected Facility model = new() { MainAddress = new(), Settings = new(), OpeningHours = new List<OpeningHours>()};

        protected bool loaded = false;

        [Inject]
        protected IFacilityService _service { get; set; }

        protected override void OnInitialized()
        {
            editContext = new EditContext(model);
            editContext.SetFieldCssClassProvider(new BsFieldCssClassProvider());

            loaded = true;
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            
            model = await _service.GetWithSetting(new Guid(ID));

        }

        protected async Task HandleValidSubmit()
        {
            await _service.Update(model);
        }
    }
}
