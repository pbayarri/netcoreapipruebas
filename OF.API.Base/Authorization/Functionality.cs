using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OF.API.Base.Authorization
{
    public static class Functionality
    {
        public const string FuncionalitySeparator = ";";
       
        public static void AddRoleFunctionalities(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationRequirementBuilder, FunctionalityRequirementBuilder>();
            services.AddSingleton<IAuthorizationPolicyProvider, APIAuthorizationPolicyProvider>();

            services.AddSingleton<IAuthorizationHandler, FunctionalityHandler>();
        }
    }
}
