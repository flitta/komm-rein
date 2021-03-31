
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
    public partial class CreateShop
    {
        protected EditContext editContext;
        protected Facility model;

        [Inject]
        protected IFacilityService _service { get; set; }

        protected override void OnInitialized()
        {
            model = new() {MainAddress = new Address() };
            editContext = new EditContext(model);
            editContext.SetFieldCssClassProvider(new BsFieldCssClassProvider());
        }

        protected async Task HandleValidSubmit()
        {
            await _service.Create(model);
        }
    }
}
