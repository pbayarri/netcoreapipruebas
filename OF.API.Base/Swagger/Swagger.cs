using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OF.API.Base.Swagger
{
    public static class Swagger
    {
        private const string BearerKey = "Bearer";

        public static void AddSwaggerGenerationInfo(this IServiceCollection services, SwaggerDocInfo docInfo)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(docInfo.Version, new Info
                {
                    Version = docInfo.Version,
                    Title = docInfo.Title,
                    Description = docInfo.Description,
                    TermsOfService = docInfo.TermsOfService,
                    Contact = new Contact() { Name = docInfo.ContactName, Email = docInfo.ContactEmail, Url = docInfo.ContactUrl }
                });
                c.AddSecurityDefinition(BearerKey, new ApiKeyScheme { In = "header", Description = "Please Enter Authentication Token: Bearer {Token}", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { BearerKey, Enumerable.Empty<string>() },
                });
            });
        }

        public static void UseUIOfSwagger(this IApplicationBuilder app, string swaggerJsonEndPointUrl, string swaggerJsonEndPointTitle)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerJsonEndPointUrl, swaggerJsonEndPointTitle);
            });
        }
    }
}
