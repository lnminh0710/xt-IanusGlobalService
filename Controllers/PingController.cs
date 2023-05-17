using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IanusGlobalServiceApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("ping")]
    public class PingController : ControllerBase
    {
        public PingController()
        {
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return $"API is alive: {DateTime.Now.ToString()}";
        }
    }
}
