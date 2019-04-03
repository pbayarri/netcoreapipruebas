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
using OF.API.Base.Utils;
using OF.API.Front.Helpers;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using System;

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
            services.AddOData();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(2, 0);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
            });
            services.AddAutoMapper();

            // los settings
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            // autenticado por JWT
            var key = appSettings.Secret;
            services.AddJwtAuthentication<User, Session>(key, Util.IsTrue(appSettings.AuthValidateIP), Util.IsTrue(appSettings.AuthValidateChangedPass), Util.ToInt(appSettings.SessionTimeoutInSeconds));

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
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ISessionServiceBasic<Session>, SessionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleServiceBasic, RoleService>();
            services.AddScoped<IApiKeyService, ApiKeyService>();
            services.AddScoped<IApiKeyServiceBasic<ApiKey>, ApiKeyService>();
            services.AddScoped<IApiInfoService, ApiInfoService>();

            services.AddSingleton<ILoggerFilters, LogConfigurationService>();
            services.AddSingleton<IHateoasHelper, HateoasHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IHateoasHelper hateoasHelper, IApiInfoService apiInfoService)
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
            app.UseMiddleware<ApiKeyMiddleware<User, ApiKey>>();
            loggerFactory.AddLog4Net();

            app.UseHttpsRedirection();
            app.UseMvc(b =>
            {
                b.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                b.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
            app.UseApiVersioning();

            // Inicializando los colaboradores para Hateoas según las APIs que hayamos configurado
            apiInfoService.GetAll().ToList().ForEach(installedApi => hateoasHelper.AddCollaborator(installedApi.ApiType, installedApi.BaseHref));
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<User>("Users");
            builder.EntitySet<RoleFuncionality>("Funcionalities");
            return builder.GetEdmModel();
        }
    }
}
