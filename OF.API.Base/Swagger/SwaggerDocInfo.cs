using System;
using System.Collections.Generic;
using System.Text;

namespace OF.API.Base.Swagger
{
    public class SwaggerDocInfo
    {
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactUrl { get; set; }

        public SwaggerDocInfo(string version, string title, string description, string termsOfService, string contactName, string contactEmail, string contactUrl)
        {
            Version = version;
            Title = title;
            Description = description;
            TermsOfService = termsOfService;
            ContactName = contactName;
            ContactEmail = contactEmail;
            ContactUrl = contactUrl;
        }
    }
}
