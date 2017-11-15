// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace ResourceServer.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Another()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var tokenClient = new TokenClient("http://localhost:5000/connect/token", "api1.client", "secret");
            // send custom grant to token endpoint, return response
            var newToken = await tokenClient.RequestCustomGrantAsync("delegation", "api2", new { token = accessToken });

            var client = new HttpClient();
            client.SetBearerToken(newToken.AccessToken);
            var content = await client.GetStringAsync("http://localhost:5004/identity/");

            return this.Ok(content);
        }
    }
}