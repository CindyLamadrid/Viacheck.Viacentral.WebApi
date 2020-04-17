using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Viacheck.Viacentral.Models;

namespace Viam.Viacentral.Controllers.Controllers
{
    [Route("api/Status")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class StatusController : ControllerBase
    {
        private readonly IOptions<ConfigurationModel> _configurations;

        public StatusController(IOptions<ConfigurationModel> configurations)
        {
            _configurations = configurations;
        }

        [HttpGet]
        [Route("GetVersion")]
        public string GetVersion()
        {
            return _configurations.Value.Version;
        }
    }
}
