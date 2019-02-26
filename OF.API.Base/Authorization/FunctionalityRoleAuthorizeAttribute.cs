using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OF.API.Base.Authorization
{
    public class FunctionalityRoleAuthorizedAttribute : AuthorizeAttribute
    {
        public const string POLICY_PREFIX = "FunctionalityRole.";

        public FunctionalityRoleAuthorizedAttribute(string functionality) => Functionality = functionality;

        public string Functionality
        {
            get
            {
                return Policy.Substring(POLICY_PREFIX.Length);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }
}
