using komm_rein.api.Repositories;
using komm_rein.api.Services;
using komm_rein.model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace komm_rein.api
{
    public class Startup
    {
        public const string API_NAME = "komm-rein.api";
        
        public const string API_NAME_PUB = "public.komm-rein.api";

        public const string AUTH_POLICY = "api-auth";

        public const string API_VERSION = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KraDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
            services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   Configuration.GetSection("JwtBearer").Bind(options);
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = false
                   };
               });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(API_VERSION, new OpenApiInfo { Title = API_NAME, Version = API_VERSION });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AUTH_POLICY, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", API_NAME);
                    policy.RequireClaim("scope", "email");
                });
            });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    var origins = Configuration.GetSection("Cors:Origins").Get<string[]>();
                    policy.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddScoped<IProtectionService, ProtectionService>();
            services.AddScoped<IVisitRepository, VisitRepository>();
            services.AddScoped<IFacilityRepository, FacilityRepository>();
            services.AddScoped<IFacilityService, FacilityService>();
            services.AddScoped<ISearchService<Facility>, BasicFacilitySearch>();
            services.AddScoped<IVisitService, VisitService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{API_VERSION}/swagger.json", $"{API_NAME} {API_VERSION}"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("default");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    // TODO: needs role claims
                    //.RequireAuthorization(AUTH_POLICY)
                    ;
            });
        }
    }
}
