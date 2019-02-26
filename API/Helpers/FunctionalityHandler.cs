using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class FunctionalityHandler : AuthorizationHandler<FunctionalityRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FunctionalityRequirement requirement)
        {
            Claim functionalities = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var funcionalitiesList = functionalities.Value.Split(Functionality.FuncionalitySeparator);

            if (funcionalitiesList.Contains(requirement.FuncionalityName.Substring(FunctionalityRoleAuthorizedAttribute.POLICY_PREFIX.Length)))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}