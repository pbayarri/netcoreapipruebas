using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Helpers;
using API.Services;
using Microsoft.Extensions.Logging;
using OF.API.Base.Authentication;
using OF.API.Base.Authorization;
using OF.API.Base.Log;
using OF.API.Base.Cors;
using API.Entities;
using OF.API.Base.Swagger;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper();

            // los settings
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            // autenticado por JWT
            var key = appSettings.Secret;
            services.AddJwtAuthentication<User>(key);

            // autorizado por funcionalidades
            services.AddRoleFunctionalities();

            // swagger
            services.AddSwaggerGenerationInfo(
                new SwaggerDocInfo(
                    appSettings.SwaggerDocInfoVersion, 
                    appSettings.SwaggerDocInfoTitle, 
                    appSettings.SwaggerDocInfoDescription, 
                    appSettings.SwaggerDocInfoTermsOfService, 
                    appSettings.SwaggerDocInfoContactName, 
                    appSettings.SwaggerDocInfoContactEmail, 
                    appSettings.SwaggerDocInfoContactUrl));

            // los servicios en la ID
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserServiceBasic<User>, UserService>();
            services.AddSingleton<ILoggerFilters, LogConfigurationService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            app.AddCors(appSettings.CorsAllowedOrigins, appSettings.CorsAllowedMethods, appSettings.CorsAllowedHeaders);
            
            app.UseAuthentication();

            app.UseUIOfSwagger(appSettings.SwaggerEndPointJsonUrl, appSettings.SwaggerEndPointTitle);
            
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            loggerFactory.AddLog4Net();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
