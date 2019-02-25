using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PermissionHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var requirement in pendingRequirements)
            {
                Claim functionalities = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                var funcionalitiesList = functionalities.Value.Split(";");

                if (funcionalitiesList.Contains(((API.Helpers.FunctionalityRequirement)requirement).FuncionalityName))
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
