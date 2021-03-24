﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace komm_rein.oidc.Services
{
    public static class IdentityServer4
    {
        public const string API_SCOPE_KOMM_REIN = "komm-rein.api";
        public const string CLIENT_WASM = "wasm";

        public static IServiceCollection AddIds4WithConfig(this IServiceCollection services, IConfiguration configuration)
        {

            IdentityServer4Options ids4Options = new();
            configuration.GetSection(IdentityServer4Options.SECTION).Bind(ids4Options);

            var ids4Builder = services.AddIdentityServer(configuration)
              .AddInMemoryIdentityResources(new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            })
              .AddDeveloperSigningCredential()        //TODO: This is for dev only scenarios when you don’t have a certificate to use.
              .AddInMemoryApiScopes(new ApiScope[]
            {
              new ApiScope(API_SCOPE_KOMM_REIN),
            })
              .AddInMemoryClients(new List<Client>
            {
              new Client
                {
                    ClientId = CLIENT_WASM,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = { ids4Options.ClientUrl },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        API_SCOPE_KOMM_REIN
                    },
                    RedirectUris = { $"{ids4Options.ClientUrl}/authentication/login-callback" },
                    PostLogoutRedirectUris = { $"{ids4Options.ClientUrl}/authentication/logout-callback" }
                }
            })
            .AddAspNetIdentity<IdentityUser>();

            services.AddAuthentication()
             .AddGoogle("Google", options =>
             {
                 configuration.GetSection("Google").Bind(options);
             });

            // TODO: not recommended for production - you need to store your key material somewhere secure
            ids4Builder.AddDeveloperSigningCredential();

            return services;
        }
    }
}