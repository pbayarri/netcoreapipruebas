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
        public enum Functionalities
        {
            GetAllUsers,
            GetOneUser
        }

        public static System.Action<AuthorizationOptions> GetRoleFunctionalityOptions()
        {
            return options =>
            {
                foreach (var functionality in System.Enum.GetValues(typeof(Functionality.Functionalities)))
                {
                    string policyName = $"{FunctionalityRoleAuthorizedAttribute.POLICY_PREFIX}{functionality.ToString()}";
                    options.AddPolicy(policyName, policy =>
                        policy.Requirements.Add(new FunctionalityRequirement(policyName)));
                }
            };
        }

        public static void AddRoleFunctionalities(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationRequirementBuilder, FunctionalityRequirementBuilder>();
            services.AddSingleton<IAuthorizationPolicyProvider, APIAuthorizationPolicyProvider>();

            services.AddSingleton<IAuthorizationHandler, FunctionalityHandler>();
        }
    }
}
