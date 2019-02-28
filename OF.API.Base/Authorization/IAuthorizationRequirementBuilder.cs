using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OF.API.Base.Authorization
{
    public interface IAuthorizationRequirementBuilder
    {
        IAuthorizationRequirement Build(string policyName);
        bool CanManage(string policyName);
    }
}
