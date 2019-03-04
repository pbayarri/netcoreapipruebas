using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetVersion()
        {
            Assembly assembly = Assembly.Load("API");
            var fields = assembly.GetName().Version.ToString().Split('.');
            string versionInfo = $"{fields[0]}.{fields[1]}.{fields[2]}";
            return Ok(versionInfo);
        }
    }
}
