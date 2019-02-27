using API.Helpers;
using Microsoft.Extensions.Options;
using OF.API.Base.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class LogConfigurationService : ILoggerFilters
    {
        private const string TrueValue = "true";
        private readonly AppSettings _appSettings;
        private List<ILogFilter> _filters;

        public LogConfigurationService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _filters = new List<ILogFilter>();
            if (!ShowPassword())
                _filters.Add(new PasswordLogFilter());
            if (!ShowToken())
                _filters.Add(new TokenLogFilter());
        }

        public IEnumerable<ILogFilter> GetFilters()
        {
            return _filters;
        }

        private bool ShowPassword()
        {
            return _appSettings.LogShowPassword.Equals(TrueValue, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool ShowToken()
        {
            return _appSettings.LogShowToken.Equals(TrueValue, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
