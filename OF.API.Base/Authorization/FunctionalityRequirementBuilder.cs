using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace OF.API.Base.Authorization
{
    public class FunctionalityRequirementBuilder : IAuthorizationRequirementBuilder
    {
        public IAuthorizationRequirement Build(string policyName)
        {
            var functionalityName = policyName.Replace(FunctionalityRoleAuthorizedAttribute.POLICY_PREFIX, string.Empty);
            return new FunctionalityRequirement(functionalityName);
        }

        public bool CanManage(string policyName)
        {
            return policyName.StartsWith(FunctionalityRoleAuthorizedAttribute.POLICY_PREFIX, StringComparison.OrdinalIgnoreCase);
        }
    }
}
