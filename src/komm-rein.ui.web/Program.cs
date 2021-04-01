using kommrein.ui.web.Config;
using kommrein.ui.web.Services;
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

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient(ApiConfig.API_NAME,
                    client => client.BaseAddress = new Uri(builder.Configuration["api"]))
                .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient(ApiConfig.PUBLIC_API_NAME,
                    client => client.BaseAddress = new Uri(builder.Configuration["api"]));

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("oidc", options.ProviderOptions);
            });

            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddScoped<IFacilityService, FacilityService>();
            builder.Services.AddScoped<IFacilitySearchService, FacilitySearchService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            builder.Services.Configure<FacilityApiConfig>(options =>
            {
                options.Path = "Facility";
                options.ApiName = ApiConfig.API_NAME;
            });

            builder.Services.Configure<VisitApiConfig>(options =>
            {
                options.Path = "Visit";
                options.ApiName = ApiConfig.API_NAME;
            });

            builder.Services.Configure<SearchApiConfig>(options =>
            {
                options.Path = "Search";
                options.ApiName = ApiConfig.PUBLIC_API_NAME;
            });

            builder.Services.Configure<BookingApiConfig>(options =>
            {
                options.Path = "Booking";
                options.ApiName = ApiConfig.PUBLIC_API_NAME;
            });

            await builder.Build().RunAsync();
        }
    }

    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(
            IConfiguration configuration,
            IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] 
                {
                    // Facility, Visit APIs require auth, Search is public
                    $"{configuration.GetSection("api").Value}/Facility",
                    $"{configuration.GetSection("api").Value}/Visit",
                },
                scopes: new[] { "openid", "profile", "komm-rein.api" });
        }
    }
}
