using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Cors
{
    public static class CorsHelper
    {
        public const string AllowAll = "*";
        public const string AllowCorsSeparator = ";";

        public static void AddCors(this IApplicationBuilder app, string allowedOrigins, string allowedMethods, string alloweHeaders)
        {
            app.UseCors(x => {
                if (string.IsNullOrEmpty(allowedOrigins) || allowedOrigins == AllowAll)
                    x.AllowAnyOrigin();
                else
                    x.WithOrigins(allowedOrigins.Split(AllowCorsSeparator));

                if (string.IsNullOrEmpty(allowedMethods) || allowedMethods == AllowAll)
                    x.AllowAnyMethod();
                else
                    x.WithMethods(allowedMethods.Split(AllowCorsSeparator));

                if (string.IsNullOrEmpty(alloweHeaders) || alloweHeaders == AllowAll)
                    x.AllowAnyHeader();
                else
                    x.WithHeaders(alloweHeaders.Split(AllowCorsSeparator));
            });
        }
    }
}
