using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace komm_rein.ui.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            //builder.Services.AddHttpClient("ServerAPI",
            //    client => client.BaseAddress = new Uri(builder.Configuration["api"]))
            //    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            //builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            //    .CreateClient("ServerAPI"));

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient("ServerAPI",
                    client => client.BaseAddress = new Uri(builder.Configuration["api"]))
                .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });

            await builder.Build().RunAsync();
        }
    }

    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://localhost:44309" },
                scopes: new[] { "openid", "profile", "komm-rein.api"});
        }
    }
}
