using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OF.API.Base.Authorization
{
    public class APIAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
        public IEnumerable<IAuthorizationRequirementBuilder> AuthorizationRequirementBuilderCollection { get; set; }
        public APIAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IEnumerable<IAuthorizationRequirementBuilder> authorizationRequirementBuilderCollection)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            AuthorizationRequirementBuilderCollection = authorizationRequirementBuilderCollection;
        }


        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var authRequirementbuilder = AuthorizationRequirementBuilderCollection.Where(b => b.CanManage(policyName)).FirstOrDefault();

            if (authRequirementbuilder != null)
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(authRequirementbuilder.Build(policyName));
                return Task.FromResult(policy.Build());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
