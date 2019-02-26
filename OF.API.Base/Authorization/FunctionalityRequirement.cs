using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OF.API.Base.Authorization
{
    public class FunctionalityRequirement : IAuthorizationRequirement
    {
        public string FuncionalityName { get; private set; }

        public FunctionalityRequirement(string funcionality)
        {
            FuncionalityName = funcionality;
        }
    }
}
