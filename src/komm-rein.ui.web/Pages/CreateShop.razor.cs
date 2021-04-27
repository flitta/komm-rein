
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
    [Authorize] 
    public partial class CreateShop
    {
        protected EditContext editContext;
        protected CreateFacilityViewModel model;

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IFacilityService _service { get; set; }

        protected override void OnInitialized()
        {
            model = new();
            editContext = new EditContext(model);
            editContext.SetFieldCssClassProvider(new BsFieldCssClassProvider());
        }

        protected async Task HandleValidSubmit(EditContext editContext)
        {
            try
            {
                Facility facilty = new()
                {
                    Name = model.Name,
                    MainAddress = new()
                    {
                        Street_1 = model.Street,
                        Street_2 = model.Street_2,
                        ZipCode = model.ZipCode,
                        ContactPhone = model.Phone,
                        ContactEmail = model.Email
                    },
                };

                var result = await _service.Create(facilty);
                NavigationManager.NavigateTo($"shop-verwalten/{result.ID}");
            }
            catch(Exception ex)
            {
                NavigationManager.NavigateTo($"shop-erstellen");
            }
            
        }

    }
}
