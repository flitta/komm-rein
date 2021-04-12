using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static IdentityServer4.Models.IdentityResources;

namespace komm_rein.oidc.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<IdentityUser> _claimsFactory;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileService(IUserClaimsPrincipalFactory<IdentityUser> claimsFactory, UserManager<IdentityUser> userManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }

        public UserManager<IdentityUser> UserManager => _userManager;

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.FindFirst("sub").Value;
            var user = await UserManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new SecurityException();
            }

            if (context.RequestedResources.Resources.IdentityResources.OfType<Email>().Any())
            {
                context.IssuedClaims.Add(new System.Security.Claims.Claim("email", user.Email));
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.FindFirst("sub").Value;
            var user = await UserManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
