using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OF.API.Base.Authorization
{
    public class FunctionalityHandler : AuthorizationHandler<FunctionalityRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FunctionalityRequirement requirement)
        {
            var funcionalitiesList = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            if (funcionalitiesList != null && funcionalitiesList.Any() && funcionalitiesList.Contains(requirement.FuncionalityName))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}