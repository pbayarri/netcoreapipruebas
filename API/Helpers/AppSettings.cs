using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string CorsAllowedOrigins { get; set; }
        public string CorsAllowedMethods { get; set; }
        public string CorsAllowedHeaders { get; set; }
        public string SessionTimeoutInSeconds { get; set; }
        public string LogShowPassword { get; set; }
        public string LogShowToken { get; set; }
    }
}
