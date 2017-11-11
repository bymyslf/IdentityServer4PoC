using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ResourceServer.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> values;

        static ValuesController()
        {
            values = new List<string>
            {
                "OpenID Connect",
                "OAuth 2.0"
            };
        }

        [Authorize("ReadOnly")]
        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok(values);
        }

        [Authorize("FullAccess")]
        [HttpPost]
        public IActionResult Post([FromForm]string value)
        {
            values.Add(value);
            return this.Ok(values);
        }
    }
}