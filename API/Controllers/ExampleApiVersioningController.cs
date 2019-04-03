using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    //[Route("api/v{version:apiVersion}/helloworld")]
    //[ApiVersion("1.0"/*, Deprecated = true*/)]
    //public class HelloWorldDeprecatedController : Controller
    //{
    //    [HttpGet]
    //    public string Get() => "Hello world!";
    //}

    [Route("api/[controller]")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [ApiVersion("4.0")]
    public class HelloWorldController : Controller
    {
        [HttpGet, MapToApiVersion("2.0")]
        public string Get() => "Hello world v2!";

        [HttpGet, MapToApiVersion("3.0")]
        public string GetV3() => "Hello world v3!";

      /*  [HttpGet, MapToApiVersion("4.0")]
        public string GetV4() => "Hello world v4!";*/
    }
}
