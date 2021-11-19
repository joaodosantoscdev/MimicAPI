using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimicAPI.Database;
using MimicAPI.V1.Repositories;
using MimicAPI.V1.Repositories.Interfaces;
using AutoMapper;
using MimicAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.Linq;
using System;
using MimicAPI.Helpers.Swagger;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace MimicAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //AUTO-MAPPER CONFIG
            #region  
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMapperProfile());
            });
            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            // CONVENTIONS MVC
            services.AddMvc(cfg => {
                cfg.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
            });

            services.AddDbContext<MimicContext>(options => {
                options.UseSqlite("Data Source=Database\\Mimic.db");
            });

            services.AddControllers();

            services.AddScoped<IWordRepository, WordRepository>();

            // API VERSIONING CONFIG //
            #region
            services.AddApiVersioning(cfg => {
                cfg.ReportApiVersions = true;
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
            });

            #endregion

            // SWAGGER CONFIG
            #region

            services.AddSwaggerGen(cfg => {
                cfg.ResolveConflictingActions(apiDescription => apiDescription.First());
                cfg.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));
                cfg.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "MimicAPI - V1",
                    Version = "v1"
                });

                cfg.SwaggerDoc("v2", new OpenApiInfo() {
                    Title = "MimicAPI - V2",
                    Version = "v2"
                });
            });

            #endregion



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStatusCodePages();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSwagger(); // /swagger/v1/swagger.json
            app.UseSwaggerUI(cfg => {
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "MimicAPI v1.0");
                cfg.SwaggerEndpoint($"/swagger/v2/swagger.json", "MimicAPI - v2.0");
                cfg.RoutePrefix = String.Empty;
            });
        }
    }
}
